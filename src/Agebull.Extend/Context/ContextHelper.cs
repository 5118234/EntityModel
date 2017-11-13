using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace Agebull.Common
{
    /// <summary>
    /// �����ĸ�����
    /// </summary>
    public static class ContextHelper
    {
        /// <summary>
        /// ��Ԫ����ʱʹ�õ��ֵ�
        /// </summary>
        private static readonly Dictionary<string, object> Contexts = new Dictionary<string, object>();

        /// <summary>
        /// �õ������߼������Ķ���
        /// </summary>
        public static T LogicalGetData<T>(string name)
            where T : class
        {
            if (!InUnitTest)
                return CallContext.LogicalGetData(name) as T;
            object value;
            if (!Contexts.TryGetValue(name, out value))
                return null;
            return value as T;
        }

        /// <summary>
        /// ���õ����߼������Ķ���
        /// </summary>
        public static void LogicalSetData<T>(string name, T value)
            where T : class
        {
            if (!InUnitTest)
            {
                CallContext.LogicalSetData(name, value);
                return;
            }
            if (Contexts.ContainsKey(name))
                Contexts[name] = value;
            else
                Contexts.Add(name, value);
        }

        /// <summary>
        /// ��������߼������Ķ���
        /// </summary>
        public static void Remove(string name)
        {
            if (!InUnitTest)
            {
                CallContext.LogicalSetData(name, null);
                return;
            }
            if (Contexts.ContainsKey(name))
                Contexts.Remove(name);
        }

        /// <summary>
        /// �Ƿ��ڵ�Ԫ���Ի����У���Ԫ���Ի�����CallContext�����ݣ�
        /// </summary>
        public static bool InUnitTest { get; set; }

    }
}