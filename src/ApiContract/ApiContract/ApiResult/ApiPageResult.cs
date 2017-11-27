using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yizuan.Service.Api
{

    /// <summary>
    /// API�������鷺����
    /// </summary>
    public class ApiPageResult<TData> : ApiResult<ApiPageData<TData>>
    {
    }

    /// <summary>
    /// API���ط�ҳ��Ϣ
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPage : IApiResultData
    {
        /// <summary>
        /// ��ǰҳ�ţ���1��ʼ��
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PageIndex { get; set; }

        /// <summary>
        /// ҳ����
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PageSize { get; set; }

        /// <summary>
        /// ��ҳ��
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PageCount { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int RowCount { get; set; }
    }

    /// <summary>
    /// API���طֲ�ҳ����
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiPageData<TData> : ApiPage
    {
        /// <summary>
        /// ����ֵ
        /// </summary>
        [JsonProperty("Rows", NullValueHandling = NullValueHandling.Ignore)]
        public List<TData> Rows { get; set; }
    }
    /// <summary>
    /// API�������鷺����
    /// </summary>
    public class ApiPageResult : ApiResult<ApiPage>
    {
    }
}