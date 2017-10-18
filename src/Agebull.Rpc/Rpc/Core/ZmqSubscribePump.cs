using System;
using System.Diagnostics;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;

namespace Agebull.Zmq.Rpc
{
    /// <summary>
    /// ���������
    /// </summary>
    public abstract class ZmqSubscribePump : ZmqCommandPump<SubscriberSocket>
    {
        #region ��Ϣ��
        /// <summary>
        /// ����SOCKET����
        /// </summary>
        /// <remarks>�汾�����ݵ��¸ı�</remarks>
        protected override SubscriberSocket CreateSocket()
        {
            return new SubscriberSocket();
        }

        /// <summary>
        /// ����Socket����
        /// </summary>
        /// <param name="socket"></param>
        protected override void OptionSocktet(SubscriberSocket socket)
        {
            //�û�ʶ��
            socket.Options.Identity = RpcEnvironment.Token;
            //�ر�����ͣ��ʱ��,����
            socket.Options.Linger = new TimeSpan(0, 0, 0, 0, 50);
        }
        
        #endregion

    }
}