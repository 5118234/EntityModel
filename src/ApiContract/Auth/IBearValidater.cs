namespace Yizuan.Service.Api.OAuth
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
        ApiResult ValidateServiceKey(string token);

        /// <summary>
        /// ���AT(���Ե�¼�û�)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ApiResult<LoginUserInfo> VerifyAccessToken(string token);

        /// <summary>
        /// ����豸��ʶ������δ��¼�û���
        /// </summary>
        /// <param name="token">����</param>
        /// <returns></returns>
        ApiResult<LoginUserInfo> ValidateDeviceId(string token);

        /// <summary>
        /// ����豸��ʶ������δ��¼�û���
        /// </summary>
        /// <param name="at">��¼�û���AT</param>
        /// <returns></returns>
        ApiResult<LoginUserInfo> GetLoginUser(string at);
    }
}