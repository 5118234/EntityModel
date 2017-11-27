using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yizuan.Service.Api
{
    /// <summary>
    /// ��ʾAPI���ص��б�
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class ApiList<TData> : List<TData>, IApiResultData
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="list"></param>
        public ApiList(IList<TData> list)
        {
            if (list == null)
                return;
            AddRange(list);
        }
    }

    /// <summary>
    /// API�������鷺����
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ApiArrayResult<TData> : IApiResult<ApiList<TData>>
    {
        /// <summary>
        /// �ɹ���ʧ�ܱ��
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Result { get; set; }

        /// <summary>
        /// APIִ��״̬��Ϊ�ձ�ʾ״̬������
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IApiStatusResult Status { get; set; }

        private ApiList<TData> _datas;

        /// <summary>
        /// ����ֵ
        /// </summary>
        [JsonIgnore]
        ApiList<TData> IApiResult<ApiList<TData>>.ResultData
        {
            get
            {
                if (_datas != null)
                    return _datas;
                _datas = ResultData as ApiList<TData>;
                if (_datas != null)
                    return _datas;
                return _datas = new ApiList<TData>(ResultData);
            }
            set { ResultData = value; }
        }

        /// <summary>
        /// ����ֵ
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TData> ResultData { get; set; }

    }
}