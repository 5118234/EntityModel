namespace GoodLin.Common.Api
{
    /// <summary>
    /// �û����У��
    /// </summary>
    public interface IBearValidater
    {
        /// <summary>
        /// �����õ�ServiceKey�������ڲ����ã�
        /// </summary>
        /// <param name="token">����</param>
        /// <returns></returns>
        IApiResult ValidateServiceKey(string token);
        /// <summary>
        /// ���AT(���Ե�¼�û�)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        IApiResult<IUserProfile> VerifyAccessToken(string token);
        /// <summary>
        /// ����豸��ʶ������δ��¼�û���
        /// </summary>
        /// <param name="token">����</param>
        /// <returns></returns>
        IApiResult<IUserProfile> ValidateDeviceId(string token);

        /// <summary>
        /// ����豸��ʶ������δ��¼�û���
        /// </summary>
        /// <param name="uid">�û�ID</param>
        /// <returns></returns>
        IApiResult<IUserProfile> GetUserProfile(long uid);
    }
}