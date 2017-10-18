using Newtonsoft.Json;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    /// ����ı�׼����
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// ״̬��0��ʾ�ɹ���������ʾ�������
        /// </summary>
        int Status { get; }

        /// <summary>
        /// ������Ϣ������еĻ�
        /// </summary>
        string Message { get; }
    }

    /// <summary>
    /// ���������
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CommandResult : ICommandResult
    {
        /// <summary>
        /// �ɹ���ʶ
        /// </summary>
        public bool Success
        {
            get { return Status == 0; }
            set { Status = Success ? 0 : -1; }
        }
        /// <summary>
        /// ״̬��0��ʾ�ɹ���������ʾ�������,-1��ʾδ֪����
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message", NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}