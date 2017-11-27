using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// API״̬���ؽӿ�ʵ��
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiStatsResult : IApiStatusResult
    {
        /// <summary>
        /// Ĭ�Ϲ���
        /// </summary>
        public ApiStatsResult()
        {

        }
        /// <summary>
        /// Ĭ�Ϲ���
        /// </summary>
        public ApiStatsResult(int code, string messgae)
        {
            ErrorCode = code;
            Message = messgae;
        }
        /// <summary>
        /// ������
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int ErrorCode { get; set; }

        /// <summary>
        /// ��ӦHTTP�����루�ο���
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string HttpCode { get; set; }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
        /// <summary>
        /// �ڲ���ʾ��Ϣ
        /// </summary>
#if DEBUG
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
#else
        [JsonIgnore]
#endif
        public string InnerMessage { get; set; }
    }
}