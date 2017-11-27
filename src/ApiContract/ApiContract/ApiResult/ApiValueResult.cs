using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// API�������ݷ�����
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiValueResult : ApiResult
    {
        /// <summary>
        /// ����ֵ
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ResultData { get; set; }

        /// <summary>
        /// ����һ���ɹ��ı�׼����
        /// </summary>
        /// <returns></returns>
        public static ApiValueResult Succees(string data)
        {
            return new ApiValueResult
            {
                Result = true,
                ResultData = data
            };
        }
        /// <summary>
        /// ����һ������������ı�׼����
        /// </summary>
        /// <param name="errCode">������</param>
        /// <returns></returns>
        public static ApiValueResult ErrorResult(int errCode)
        {
            return new ApiValueResult
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = ErrorCode.GetMessage(errCode)
                }
            };
        }
        /// <summary>
        /// ����һ������������ı�׼����
        /// </summary>
        /// <param name="errCode">������</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiValueResult ErrorResult(int errCode, string message)
        {
            return new ApiValueResult
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message
                }
            };
        }

        /// <summary>
        /// ����һ������������ı�׼����
        /// </summary>
        /// <param name="errCode">������</param>
        /// <param name="message"></param>
        /// <param name="innerMessage"></param>
        /// <returns></returns>
        public static ApiValueResult ErrorResult(int errCode, string message, string innerMessage)
        {
            return new ApiValueResult
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message,
                    InnerMessage = innerMessage
                }
            };
        }
    }


    /// <summary>
    /// API���ص��������ݷ�����
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiValueResult<TData> : ApiResult
    {
        /// <summary>
        /// ����ֵ
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData ResultData { get; set; }

        /// <summary>
        /// ����һ���ɹ��ı�׼����
        /// </summary>
        /// <returns></returns>
        public static ApiValueResult<TData> Succees(TData data)
        {
            return new ApiValueResult<TData>
            {
                Result = true,
                ResultData = data
            };
        }
        /// <summary>
        /// ����һ������������ı�׼����
        /// </summary>
        /// <param name="errCode">������</param>
        /// <returns></returns>
        public static ApiValueResult<TData> ErrorResult(int errCode)
        {
            return new ApiValueResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = ErrorCode.GetMessage(errCode)
                }
            };
        }
        /// <summary>
        /// ����һ������������ı�׼����
        /// </summary>
        /// <param name="errCode">������</param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ApiValueResult<TData> ErrorResult(int errCode, string message)
        {
            return new ApiValueResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message
                }
            };
        }

        /// <summary>
        /// ����һ������������ı�׼����
        /// </summary>
        /// <param name="errCode">������</param>
        /// <param name="message"></param>
        /// <param name="innerMessage"></param>
        /// <returns></returns>
        public static ApiValueResult<TData> ErrorResult(int errCode, string message, string innerMessage)
        {
            return new ApiValueResult<TData>
            {
                Result = false,
                Status = new ApiStatsResult
                {
                    ErrorCode = errCode,
                    Message = message,
                    InnerMessage = innerMessage
                }
            };
        }
    }
}