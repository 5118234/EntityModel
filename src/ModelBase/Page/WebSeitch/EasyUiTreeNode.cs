// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Agebull.ProjectDeveloper.WebDomain.Models
{
    /// <summary>
    ///     EasyUi�����ݵĸ�ʽ
    /// </summary>
    /// <remarks>������ToJsonʱ���ϸ�ʽ</remarks>
    [DataContract]
    public class EasyUiTreeNodeBase
    {
        #region ״̬���

        /// <summary>
        ///     �ڵ�� id�������ڼ���Զ�����ݺ���Ҫ��
        /// </summary>
        [DataMember, JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        ///     �Ƿ���ѡ��
        /// </summary>
        [DataMember, JsonProperty("checked")]
        public bool IsChecked { get; set; }

        /// <summary>
        ///     �Ƿ���ѡ��
        /// </summary>
        [DataMember, JsonProperty("selected")]
        public bool IsSelect { get; set; }

        /// <summary>
        ///     ������ر�״̬
        /// </summary>
        [IgnoreDataMember, JsonProperty("state")]
        public virtual string TreeState => IsOpen != null && IsOpen.Value ? "open" : "closed";

        /// <summary>
        ///     ͼ��
        /// </summary>
        [DataMember, JsonProperty("iconCls")]
        public string Icon { get; set; }

        /// <summary>
        ///     �Ƿ�չ��
        /// </summary>
        [DataMember, JsonProperty("IsOpen")]
        public bool? IsOpen { get; set; }

        #endregion

        #region �������

        /// <summary>
        ///     ��ʾ�Ľڵ����֡�
        /// </summary>
        [DataMember, JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        ///     �����ԭʼ����
        /// </summary>
        [DataMember, JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///     ��һ���ڵ�׷�ӵ��Զ������ԡ�
        /// </summary>
        [DataMember, JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public string Attributes { get; set; }

        /// <summary>
        ///     ��ǩ�ı�
        /// </summary>
        [DataMember, JsonProperty("tag", NullValueHandling = NullValueHandling.Ignore)]
        public string Tag { get; set; }

        /// <summary>
        ///     ��ע�ı�
        /// </summary>
        [DataMember, JsonProperty("memo", NullValueHandling = NullValueHandling.Ignore)]
        public string Memo { get; set; }

        /// <summary>
        ///     ����������
        /// </summary>
        [DataMember, JsonProperty("json", NullValueHandling = NullValueHandling.Ignore)]
        public string Json { get; set; }

        /// <summary>
        ///     �����ı�
        /// </summary>
        [DataMember, JsonProperty("extend", NullValueHandling = NullValueHandling.Ignore)]
        public string Extend { get; set; }

        #endregion

        public static EasyUiTreeNodeBase Empty = new EasyUiTreeNodeBase { ID = 0, Text = "-", Title = "-" };
    }

    /// <summary>
    ///     EasyUi�����ݵĸ�ʽ
    /// </summary>
    /// <remarks>������ToJsonʱ���ϸ�ʽ</remarks>
    [DataContract]
    public class EasyUiTreeNode : EasyUiTreeNodeBase
    {
        /// <summary>
        ///     ��������¼�
        /// </summary>
        [IgnoreDataMember, JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<EasyUiTreeNode> Lists
        {
            get
            {
                return Children == null || Children.Count == 0
                    ? IsOpen == true
                        ? new List<EasyUiTreeNode>()
                        : null
                    : Children;
            }
        }

        /// <summary>
        ///     ��������¼�
        /// </summary>
        [DataMember, JsonIgnore]
        public List<EasyUiTreeNode> Children { get; set; }

        /// <summary>
        ///     ������ر�״̬
        /// </summary>
        [IgnoreDataMember, JsonProperty("state")]
        public override string TreeState
        {
            get
            {
                return IsOpen == null
                    ? (Children != null && Children.Count > 0 ? "open" : "closed")
                    : IsOpen.Value ? "open" : "closed";
            }
        }
        public static EasyUiTreeNode EmptyNode = new EasyUiTreeNode { ID = 0, Text = "-", Title = "-", IsOpen = true };
    }

    /// <summary>
    ///     EasyUi�����ݵĸ�ʽ
    /// </summary>
    /// <remarks>������ToJsonʱ���ϸ�ʽ</remarks>
    public class EasyUiTreeNode<TData> : EasyUiTreeNodeBase
    {
        /// <summary>
        ///     ����
        /// </summary>
        [DataMember, JsonProperty("data")]
        public TData Data { get; set; }

        /// <summary>
        ///     ��������¼�
        /// </summary>
        [IgnoreDataMember, JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<EasyUiTreeNode<TData>> Lists
        {
            get { return Children == null || Children.Count == 0 ? null : Children; }
        }

        /// <summary>
        ///     ��������¼�
        /// </summary>
        [DataMember, JsonIgnore]
        public List<EasyUiTreeNode<TData>> Children { get; set; }

        /// <summary>
        ///     ������ر�״̬
        /// </summary>
        [IgnoreDataMember, JsonProperty("state")]
        public override string TreeState
        {
            get
            {
                return IsOpen == null
                    ? (Children != null && Children.Count > 0 ? "open" : "closed")
                    : IsOpen.Value ? "open" : "closed";
            }
        }
    }
}