using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    ///  ��Ϣ���������
    /// </summary>
    public class ZmqNotifyPump : ZmqSubscribePump
    {
        #region ��Ϣ��

        /// <summary>
        /// ����������
        /// </summary>
        protected override void DoRun()
        {
            Task.Factory.StartNew(MessageNotifyTask);
            base.DoRun();
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        protected sealed override void DoInitialize()
        {
        }

        /// <summary>
        /// ����Socket����
        /// </summary>
        /// <param name="socket"></param>
        protected sealed override void OptionSocktet(SubscriberSocket socket)
        {
            //�û�ʶ��
            socket.Options.Identity = RpcEnvironment.Token;
            //�ر�����ͣ��ʱ��,����
            socket.Options.Linger = new TimeSpan(0, 0, 0, 0, 50);
        }
        /// <summary>
        /// ���Ӵ���
        /// </summary>
        /// <param name="socket"></param>
        protected sealed override void OnConnected(SubscriberSocket socket)
        {
            socket.Subscribe(EventWorkerContainer.SubName ?? "");
        }

        /// <summary>
        /// ִ�й���
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>����״̬,����-1�ᵼ������</returns>
        protected sealed override int DoWork(SubscriberSocket socket)
        {
            byte[] buffer;
            if (!socket.TryReceiveFrameBytes(timeOut, out buffer))
            {
                Thread.Sleep(10);
                return 0;
            }
            try
            {
                var reader = new CommandReader(buffer);
                reader.ReadCommandFromBuffer();
                Console.WriteLine("�յ���Ϣ" + reader.Command.Data.GetType());
                Push(reader.Command);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, GetType().Name + "DoWork");
                return -1;
            }
            return 0;
        }
        #endregion

        #region �¼�����

        /// <summary>
        ///     ��Ϣ֪ͨ����Task
        /// </summary>
        private void MessageNotifyTask()
        {
            if (EventWorkerContainer.CreateWorker == null)
                return;
            //log_msg("֪ͨ��Ϣ��������");
            while (RpcEnvironment.NetState != ZmqNetStatus.Destroy)
            {
                CommandArgument cmdMsg = Pop();
                if (cmdMsg == null)
                    continue;
                Task.Factory.StartNew(ProcessMessage, cmdMsg);
            }
            //log_msg("֪ͨ��Ϣ���ѹر�");
        }
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        /// <param name="state"></param>
        private void ProcessMessage(object state)
        {
            CommandArgument cmd = (CommandArgument)state;
            var data = (StringArgumentData)cmd.Data;
            IEventWorker worker = EventWorkerContainer.CreateWorker();
            var arg = worker.ToArgument(cmd.command, data.Argument);
            using (RpcContextScope.CreateScope(cmd, arg))
            {
                try
                {
                    worker.Process(cmd.command, arg);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    if (cmd.cmdId == RpcEnvironment.NET_COMMAND_CALL)
                        RpcProxy.Result(new CommandResult
                        {
                            Status = RpcEnvironment.NET_COMMAND_STATE_SERVER_UNKNOW,
                            Message = e.Message
                        });
                }
            }
        }

        #endregion
    }

}