namespace Gboxt.Common.DataModel
{
    /// <summary>
    /// ȫ�ֶ������
    /// </summary>
    public abstract class GlobalBase
    {
        /// <summary>
        /// ����
        /// </summary>
        public static GlobalBase Signle { get; set; }

        /// <summary>
        /// ����������
        /// </summary>
        public abstract void CreateContext();

        /// <summary>
        /// ��������ע��
        /// </summary>
        public abstract void DependantRegist();

        /// <summary>
        /// ��ʼ
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// ���»���
        /// </summary>
        public abstract void FlushCache();

        /// <summary>
        /// ����
        /// </summary>
        public abstract void Dispose();
    }
}