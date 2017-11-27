using System.Text;
using Newtonsoft.Json;
using System.Web;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// 请求参数
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Argument : IApiArgument
    {
        /// <summary>
        /// AT
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }



        string IApiArgument.ToFormString()
        {
            StringBuilder code = new StringBuilder();
            code.Append($"Value={HttpUtility.UrlEncode(Value)}");
            return code.ToString();
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="message">返回的消息</param>
        /// <returns>成功则返回真</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
    /// <summary>
    /// 请求参数
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Argument<T> : IApiArgument
    {
        /// <summary>
        /// AT
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Value { get; set; }



        string IApiArgument.ToFormString()
        {
            StringBuilder code = new StringBuilder();
            code.Append($"Value={Value}");
            return code.ToString();
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="message">返回的消息</param>
        /// <returns>成功则返回真</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
}