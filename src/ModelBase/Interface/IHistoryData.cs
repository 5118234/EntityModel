// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ��ʾ�������ݼ�¼�޸���ʷ
    /// </summary>
    public interface IHistoryData
    {
        /// <summary>
        ///     ����
        /// </summary>
        /// <value>string</value>
        int AuthorID { get; set; }

        /// <summary>
        ///     ��������
        /// </summary>
        /// <value>DateTime</value>
        DateTime AddDate { get; set; }

        /// <summary>
        ///     ����޸���
        /// </summary>
        /// <value>string</value>
        int LastReviserID { get; set; }

        /// <summary>
        ///     ����޸�����
        /// </summary>
        /// <value>DateTime</value>
        DateTime LastModifyDate { get; set; }
    }
}