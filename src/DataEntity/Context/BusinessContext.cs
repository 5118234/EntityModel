// /*****************************************************
// (c)2016-2018 Copy right www.gboxt.com
// ����:Agebull
// ����:Agebull.DataModel
// ����:2018.01.16
// ˵����ȫ��ҵ�������Ҫ�ṩ����ע����ص�
// *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using Agebull.Common.DataModel;
using Agebull.Common.Logging;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     Ϊҵ����ȫ�ֶ���
    /// </summary>
    public static class BusinessGlobal
    {
        /// <summary>
        /// ����ע���ʵ���¼��������
        /// </summary>
        public static IEntityEventProxy EntityEventProxy { get; set; }
    }
}