using System;
using System.Collections.Generic;
using System.Threading;
using NetMQ;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    ///     ZMQ����ִ�б�
    /// </summary>
    public abstract class ZmqCommandPump<TSocket> : IDisposable
        where TSocket : NetMQSocket
    {
        #region ��������
        /// <summary>
        /// ������
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public void Run()
        {
            m_state = 3;
            DoRun();
            m_state = 4;
        }
        /// <summary>
        /// �߳�
        /// </summary>
        internal Thread thread;
        /// <summary>
        /// ����������
        /// </summary>
        protected virtual void DoRun()
        {
            thread = new Thread(RunPump)
            {
                Priority = ThreadPriority.BelowNormal,
                IsBackground = true
            };
            thread.Start();
        }

        /// <summary>
        /// ��������
        /// </summary>
        private int m_restart;

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        public int RestartCount {get { return m_restart; } }
        /// <summary>
        /// ִ��״̬
        /// </summary>
        private int m_state;

        /// <summary>
        /// �Ƿ��ѳ�ʼ��
        /// </summary>
        public bool IsInitialized { get { return m_state > 0; } }
        

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        public bool IsDisposed { get { return m_state > 4; } }

        /// <summary>
        /// ״̬ 0 ��ʼ״̬,1 ���ڳ�ʼ��,2 ��ɳ�ʼ��,3 ��ʼ����,4 ������,5 �������� 6 �������
        /// </summary>
        public int State {get { return m_state; } }

        /// <summary>
        ///     ��ʼ���ͻ���
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized)
                return;
            m_state = 1;
            m_queue = new Queue<CommandArgument>();
            m_mutex = new Mutex();
            DoInitialize();
            m_state = 2;
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected abstract void DoInitialize();

        /// <summary>
        ///     ����
        /// </summary>
        public void Dispose()
        {
            m_state = 5;
            if (m_mutex != null)
            {
                m_mutex.Dispose();
                m_mutex = null;
            }
            DoDispose();
            m_state = 6;
        }

        /// <summary>
        /// ����
        /// </summary>
        protected virtual void DoDispose()
        {

        }

        #endregion

        #region ��Ϣ����

        /// <summary>
        ///     C�˵�������ö���
        /// </summary>
        private Queue<CommandArgument> m_queue;

        /// <summary>
        ///     C��������ö�����
        /// </summary>
        private Mutex m_mutex;

        /// <summary>
        ///     ȡ�ö�������
        /// </summary>
        protected CommandArgument Pop()
        {
            if (!m_mutex.WaitOne(100))
            {
                return null;
            }
            if (m_queue.Count == 0)
            {
                m_mutex.ReleaseMutex();
                Thread.Sleep(1);
                return null;
            }
            var mdMsg = m_queue.Dequeue();
            m_mutex.ReleaseMutex();
            return mdMsg;
        }

        /// <summary>
        ///     ����д�����
        /// </summary>
        /// <param name="cmdMsg"></param>
        protected void Push(CommandArgument cmdMsg)
        {
            m_mutex.WaitOne();
            m_queue.Enqueue(cmdMsg);
            m_mutex.ReleaseMutex();
        }

        #endregion

        #region ��������

        /// <summary>
        /// Ĭ�ϳ�ʱ����
        /// </summary>
        protected readonly TimeSpan timeOut = new TimeSpan(0, 0, 0, 1);

        /// <summary>
        /// ZMQ�����ַ
        /// </summary>
        public string ZmqAddress { get; set; }

        /// <summary>
        /// ����Socket����
        /// </summary>
        /// <param name="socket"></param>
        protected abstract void OptionSocktet(TSocket socket);

        /// <summary>
        /// ���Ӵ���
        /// </summary>
        /// <param name="socket"></param>
        protected virtual void OnConnected(TSocket socket)
        {

        }

        //private NetMQMonitor monitor;
        /// <summary>
        /// ִ�й���
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>����״̬,����-1�ᵼ������</returns>
        protected abstract int DoWork(TSocket socket);
        
        /// <summary>
        /// ����SOCKET����
        /// </summary>
        /// <remarks>�汾�����ݵ��¸ı�</remarks>
        protected abstract TSocket CreateSocket();

        /// <summary>
        ///     ִ�д����
        /// </summary>
        /// <returns></returns>
        protected void RunPump()
        {
            //�Ǽ��߳̿�ʼ
            RpcEnvironment.set_command_thread_start();
            //monitor?.Stop();
            int state = 0;
            Console.WriteLine($"���������({ZmqAddress})��������");
            using (var socket = CreateSocket())
            {
                //monitor = new NetMQMonitor(socket, $"inproc://pump_{Guid.NewGuid()}.rep", SocketEvents.All);
                OptionSocktet(socket);
                socket.Connect(ZmqAddress);
                OnConnected(socket);

                Console.WriteLine($"���������({ZmqAddress})������");

                while (RpcEnvironment.NetState == ZmqNetStatus.Runing)
                {
                    state = DoWork(socket);
                    if (state == -1)
                        break;
                }
                socket.Disconnect(ZmqAddress);
            }
            //�Ǽ��̹߳ر�
            RpcEnvironment.set_command_thread_end();

            OnTaskEnd(state);
        }

        /// <summary>
        /// ���������ʱ����(����ʵ��Ϊ�쳣ʱ����ִ��)
        /// </summary>
        /// <param name="state">״̬</param>
        protected virtual void OnTaskEnd(int state)
        {
            if (state == -1 && RpcEnvironment.NetState == ZmqNetStatus.Runing)
            {
                Thread.Sleep(10);
                m_restart++;
                DoRun();
            }
        }
        #endregion
    }
}