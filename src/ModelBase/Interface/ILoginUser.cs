// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

namespace Gboxt.Common.SystemModel
{
    /// <summary>
    ///     ��ʾ��¼�û�����
    /// </summary>
    public interface ILoginUser
    {
        /// <summary>
        ///     �û���ʶ
        /// </summary>
        /// <remarks>
        ///     �û���ʶ
        /// </remarks>
        int Id { get;  }

        /// <summary>
        ///     �û���
        /// </summary>
        /// <remarks>
        ///     �û���
        /// </remarks>
        string UserName { get;  }

        /// <summary>
        ///     ��ɫID
        /// </summary>
        /// <remarks>
        ///     ��ɫID
        /// </remarks>
        int RoleId { get; }

        /// <summary>
        ///     ��������
        /// </summary>
        int DepartmentLevel { get; }

        /// <summary>
        ///     ����ID
        /// </summary>
        /// <remarks>
        ///     ����ID
        /// </remarks>
        int DepartmentId { get; }

        /// <summary>
        ///     ����
        /// </summary>
        /// <remarks>
        ///     ����
        /// </remarks>
        string RealName { get; }

    }
}