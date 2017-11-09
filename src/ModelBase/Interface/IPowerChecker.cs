// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;

#endregion

namespace Gboxt.Common.SystemModel
{
    /// <summary>
    ///     Ȩ�޲�������ע�����
    /// </summary>
    public interface IPowerChecker
    {
        /// <summary>
        /// ����ҳ��Ķ���
        /// </summary>
        void SavePageAction(long pageid, string name, string title, string action,string type);

        /// <summary>
        /// ���������û���Ϣ
        /// </summary>
        void ReloadLoginUserInfo();

        /// <summary>
        /// ���������û���Ϣ
        /// </summary>
        void ReloadLoginUserInfo(Guid uid);

        /// <summary>
        ///     ����ҳ������
        /// </summary>
        /// <param name="url">���������������url</param>
        /// <returns>ҳ������</returns>
        IPageItem LoadPageConfig(string url);

        /// <summary>
        ///     ����ҳ������İ�ť����
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">ҳ��,����Ϊ��</param>
        /// <returns>��ť���ü���</returns>
        List<string> LoadPageButtons(ILoginUser loginUser, IPageItem page);

        /// <summary>
        ///     ����ҳ������İ�ť����
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">ҳ��</param>
        /// <param name="action">����</param>
        /// <returns>�Ƿ��ִ��ҳ�涯��</returns>
        bool CanDoAction(ILoginUser loginUser, IPageItem page, string action);

        /// <summary>
        ///     �����û��Ľ�ɫȨ��
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <returns></returns>
        List<IRolePower> LoadUserPowers(ILoginUser loginUser);

        /// <summary>
        ///     �����û��ĵ�ҳ��ɫȨ��
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">ҳ��</param>
        /// <returns></returns>
        IRolePower LoadPagePower(ILoginUser loginUser, IPageItem page);

        /// <summary>
        ///     �����û��Ĳ�ѯ��ʷ
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">����ҳ��</param>
        /// <param name="args">��ѯ����</param>
        void SaveQueryHistory(ILoginUser loginUser, IPageItem page, Dictionary<string, string> args);

        /// <summary>
        ///     ��ȡ�û��Ĳ�ѯ��ʷ
        /// </summary>
        /// <param name="loginUser">��¼�û�</param>
        /// <param name="page">����ҳ��</param>
        /// <returns>���ص��ǲ����ֵ��JSON��ʽ���ı�</returns>
        string LoadQueryHistory(ILoginUser loginUser, IPageItem page);
    }

}