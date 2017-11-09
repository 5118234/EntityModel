using Gboxt.Common.DataModel;

namespace Yizuan.Service.Api.OAuth
{
    /// <summary>
    /// ��ǰ��¼���û���Ϣ
    /// </summary>
    public interface IUser : IStateData
    {
        /// <summary>
        /// �û����ֱ�ʶ
        /// </summary>
        long UserId { get; set; }
    }

    /// <summary>
    /// ��ǰ��¼���û���Ϣ
    /// </summary>
    public interface IPerson : IUser
    {
        /// <summary>
        /// �û��ǳ�
        /// </summary>
        string NickName { get; set; }

        /// <summary>
        /// ͷ��
        /// </summary>
        /// <remarks>
        /// ͷ��
        /// </remarks>
        string AvatarUrl
        {
            get;
            set;
        }

    }

    /// <summary>
    /// ��ǰ��¼���û���Ϣ
    /// </summary>
    public interface ILoginUserInfo : IPerson, IApiResultData
    {
        /// <summary>
        /// ��ǰ�û���¼���ĸ�ϵͳ��Ԥ�ȶ����ϵͳ��ʶ��
        /// </summary>
        string LoginSystem { get; set; }

        /// <summary>
        /// ��ǰ�û���¼��ʽ
        /// </summary>
        int LoginType { get; set; }

        /// <summary>
        /// ��¼�ߵ��ֻ���
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// ��¼�ߵ��˺�
        /// </summary>
        string Account { get; set; }

        /// <summary>
        /// ��¼�豸�ı�ʶ
        /// </summary>
        string DeviceId { get; set; }

        /// <summary>
        /// ��¼�豸�Ĳ���ϵͳ
        /// </summary>
        string Os { get; set; }

        /// <summary>
        /// ��¼�豸�������
        /// </summary>
        string Browser { get; set; }
    }

    /// <summary>
    /// ��ǰ��¼���û���Ϣ
    /// </summary>
    public class LoginUserInfo : ILoginUserInfo
    {
        /// <summary>
        /// �û����ֱ�ʶ
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// �û��ǳ�
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// ͷ��
        /// </summary>
        /// <remarks>
        /// ͷ��
        /// </remarks>
        public string AvatarUrl
        {
            get;
            set;
        }
        /// <summary>
        /// ��ǰ�û���¼���ĸ�ϵͳ��Ԥ�ȶ����ϵͳ��ʶ��
        /// </summary>
        public string LoginSystem { get; set; }

        /// <summary>
        /// ��ǰ�û���¼��ʽ
        /// </summary>
        public int LoginType { get; set; }

        /// <summary>
        /// ��¼�ߵ��ֻ���
        /// </summary>
        public string Phone { get; set; }


        /// <summary>
        /// ��¼�ߵ��˺�
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// ��¼�豸�ı�ʶ
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// ��¼�豸�Ĳ���ϵͳ
        /// </summary>
        public string Os { get; set; }

        /// <summary>
        /// ��¼�豸�������
        /// </summary>
        public string Browser { get; set; }

        /// <summary>
        ///     ����״̬
        /// </summary>
        public DataStateType DataState { get; set; }

        /// <summary>
        ///     �����Ƿ��Ѷ��ᣬ����ǣ���Ϊֻ������
        /// </summary>
        /// <value>bool</value>
        public bool IsFreeze { get; set; }
    }
}