namespace Yizuan.Service.Api
{
    /// <summary>
    /// API״̬���أ�һ���ڳ���ʱ������
    /// </summary>
    public interface IApiStatusResult
    {
        /// <summary>
        /// �����루ϵͳ���壩
        /// </summary>
        int ErrorCode { get; set; }

        /// <summary>
        /// ��ӦHTTP�����루�ο���
        /// </summary>
        string HttpCode { get; set; }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// �ڲ���ʾ��Ϣ
        /// </summary>
        string InnerMessage { get; set; }
    }
}