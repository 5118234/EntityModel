// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-24
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using Agebull.Common.DataModel;
using Agebull.Common.Logging;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.SystemModel;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     Ϊҵ���������Ķ���
    /// </summary>
    public class BusinessContext : IDisposable
    {
        #region ȫ����Ϣ

        private readonly StringBuilder _messageBuilder = new StringBuilder();

        private string _lastMessage;

        /// <summary>
        ///     ���һ���Ĳ�����Ϣ
        /// </summary>
        public string LastMessage
        {
            get => _lastMessage;
            set
            {
                if (string.Equals(_lastMessage, value, StringComparison.OrdinalIgnoreCase))
                    return;
                _lastMessage = value;
                if (_messageBuilder.Length > 0)
                    _messageBuilder.Append("��");
                _messageBuilder.Append(value);
            }
        }
        /// <summary>
        /// ȡ������Ϣ
        /// </summary>
        /// <returns></returns>
        public string GetFullMessage()
        {
            return _messageBuilder.ToString();
        }
        #endregion 

        #region ���������ֵ�

        /// <summary>
        ///     ���������ֵ�
        /// </summary>
        public DependencyObjects DependencyObjects { get; } = new DependencyObjects();

        #endregion

        #region Ȩ�޶���
        
        /// <summary>
        ///     �Ƿ�����ϵͳģʽ��
        /// </summary>
        public bool IsSystemMode { get;set; }

        /// <summary>
        ///     ����ע�������IPowerChecker����ķ���
        /// </summary>
        public static Func<IPowerChecker> CreatePowerChecker { private get; set; }

        /// <summary>
        ///     δ��֤�û�
        /// </summary>
        public static ILoginUser Anymouse
        {
            get;
            set;
        }

        /// <summary>
        ///     ϵͳ�û�
        /// </summary>
        public static ILoginUser SystemUser
        {
            get;
            set;
        }
        /// <summary>
        ///     ��ǰ��¼���û�
        /// </summary>
        public ILoginUser LoginUser
        {
            get { return IsSystemMode ? SystemUser  : _loginUser ?? Anymouse; }
            set { _loginUser = value; }
        }

        /// <summary>
        ///     ��ǰ��¼���û�ID
        /// </summary>
        public int LoginUserId => LoginUser?.Id ?? -1;

        /// <summary>
        ///     ��ǰҳ��ڵ�����
        /// </summary>
        public IPageItem PageItem { get; set; }

        /// <summary>
        ///     Ȩ��У�����
        /// </summary>
        private IPowerChecker _powerChecker;


        /// <summary>
        ///     Ȩ��У�����
        /// </summary>
        public IPowerChecker PowerChecker => _powerChecker ?? (_powerChecker = CreatePowerChecker());

        /// <summary>
        ///     �û��Ľ�ɫȨ��
        /// </summary>
        private List<IRolePower> _powers;

        /// <summary>
        ///     �û��Ľ�ɫȨ��
        /// </summary>
        public List<IRolePower> Powers => _powers ?? (_powers = PowerChecker.LoadUserPowers(LoginUser));

        /// <summary>
        /// ��ǰҳ��Ȩ������
        /// </summary>
        public IRolePower CurrentPagePower
        {
            get;
            set;
        }
        /// <summary>
        /// �ڵ�ǰҳ�����Ƿ����ִ�в���
        /// </summary>
        /// <param name="action">����</param>
        /// <returns></returns>
        public bool CanDoCurrentPageAction(string action)
        {
            return PowerChecker == null || PowerChecker.CanDoAction(LoginUser, PageItem, action);
        }

        /// <summary>
        /// �û�����
        /// </summary>
        public Guid Tooken { get; set; }

        /// <summary>
        /// �û������Ƿ񱣴���COOKIE��;
        /// </summary>
        public bool WorkByCookie { get; set; }

        #endregion

        #region �̵߳���

        /// <summary>
        ///     �̵߳�������
        /// </summary>
        [ThreadStatic] internal static BusinessContext _current;

        /// <summary>
        ///     ȡ�û������̵߳������󣬵�ǰ���󲻴���ʱ�����Զ�����һ��
        /// </summary>
        public static BusinessContext Current
        {
            get => _current ?? CreateContext();
            internal set => _current = value;
        }

        #endregion

        #region ����������

        /// <summary>
        ///     ����
        /// </summary>
        public BusinessContext()
        {
            _current = this;
            LogRecorder.MonitorTrace("BusinessContext.ctor");
        }

        /// <summary>
        ///     ȡ�õ�ǰ�������Ķ���
        /// </summary>
        /// <returns>�����ǰ�������Ķ���Ϊnull,��Ϊnull</returns>
        internal static BusinessContext GetCurrentContext()
        {
            return _current;
        }

        /// <summary>
        ///     ����һ�������Ķ��󷽷��ĺ���ע��
        /// </summary>
        /// <returns>�����Ķ���</returns>
        public static Func<BusinessContext> CreateFunc;

        /// <summary>
        ///     ����һ�������Ķ���
        /// </summary>
        /// <returns>�����Ķ���</returns>
        public static BusinessContext CreateContext()
        {
            return CreateFunc != null ? CreateFunc() : new BusinessContext();
        }

        /// <summary>
        ///     ����
        /// </summary>
        ~BusinessContext()
        {
            DoDispose();
        }

        /// <summary>
        ///     �Ƿ���ȷ�����ı��
        /// </summary>
        private bool _isDisposed;

        private ILoginUser _loginUser;

        /// <summary>
        ///     ִ�����ͷŻ����÷��й���Դ��ص�Ӧ�ó����������
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            DoDispose();
        }

        /// <summary>
        ///     ִ�����ͷŻ����÷��й���Դ��ص�Ӧ�ó����������
        /// </summary>
        /// <filterpriority>2</filterpriority>
        private void DoDispose()
        {
            if (_isDisposed)
            {
                return;
            }
            GC.ReRegisterForFinalize(this);
            TransactionScope.EndAll();
            LogRecorder.MonitorTrace("BusinessContext.DoDispose");
            _isDisposed = true;
            if (_current == this)
            {
                _current = null;
            }
        }

        #endregion
    }
}