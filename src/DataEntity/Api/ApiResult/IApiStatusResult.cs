using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// API状态返回（一般在出错时发生）
    /// </summary>
    public interface IApiStatusResult
    {
        /// <summary>
        /// 错误码（系统定义）
        /// </summary>
        [JsonProperty("code")]
        int ErrorCode { get; set; }

        /// <summary>
        /// 对应HTTP错误码（参考）
        /// </summary>
        [JsonProperty("http")]
        string HttpCode { get; set; }

        /// <summary>
        /// 客户端信息
        /// </summary>
        [JsonProperty("msg")]
        string ClientMessage { get; set; }

        /// <summary>
        /// 内部信息
        /// </summary>
        string InnerMessage { get; set; }
    }
}