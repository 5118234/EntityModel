using System;
using Gboxt.Common.DataModel;
using Newtonsoft.Json;
using System.Runtime.Serialization;

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
    [Serializable]
    [DataContract, JsonObject(MemberSerialization.OptIn)]
    public class LoginUserInfo : ILoginUserInfo
    {
        /// <summary>
        /// �û����ֱ�ʶ
        /// </summary>
        [JsonProperty]
        public long UserId { get; set; }

        /// <summary>
        /// �û��ǳ�
        /// </summary>
        [JsonProperty]
        public string NickName { get; set; }

        /// <summary>
        /// ͷ��
        /// </summary>
        /// <remarks>
        /// ͷ��
        /// </remarks>
        [JsonProperty]
        public string AvatarUrl
        {
            get;
            set;
        }
        /// <summary>
        /// ��ǰ�û���¼���ĸ�ϵͳ��Ԥ�ȶ����ϵͳ��ʶ��
        /// </summary>
        [JsonProperty]
        public string LoginSystem { get; set; }

        /// <summary>
        /// ��ǰ�û���¼��ʽ
        /// </summary>
        [JsonProperty]
        public int LoginType { get; set; }

        /// <summary>
        /// ��¼�ߵ��ֻ���
        /// </summary>
        [JsonProperty]
        public string Phone { get; set; }


        /// <summary>
        /// ��¼�ߵ��˺�
        /// </summary>
        [JsonProperty]
        public string Account { get; set; }

        /// <summary>
        /// ��¼�豸�ı�ʶ
        /// </summary>
        [JsonProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// ��¼�豸�Ĳ���ϵͳ
        /// </summary>
        [JsonProperty]
        public string Os { get; set; }

        /// <summary>
        /// ��¼�豸�������
        /// </summary>
        [JsonProperty]
        public string Browser { get; set; }

        /// <summary>
        ///     ����״̬
        /// </summary>
        [JsonProperty]
        public DataStateType DataState { get; set; }

        /// <summary>
        ///     �����Ƿ��Ѷ��ᣬ����ǣ���Ϊֻ������
        /// </summary>
        /// <value>bool</value>
        [JsonProperty]
        public bool IsFreeze { get; set; }
    }
}