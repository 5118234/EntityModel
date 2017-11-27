using System.Text;
using Newtonsoft.Json;
using System.Web;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// �������
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
        /// ����У��
        /// </summary>
        /// <param name="message">���ص���Ϣ</param>
        /// <returns>�ɹ��򷵻���</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
    /// <summary>
    /// �������
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
        /// ����У��
        /// </summary>
        /// <param name="message">���ص���Ϣ</param>
        /// <returns>�ɹ��򷵻���</returns>
        public bool Validate(out string message)
        {
            message = null;
            return true;
        }
    }
}