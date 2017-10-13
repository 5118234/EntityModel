using System;
using System.Runtime.Serialization;
using Gboxt.Common.DataModel;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    /// ��ʾΪ������������
    /// </summary>
    public interface ILevelAuditData : IAuditData
    {
        /// <summary>
        /// ��ǰ��������
        /// </summary>
        int DepartmentLevel { get; set; }

        /// <summary>
        /// ���󼶱�
        /// </summary>
        int LastLevel { get; }
    }

    /// <summary>
    ///     ��ʾ��������֧�����
    /// </summary>
    [DataContract, Serializable]
    public abstract class AutoJobEntity : EditDataObject
    {
        /// <summary>
        ///     �����
        /// </summary>
        /// <value>string</value>
        [DataMember]
        public string ToUsers { get; set; }

        /// <summary>
        ///     �����
        /// </summary>
        /// <value>string</value>
        [DataMember]
        public bool CanAudit { get; set; }
    }
}