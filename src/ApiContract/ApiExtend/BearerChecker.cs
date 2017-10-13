using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.Logging;
using Newtonsoft.Json;
using GoodLin.Common.Ioc;
using GoodLin.Common.Configuration;
using GoodLin.Common.Helper;

namespace GoodLin.Common.Api
{
    /// <summary>
    /// ��ݼ����
    /// </summary>
    public class BearerHandler : DelegatingHandler
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            int code = Check();
            //У�鲻ͨ����ֱ�ӷ��أ������κδ���
            if (code != 0)
            {
                LogRecorder.FlushMonitor("AuthorizationͷУ�����");
                return Task.Factory.StartNew(() =>
                {
                    var result = ApiResult.Error(code);
                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(result))
                    };
                }, cancellationToken);
            }
            //��������ʱ�������������
            var resultTask = base.SendAsync(request, cancellationToken);
            resultTask.ContinueWith((task,state)=>
            {
                CallContext.LogicalSetData("ApiContext", null);
            },null,cancellationToken);

            return resultTask;
        }

        /// <summary>
        /// �������
        /// </summary>
        private HttpRequestMessage Request;

        /// <summary>
        /// ����
        /// </summary>
        private string token;
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
            token = ExtractToken();
            if (string.IsNullOrWhiteSpace(token))
                return 1;
            token = token.Trim();
            if (token.Length < 36)
                return 1;
            if (token[0] == '{')
            {
                return CheckDeviceId();
            }
            if (token.Length == 36)
            {
                return CheckServiceKey();
            }
            return CheckAccessToken();
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
            var result = checker.ValidateDeviceId(token);
            if (!result.Result)
                return result.Status.ErrorCode;
            ApiContext.SetCustomer(result.ResultData);
            return 0;
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
                context = JsonConvert.DeserializeObject<InternalCallContext>(token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return 2;
            }

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
            var result = checker.VerifyAccessToken(token);
            if (!result.Result)
                return result.Status.ErrorCode;
            CallContext.LogicalSetData("UserInfo", result.ResultData);
            var ts = Request.Content.ReadAsStringAsync();
            ts.Wait();
            ApiContext.SetRequestContext(new InternalCallContext
            {
                RequestId = Guid.NewGuid(),
                ServiceKey = GlobalVariable.ServiceKey,
                UserId = result.ResultData.UserId,
                RequestArgument = ts.Result
            });
            return 0;
        }

        /// <summary>
        /// ȡ����ͷ�������֤����
        /// </summary>
        /// <returns></returns>
        private string ExtractToken()
        {
            const string Bearer = "Bearer";
            var authz = Request.Headers.Authorization;
            if (authz != null && string.Equals(authz.Scheme, Bearer, StringComparison.OrdinalIgnoreCase))
            {
                return authz.Parameter;
            }
            NameValueCollection nameValues = UrlHelper.GetQueryString(Request.RequestUri.Query);
            return nameValues["ClientKey"];
        }
    }
}