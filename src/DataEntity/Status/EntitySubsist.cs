// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ������̬״̬
    /// </summary>
    public enum EntitySubsist
    {
        /// <summary>
        ///     δ֪,ֻ������ʶ��Ϊ����
        /// </summary>
        None,

        /// <summary>
        ///     ����δ����
        /// </summary>
        Adding,

        /// <summary>
        ///     �����ѱ���,�൱��Exist,�������ڴ�����������ĺ����¼�
        /// </summary>
        Added,

        /// <summary>
        ///     Ӱ��(����������ĸ���Ʒ��δ�޸�,�ɲ���������ø�������Ϣ)
        /// </summary>
        Shadow,

        /// <summary>
        ///     ��Ҫɾ��
        /// </summary>
        Deleting,

        /// <summary>
        ///     �Ѿ�ɾ��
        /// </summary>
        Deleted,

        /// <summary>
        ///     ��Ҫ�޸�
        /// </summary>
        Modify,

        /// <summary>
        ///     �Ѿ��޸�
        /// </summary>
        Modified,

        /// <summary>
        ///     �Ѵ���
        /// </summary>
        Exist = None
    }

    /// <summary>
    ///     ���ݲ���״̬
    /// </summary>
    public enum DataOperatorType
    {
        /// <summary>
        ///     δ֪
        /// </summary>
        None,

        /// <summary>
        ///     ����
        /// </summary>
        Insert,

        /// <summary>
        ///     ����
        /// </summary>
        Update,
        
        /// <summary>
        ///     ɾ��
        /// </summary>
        Delete
    }
}