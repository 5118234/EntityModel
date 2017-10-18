using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    ///  �¼����������
    /// </summary>
    public class ZmqAnswerPump : ZmqSubscribePump
    {
        #region ��Ϣ��
        
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
            socket.Subscribe(RpcCore.Singleton.Config.Token);
        }

        /// <summary>
        /// ִ�й���
        /// </summary>
        /// <param name="socket"></param>
        /// <returns>����״̬,����-1�ᵼ������</returns>
        protected sealed override int DoWork(SubscriberSocket socket)
        {
            byte[] buffer;
            Debug.WriteLine(this.ZmqAddress);
            if (!socket.TryReceiveFrameBytes(out buffer))
            {
                Thread.Sleep(0);
                return 0;
            }
            try
            {
                var reader = new CommandReader(buffer);
                reader.ReadCommandFromBuffer();
                var cmdMsg= reader.Command;
                try
                {
                    var old = RpcEnvironment.SyncCacheCommand(cmdMsg);
                    old?.OnRequestStateChanged(cmdMsg);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex, GetType().Name + "MessageNotifyTask");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, GetType().Name + "DoWork");
                return -1;
            }
            return 0;
        }
        #endregion
        
    }

}