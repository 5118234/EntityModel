using System;
using Newtonsoft.Json;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    /// RPC����
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RpcConfig
    {
        /// <summary>
        /// �����ַ
        /// </summary>
        [JsonProperty("requestUrl")]
        public string RequestUrl { get; set; }
        /// <summary>
        /// �صõ�ַ
        /// </summary>
        [JsonProperty("answerUrl")]
        public string AnswerUrl { get; set; }
        /// <summary>
        /// ֪ͨ��ַ
        /// </summary>
        [JsonProperty("notifyUrl")]
        public string NotifyUrl { get; set; }

        /// <summary>
        /// ���ط�����ȫ���еı�ʶ
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

    }
}