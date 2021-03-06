using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// 请求参数
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Argument : IApiArgument
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

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
        /// 数值
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Value { get; set; }

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