using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Gboxt.Common.SystemModel
{
    /// <summary>
    /// ҳ������
    /// </summary>
    [DataContract]
    [JsonObject(MemberSerialization.OptIn)]
    public class PageConfig
    {
        /// <summary>
        /// ϵͳ�ڲ�������
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string SystemType;
        /// <summary>
        /// �Ƿ���ʾ
        /// </summary>
        [JsonProperty("hide", NullValueHandling = NullValueHandling.Ignore)]
        public bool Hide;
        /// <summary>
        /// �Ƿ���������
        /// </summary>
        [JsonProperty("audit", NullValueHandling = NullValueHandling.Ignore)]
        public bool Audit;

        /// <summary>
        /// �ܷ�������
        /// </summary>
        [JsonProperty("level_audit", NullValueHandling = NullValueHandling.Ignore)]
        public bool LevelAudit;

        /// <summary>
        /// ����ҳ��
        /// </summary>
        [JsonProperty("audit_page", NullValueHandling = NullValueHandling.Ignore)]
        public int AuditPage;

        /// <summary>
        /// ��ҳ��
        /// </summary>
        [JsonProperty("master_page", NullValueHandling = NullValueHandling.Ignore)]
        public int MasterPage;

        /// <summary>
        /// �Ƿ����ݹ���
        /// </summary>
        [JsonProperty("data_state", NullValueHandling = NullValueHandling.Ignore)]
        public bool DataState;

        /// <summary>
        /// ��׼�༭
        /// </summary>
        [JsonProperty("edit", NullValueHandling = NullValueHandling.Ignore)]
        public bool Edit;

    }
}