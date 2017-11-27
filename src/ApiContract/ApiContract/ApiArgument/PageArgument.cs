using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// ��ҳ����Ĳ���
    /// </summary>
    public class PageArgument : IApiArgument
    {
        /// <summary>
        /// ҳ��
        /// </summary>
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int Page { get; set; }

        /// <summary>
        /// ÿҳ����
        /// </summary>
        [JsonProperty("pageSize", NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>
        /// ÿҳ����
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public string Order { get; set; }


        /// <summary>
        /// ����
        /// </summary>
        [JsonProperty("order", NullValueHandling = NullValueHandling.Ignore)]
        public bool Desc { get; set; }

        
        string IApiArgument.ToFormString()
        {
            return $"Page={Page}&PageSize={PageSize}&Order={Order}&Desc={Desc}";
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