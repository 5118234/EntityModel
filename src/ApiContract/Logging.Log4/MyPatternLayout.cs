// ���ڹ��̣�GBoxtCommonService
// �����û���bull2
// ����ʱ�䣺2012-08-13 5:34
// ����ʱ�䣺2012-08-30 2:58

#region

using log4net.Layout ;

#endregion

namespace Agebull.Common.Server.Logging
{
    /// <summary>
    ///   LOG4�ļ�¼����
    /// </summary>
    public class MyPatternLayout : PatternLayout
    {
        /// <summary>
        ///   ����
        /// </summary>
        public MyPatternLayout()
        {
            this.AddConverter("property" , typeof(MyMessagePatternConverter)) ;
        }
    }
}
