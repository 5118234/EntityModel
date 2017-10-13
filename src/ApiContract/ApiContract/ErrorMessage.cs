﻿using System.Collections.Generic;
// ReSharper disable All


namespace Yizuan.Service.Api
{
    /// <summary>
    /// 系统错误代码
    /// </summary>
    public class ErrorCode
    {
        #region 消息字典

        static readonly Dictionary<int, string> Map = new Dictionary<int, string>
        {
            {Auth_ServiceKey_Unknow,"未知的ServiceKey" },
            {Auth_RefreshToken_Unknow,"未知的RefreshToken" },
            {Auth_AccessToken_Unknow,"未知的AccessToken" },
            {Auth_User_Unknow,"未知的用户" },
            {Auth_Device_Unknow,"未知的设备识别码" },
            {Auth_AccessToken_TimeOut,"令牌过期" },
        };
        /// <summary>
        /// 取得错误码对应的消息文本
        /// </summary>
        /// <param name="eid">错误码</param>
        /// <returns>消息文本</returns>
        public static string GetMessage(int eid)
        {
            switch (eid)
            {
                case 0:
                    return "操作成功";
                case -1:
                    return "未知错误";
                case ArgumentError:
                    return "参数错误";
                case NetworkError:
                    return "网络异常";
                case InnerError:
                    return "服务器内部错误";
            }
            
            string result;
            return Map.TryGetValue(eid, out result) ? result : "未知错误";
        }

        #endregion

        /// <summary>
        /// 正确
        /// </summary>
        public const int Success = 0;

        /// <summary>
        /// 未知错误
        /// </summary>
        public const int UnknowError = -1;

        /// <summary>
        /// 参数错误
        /// </summary>
        public const int ArgumentError = -2;

        /// <summary>
        /// 网络错误
        /// </summary>
        public const int NetworkError = -3;

        /// <summary>
        /// 服务器内部错误
        /// </summary>
        public const int InnerError = -4;

        /// <summary>
        /// 拒绝访问
        /// </summary>
        public const int DenyAccess = -5;

        /// <summary>
        /// 未知的RefreshToken
        /// </summary>
        public const int Auth_RefreshToken_Unknow = 40083;

        /// <summary>
        /// 未知的ServiceKey
        /// </summary>
        public const int Auth_ServiceKey_Unknow = 40082;

        /// <summary>
        /// 未知的AccessToken
        /// </summary>
        public const int Auth_AccessToken_Unknow = 40081;
        
        /// <summary>
        /// 未知的用户
        /// </summary>
        public const int Auth_User_Unknow = 40421;

        /// <summary>
        /// 未知的设备识别码
        /// </summary>
        public const int Auth_Device_Unknow = 40022;

        /// <summary>
        /// 令牌过期
        /// </summary>
        public const int Auth_AccessToken_TimeOut = 40036;
        

        //public const int iError_40033 = 40033;
        //public const string sError_40033 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,verify access token or create access token\",error=\"invalid_userId\", error_description=\"The user id is invalidate!\"";

        //public const int iError_40034 = 40034;
        //public const string sError_40034 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,verify access token\",error=\"invalid_appId\", error_description=\"The app id is invalidate!\"";

        //public const int iError_40035 = 40035;
        //public const string sError_40035 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,verify access token\",error=\"invalid_access_token\", error_description=\"The access token is invalidate!\"";

        //public const int iError_40036 = 40036;
        //public const string sError_40036 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,verify access token\",error=\"expires_access_token\", error_description=\"The access token is expires!\"";

        //public const int iError_40038 = 40038;
        //public const string sError_40038 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,verify access token\",error=\"formart_access_token\", error_description=\"Formart the access token error!\"";

        //public const int iError_40056 = 40056;
        //public const string sError_40056 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,refresh access token\",error=\"expires_refresh_token\", error_description=\"The refresh token is expires!\"";

        //public const int iError_40057 = 40057;
        //public const string sError_40057 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,refresh access token\",error=\"invalid_access_token\", error_description=\"The access token is invalidate!\"";

        //public const int iError_40058 = 40058;
        //public const string sError_40058 = "Exception realm=\"GoodLin.OAuth.InternalService.Api,refresh access token\",error=\"invalid_refresh_token\", error_description=\"The refresh token is invalidate!\"";

    }
}
