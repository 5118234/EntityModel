using System;
using Gboxt.Common.DataModel;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    /// ��Ŀ��������������
    /// </summary>
    public class WorkflowControler
    {
        /// <summary>
        /// ���̼��
        /// </summary>
        public static Action<IAuditData> CheckWordflow;
        /// <summary>
        /// ����UI��Ϣ����
        /// </summary>
        public static Action<UserJobData> SetJobUiInfomation;
        /// <summary>
        /// ��������
        /// </summary>
        public static Func<int, int, IWorkflowData> LoadData;
    }
}

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ��ʾ������������
    /// </summary>
    public interface IWorkflowData : ITitle, IIdentityData, IStateData, IHistoryData, IAuditData
    {
    }
}