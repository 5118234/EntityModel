// ���ڹ��̣�GBoxtCommonService
// �����û���bull2
// ����ʱ�䣺2012-08-13 5:34
// ����ʱ�䣺2012-08-30 2:58

#region

using System ;

#endregion

namespace Agebull.Common.Server.Logging
{
    /// <summary>
    ///   ������Ϣ
    /// </summary>
    [Serializable]
    public class LocaleItem
    {
        /// <summary>
        ///   ����
        /// </summary>
        public string Name { get ; set ; }

        /// <summary>
        ///   ˵��
        /// </summary>
        public string Description { get ; set ; }

        /// <summary>
        ///   ֵ
        /// </summary>
        public object Value { get ; set ; }
    }
}
