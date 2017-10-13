// ���ڹ��̣�GBoxtCommonService
// �����û���bull2
// ����ʱ�䣺2012-08-13 5:34
// ����ʱ�䣺2012-08-30 2:58

#region

using System.IO ;
using System.Reflection ;

using log4net.Core ;
using log4net.Layout.Pattern ;

#endregion

namespace Agebull.Common.Server.Logging
{
    /// <summary>
    ///   ��Ϣת����
    /// </summary>
    public class MyMessagePatternConverter : PatternLayoutConverter
    {
        /// <summary>
        ///   ת��
        /// </summary>
        /// <param name="writer"> </param>
        /// <param name="loggingEvent"> </param>
        protected override void Convert(TextWriter writer , LoggingEvent loggingEvent)
        {
            if(this.Option != null)
            {
                WriteObject(writer , loggingEvent.Repository , LookupProperty(this.Option , loggingEvent)) ;
            }
            else
            {
                WriteDictionary(writer , loggingEvent.Repository , loggingEvent.GetProperties()) ;
            }
        }

        /// <summary>
        ///   ͨ�������ȡ�������־�����ĳ�����Ե�ֵ
        /// </summary>
        /// <param name="property"> </param>
        /// <param name="loggingEvent"> </param>
        /// <returns> </returns>
        private static object LookupProperty(string property , LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty ;
            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property) ;
            if(propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject , null) ;
            }
            return propertyValue ;
        }
    }
}
