using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Logging;
using GoodLin.Common.Configuration;
using GoodLin.Common.Ioc;
using GoodLin.OAuth.Api;
using Newtonsoft.Json;

namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    /// ��ݼ����
    /// </summary>
    public class BearerHandler : DelegatingHandler
    {
        static BearerHandler()
        {
            LogRecorder.GetRequestIdFunc = () => ApiContext.RequestContext?.RequestId ?? Guid.NewGuid();
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _request = request;
            int code = Check();
            //У�鲻ͨ����ֱ�ӷ��أ������κδ���
            if (code != 0)
            {
                LogRecorder.MonitorTrace("Authorization��У�����");
                return Task.Factory.StartNew(() =>
                {
                    var result = code == ErrorCode.Auth_Device_Unknow ? ApiResult.Error(code, "*" + Guid.NewGuid().ToString("N")) : ApiResult.Error(code);
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result))
                    };
                }, cancellationToken);
            }
            var tid = ApiContext.RequestContext.ThreadId;
            //��������ʱ�������������
            var resultTask = base.SendAsync(request, cancellationToken);
            resultTask.ContinueWith((task, state) =>
            {
                Debug.Assert(ApiContext.RequestContext.ThreadId == tid);
                CallContext.LogicalSetData("ApiContext", null);

            }, null, cancellationToken);

            return resultTask;
        }

        /// <summary>
        /// �������
        /// </summary>
        private HttpRequestMessage _request;

        /// <summary>
        /// ����
        /// </summary>
        private string _token;
        /// <summary>
        /// ִ�м��
        /// </summary>
        /// <returns>
        /// 0:��ʾͨ����֤�����Լ���
        /// 1������Ϊ�ջ򲻺ϸ�
        /// 2��������α���
        /// </returns>
        private int Check()
        {
            _token = ExtractToken();
            if (string.IsNullOrWhiteSpace(_token))
                return ErrorCode.Auth_Device_Unknow;
            
            ApiContext.SetRequestContext(new InternalCallContext
            {
                RequestId = Guid.NewGuid(),
                ServiceKey = GlobalVariable.ServiceKey,
                UserId = -2
            });
            _token = _token.Trim();
            switch (_token[0])
            {
                case '*':
                    return CheckDeviceId();
                case '{':
                    return CheckServiceKey();
                case '#':
                    return CheckAccessToken();
            }
            return ErrorCode.Auth_Device_Unknow;
        }
        /// <summary>
        /// ����豸��ʶ
        /// </summary>
        /// <returns>
        /// 0:��ʾͨ����֤�����Լ���
        /// 1������Ϊ��
        /// 2��������α���
        /// </returns>
        int CheckDeviceId()
        {
            var checker = IocHelper.Create<IBearValidater>();
            IApiResult<IUserProfile> result;
            try
            {
                result = checker.ValidateDeviceId(_token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_Device_Unknow;
            }
            if (!result.Result)
                return result.Status.ErrorCode;
            CreateApiContext(result.ResultData);
            LogRecorder.MonitorTrace("Authorization�������û�");
            return 0;
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="customer"></param>
        private void CreateApiContext(IUserProfile customer)
        {
            //var ts = _request.Content.ReadAsStringAsync();
            //ts.Wait();
            ApiContext.SetCustomer(customer);
            ApiContext.SetRequestContext(new InternalCallContext
            {
                RequestId = Guid.NewGuid(),
                ServiceKey = GlobalVariable.ServiceKey,
                UserId = customer?.UserId ?? -1,
                ThreadId = Thread.CurrentThread.ManagedThreadId
            });
        }

        /// <summary>
        /// ���ɱ�ʶ
        /// </summary>
        /// <returns>
        /// 0:��ʾͨ����֤�����Լ���
        /// 1������Ϊ��
        /// 2��������α���
        /// </returns>
        int CheckServiceKey()
        {
            InternalCallContext context;
            try
            {
                context = JsonConvert.DeserializeObject<InternalCallContext>(_token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_ServiceKey_Unknow;
            }
            if (context == null)
            {
                return ErrorCode.Auth_ServiceKey_Unknow;
            }
            context.ThreadId = Thread.CurrentThread.ManagedThreadId;
            var checker = IocHelper.Create<IBearValidater>();
            var result = checker.ValidateServiceKey(context.ServiceKey.ToString());
            if (!result.Result)
                return result.Status.ErrorCode;
            if (context.UserId > 0)
            {
                var user = checker.GetUserProfile(context.UserId);
                if (!user.Result)
                    return user.Status.ErrorCode;
                ApiContext.SetCustomer(user.ResultData);
                LogRecorder.MonitorTrace("Authorization��"+user.ResultData.PhoneNumber);
            }
            else
            {
                ApiContext.SetCustomer(new UserProfile
                {
                    UserId = context.UserId,
                });
                LogRecorder.MonitorTrace("Authorization�������û�");
            }
            ApiContext.SetRequestContext(context);
            return 0;
        }

        /// <summary>
        /// ���AccessToken
        /// </summary>
        /// <returns>
        /// 0:��ʾͨ����֤�����Լ���
        /// 1������Ϊ��
        /// 2��������α���
        /// </returns>
        int CheckAccessToken()
        {
            var checker = IocHelper.Create<IBearValidater>();
            IApiResult<IUserProfile> result;
            try
            {
                result = checker.VerifyAccessToken(_token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_AccessToken_Unknow;
            }
            if (!result.Result)
                return result.Status.ErrorCode;

            CreateApiContext(result.ResultData);

            LogRecorder.MonitorTrace("Authorization��" + result.ResultData.PhoneNumber);
            return 0;
        }

        /// <summary>
        /// ȡ����ͷ�������֤����
        /// </summary>
        /// <returns></returns>
        private string ExtractToken()
        {
            const string bearer = "Bearer";
            var authz = _request.Headers.Authorization;
            if (authz != null)
                return string.Equals(authz.Scheme, bearer, StringComparison.OrdinalIgnoreCase) ? authz.Parameter : null;
            if (!_request.Headers.Contains("Authorization"))
                return null;
            string au = _request.Headers.GetValues("Authorization").FirstOrDefault();
            if (au == null)
                return null;
            var aus = au.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (aus.Length < 2 || aus[0] != bearer)
                return null;
            return aus[1];
        }
    }
}