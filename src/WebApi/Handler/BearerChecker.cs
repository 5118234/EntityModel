using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Agebull.Common.DataModel.Redis;
using Agebull.Common.Logging;
using GoodLin.Common.Configuration;
using GoodLin.Common.Ioc;
using Newtonsoft.Json;
using Yizuan.Service.Api.OAuth;

namespace Yizuan.Service.Api.WebApi
{
    /// <summary>
    ///     ��ݼ����
    /// </summary>
    public class BearerHandler : IHttpSystemHandler
    {
        /// <summary>
        ///     ��ʼʱ�Ĵ���
        /// </summary>
        /// <returns>����������ݲ�Ϊ�գ�ֱ�ӷ���,�����Ĵ����ټ���</returns>
        Task<HttpResponseMessage> IHttpSystemHandler.OnBegin(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var code = Check(request);
            //У�鲻ͨ����ֱ�ӷ��أ������κδ���
            if (code == 0)
                return null;
            LogRecorder.MonitorTrace("Authorization��У�����");
            return Task.Factory.StartNew(() =>
            {
                var result = ApiResult.Error(code);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(result))
                };
            }, cancellationToken);
        }

        /// <summary>
        ///     ����ʱ�Ĵ���
        /// </summary>
        void IHttpSystemHandler.OnEnd(HttpRequestMessage request, CancellationToken cancellationToken,
            HttpResponseMessage response)
        {
        }

        /// <summary>
        ///     ִ�м��
        /// </summary>
        /// <returns>
        ///     0:��ʾͨ����֤�����Լ���
        ///     1������Ϊ�ջ򲻺ϸ�
        ///     2��������α���
        /// </returns>
        private int Check(HttpRequestMessage request)
        {
            var token = ExtractToken(request);
            if (string.IsNullOrWhiteSpace(token))
                return ErrorCode.DenyAccess;

            switch (token[0])
            {
                case '*':
                    ApiContext.SetRequestContext(new InternalCallContext
                    {
                        RequestId = Guid.NewGuid(),
                        ServiceKey = GlobalVariable.ServiceKey,
                        Bear = token,
                        UserId = -2
                    });
                    return CheckDeviceId(request, token);
                case '{':
                    return CheckServiceKey(request, token);
                case '$':
                    return RevertCallContext(request, token);
                case '#':
                    ApiContext.SetRequestContext(new InternalCallContext
                    {
                        RequestId = Guid.NewGuid(),
                        ServiceKey = GlobalVariable.ServiceKey,
                        Bear = token,
                        UserId = -2
                    });
                    return CheckAccessToken(request, token);
            }
            return ErrorCode.Auth_Device_Unknow;
        }

        /// <summary>
        ///     ����豸��ʶ
        /// </summary>
        /// <returns>
        ///     0:��ʾͨ����֤�����Լ���
        ///     1������Ϊ��
        ///     2��������α���
        /// </returns>
        private int CheckDeviceId(HttpRequestMessage request, string token)
        {
            var checker = IocHelper.Create<IBearValidater>();
            ApiResult<LoginUserInfo> result;
            try
            {
                result = checker.ValidateDeviceId(token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return 0; //ErrorCode.Auth_Device_Unknow;
            }
            if (!result.Result)
                return result.Status.ErrorCode;
            CreateApiContext(result.ResultData, token);
            LogRecorder.MonitorTrace("Authorization�������û�");
            return 0;
        }

        /// <summary>
        ///     ����������
        /// </summary>
        private void CreateApiContext(ILoginUserInfo customer, string token)
        {
            ApiContext.SetCustomer(customer);
            ApiContext.SetRequestContext(new InternalCallContext
            {
                Bear = token,
                Os = customer.Os,
                Browser = customer.Browser,
                RequestId = Guid.NewGuid(),
                ServiceKey = GlobalVariable.ServiceKey,
                UserId = customer.UserId
            });
            ApiContext.Current.Cache();
        }

        /// <summary>
        ///     ��ԭ����������
        /// </summary>
        private int RevertCallContext(HttpRequestMessage request, string token)
        {
            ApiContext context;
            using (var proxy = new RedisProxy())
            {
                context = proxy.Get<ApiContext>(ApiContext.GetCacheKey(token));
            }
            if (context?.Request == null || context.LoginUser == null)
                return ErrorCode.DenyAccess;
            var checker = IocHelper.Create<IBearValidater>();
            var result = checker.ValidateServiceKey(context.Request.ServiceKey);
            if (!result.Result)
                return result.Status.ErrorCode;
            ApiContext.SetContext(context);
            return 0;
        }

        /// <summary>
        ///     ���ɱ�ʶ
        /// </summary>
        /// <returns>
        ///     0:��ʾͨ����֤�����Լ���
        ///     1������Ϊ��
        ///     2��������α���
        /// </returns>
        private int CheckServiceKey(HttpRequestMessage request, string token)
        {
            InternalCallContext context;
            try
            {
                context = JsonConvert.DeserializeObject<InternalCallContext>(token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_ServiceKey_Unknow;
            }
            if (context == null)
                return ErrorCode.Auth_ServiceKey_Unknow;
            var checker = IocHelper.Create<IBearValidater>();
            var result = checker.ValidateServiceKey(context.ServiceKey);
            if (!result.Result)
                return result.Status.ErrorCode;
            var user = checker.GetLoginUser(context.Bear);
            if (!user.Result)
                return user.Status.ErrorCode;
            ApiContext.SetCustomer(user.ResultData);
            ApiContext.SetRequestContext(context);
            ApiContext.Current.Cache();
            LogRecorder.MonitorTrace($"Authorization��{user.ResultData.NickName}");
            return 0;
        }

        /// <summary>
        ///     ���AccessToken
        /// </summary>
        /// <returns>
        ///     0:��ʾͨ����֤�����Լ���
        ///     1������Ϊ��
        ///     2��������α���
        /// </returns>
        private int CheckAccessToken(HttpRequestMessage request, string token)
        {
            var checker = IocHelper.Create<IBearValidater>();
            ApiResult<LoginUserInfo> result;
            try
            {
                result = checker.VerifyAccessToken(token);
            }
            catch (Exception e)
            {
                LogRecorder.Exception(e);
                return ErrorCode.Auth_AccessToken_Unknow;
            }
            if (!result.Result)
                return result.Status.ErrorCode;

            CreateApiContext(result.ResultData, token);

            LogRecorder.MonitorTrace("Authorization��" + result.ResultData.Account);
            return 0;
        }

        /// <summary>
        ///     ȡ����ͷ�������֤����
        /// </summary>
        /// <returns></returns>
        private static string ExtractToken(HttpRequestMessage request)
        {
            const string bearer = "Bearer";
            var authz = request.Headers.Authorization;
            if (authz != null)
                return string.Equals(authz.Scheme, bearer, StringComparison.OrdinalIgnoreCase) ? authz.Parameter : null;
            if (!request.Headers.Contains("Authorization"))
                return null;
            var au = request.Headers.GetValues("Authorization").FirstOrDefault();
            if (au == null)
                return null;
            var aus = au.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (aus.Length < 2 || aus[0] != bearer)
                return null;
            return aus[1].Trim();
        }
    }
}