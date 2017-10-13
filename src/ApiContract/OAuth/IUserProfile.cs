
namespace GoodLin.Common.Api
{
    /// <summary>
    /// �û���Ϣ
    /// </summary>
    public interface IUserProfile : IApiResultData
    {
        /// <summary>
        /// �û����ֱ�ʶ
        /// </summary>
        long UserId { get; set; }

        /// <summary>
        /// �û������豸��ʶ
        /// </summary>
        string DeviceId { get; set; }

        /// <summary>
        /// �û��ǳ�
        /// </summary>
        string NickName { get; set; }
        /// <summary>
        /// �û��ֻ���
        /// </summary>
        string PhoneNumber { get; set; }
        /// <summary>
        /// ����ϵͳʱ��
        /// </summary>
        long JoinTime { get; set; }
        /// <summary>
        /// ��ǰ������ʱ��
        /// </summary>
        long ApiServerTime { get; set; }
    }
}