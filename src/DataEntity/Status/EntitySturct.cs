// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System.Collections.Generic;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ��ʾʵ��ṹ
    /// </summary>
    public sealed class EntitySturct
    {
        /// <summary>
        ///     ��������
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        ///     ʵ������
        /// </summary>
        public int EntityType { get; set; }

        /// <summary>
        ///     ��������
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        ///     ����
        /// </summary>
        public Dictionary<int, PropertySturct> Properties { get; set; }
    }
}