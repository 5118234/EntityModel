using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Agebull.Common.Logging
{
    /// <summary>
    /// �����Ϣ
    /// </summary>
    [Serializable]
    class MonitorData
    {
        /// <summary>
        /// 
        /// </summary>
        public FixStack<MonitorItem> Stack;

        /// <summary>
        /// 
        /// </summary>
        public StringBuilder Texter;

        /// <summary>
        /// �����
        /// </summary>
        public bool InMonitor;
    }

    /// <summary>
    /// �����Ϣ
    /// </summary>
    [Serializable]
    internal class MonitorItem
    {
        private static MonitorData Data
        {
            get
            {
                var data = ContextHelper.LogicalGetData<MonitorData>("MonitorData");
                if (data == null)
                    ContextHelper.LogicalSetData("MonitorData", data = new MonitorData());
                return data;
            }
        }

        /// <summary>
        /// �����
        /// </summary>
        internal static bool InMonitor
        {
            get { return Data.InMonitor; }
            set
            {
                if (value)
                {
                    if (Data.InMonitor)
                        return;
                    Data.InMonitor = true;
                }
                else
                {
                    ContextHelper.Remove("MonitorData");
                }
            }
        }


        private static FixStack<MonitorItem> localMonitorStack
        {
            get { return Data.Stack; }
            set { Data.Stack = value; }
        }

        private static StringBuilder localMonitorTexter
        {
            get { return Data.Texter; }
            set { Data.Texter = value; }
        }
        

        internal static FixStack<MonitorItem> MonitorStack
        {
            get
            {
                return localMonitorStack;
            }
            set
            {
                localMonitorStack = value;
            }
        }
        /// <summary>
        /// ���ӵ��ı�д����
        /// </summary>
        internal static StringBuilder MonitorTexter
        {
            get
            {
                return localMonitorTexter;
            }
            set
            {
                localMonitorTexter = value;
            }
        }

        /// <summary>
        /// �ı�
        /// </summary>
        public string Title, Space, message;
        /// <summary>
        /// ���Լ���
        /// </summary>
        public int NumberA, NumberB, NumberC;
        /// <summary>
        /// �ڴ����
        /// </summary>
        public long TotalAllocatedMemorySize, MonitoringTotalAllocatedMemorySize;
        /// <summary>
        /// �ڴ�ռ��
        /// </summary>
        public long TotalSurvivedMemorySize, MonitoringSurvivedMemorySize;
        /// <summary>
        /// ������ʱ��
        /// </summary>
        public TimeSpan MonitoringTotalProcessorTime;

        /// <summary>
        /// �ܴ�����ʱ��
        /// </summary>
        public double TotalProcessorTime, TotalTime;
        /// <summary>
        /// ��ֹʱ��
        /// </summary>
        public DateTime startTime, preTime;

        /// <summary>
        /// ����
        /// </summary>
        public MonitorItem(string title, string space = "")
        {
            Space = space;
            Title = title;

            NumberA = 0;
            NumberB = 0;
            NumberC = 0;
            TotalTime = 0F;
            TotalProcessorTime = 0F;
            TotalSurvivedMemorySize = 0;
            TotalAllocatedMemorySize = 0;


            message = string.Format("|��ʼ| {0:HH:mm:ss} |       -       |     -    |     -    |{1}|    -     |{2}|"//    -     |     -    |     -    |
                , DateTime.Now, (AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize / 1048576F).ToFixLenString(10, 3)
                , (AppDomain.CurrentDomain.MonitoringSurvivedMemorySize / 1048576F).ToFixLenString(10, 3));

            startTime = DateTime.Now;
            Flush();
        }
        /// <summary>
        /// ˢ����Ϣ
        /// </summary>
        /// <returns></returns>
        public void FlushMessage()
        {
            var a = DateTime.Now - preTime;
            var b = AppDomain.CurrentDomain.MonitoringTotalProcessorTime - MonitoringTotalProcessorTime;
            var c = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize - MonitoringTotalAllocatedMemorySize;
            var d = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            var e = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize - MonitoringSurvivedMemorySize;
            var f = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;

            message = string.Format("| �� |    -     |{0}|{1}|{2}|{3}|{4}|{5}|"//{6}|{7}|{8}|
                , a.TotalMilliseconds.ToFixLenString(15, 2)
                , b.TotalMilliseconds.ToFixLenString(10, 2)
                , (c / 1024F).ToFixLenString(10, 3)
                , (d / 1048576F).ToFixLenString(10, 3)
                , (e / 1024F).ToFixLenString(10, 3)
                , (f / 1048576F).ToFixLenString(10, 3)
                , NumberA.ToFixLenString(10)
                , NumberB.ToFixLenString(10)
                , NumberC.ToFixLenString(10));

        }
        /// <summary>
        /// �ռ���Ϣ
        /// </summary>
        /// <returns></returns>
        public void Coll()
        {
            TotalTime += (DateTime.Now - preTime).TotalMilliseconds;
            TotalProcessorTime += (AppDomain.CurrentDomain.MonitoringTotalProcessorTime - MonitoringTotalProcessorTime).TotalMilliseconds;
            TotalSurvivedMemorySize += AppDomain.CurrentDomain.MonitoringSurvivedMemorySize - MonitoringSurvivedMemorySize;
            TotalAllocatedMemorySize += AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize - MonitoringTotalAllocatedMemorySize;
        }
        /// <summary>
        /// �ռ���Ϣ
        /// </summary>
        /// <returns></returns>
        public void Coll(MonitorItem item)
        {
            TotalTime += item.TotalTime;
            TotalProcessorTime += item.TotalProcessorTime;
            TotalSurvivedMemorySize += item.TotalSurvivedMemorySize;
            TotalAllocatedMemorySize += item.TotalAllocatedMemorySize;
        }
        /// <summary>
        /// ˢ����Ϣ
        /// </summary>
        /// <returns></returns>
        public void Flush()
        {
            MonitoringTotalAllocatedMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;

            MonitoringSurvivedMemorySize = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;

            MonitoringTotalProcessorTime = AppDomain.CurrentDomain.MonitoringTotalProcessorTime;

            preTime = DateTime.Now;
        }
        /// <summary>
        /// ˢ����Դ���
        /// </summary>
        public void EndMessage()
        {
            var a = DateTime.Now - startTime;
            var d = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize;
            var f = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize;
            message = string.Format("|���| {0:HH:mm:ss} |{1}/{10}|{2}|{3}|{4}|{5}|{6}|"//{7}|{8}|{9}|
                , DateTime.Now, TotalTime.ToFixLenString(7, 1)
                , TotalProcessorTime.ToFixLenString(10, 2)
                , (TotalAllocatedMemorySize / 1024F).ToFixLenString(10, 3)
                , (d / 1048576F).ToFixLenString(10, 3)
                , (TotalSurvivedMemorySize / 1024F).ToFixLenString(10, 3)
                , (f / 1048576F).ToFixLenString(10, 3)
                , NumberA.ToFixLenString(10)
                , NumberB.ToFixLenString(10)
                , NumberC.ToFixLenString(10)
                , a.TotalMilliseconds.ToFixLenString(7, 1));
        }
    }
}