using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#if FRAMEWORK
using System.Runtime.Remoting.Messaging;
#endif

namespace Agebull.Common
{
    /// <summary>
    /// �����ĸ�����
    /// </summary>
    public static class ContextHelper
    {
#if FRAMEWORK
/// <summary>
/// ��Ԫ����ʱʹ�õ��ֵ�
/// </summary>
        private static readonly Dictionary<string, object> Contexts = new Dictionary<string, object>();
#else
        /// <summary>
        /// ��Ԫ����ʱʹ�õ��ֵ�
        /// </summary>
        private static readonly Dictionary<string, object> Contexts = new Dictionary<string, object>();
#endif

        /// <summary>
        /// �õ������߼������Ķ���
        /// </summary>
        public static T LogicalGetData<T>(string name)
            where T : class
        {
            if (!InUnitTest)
            {
#if FRAMEWORK
                return CallContext.LogicalGetData(name) as T;
#else
                var slot = Thread.GetNamedDataSlot($"ctx_bl_{name}");
                if (slot == null)
                {
                    return null;
                }
                return  Thread.GetData(slot) as T;
#endif
            }
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
#if FRAMEWORK
                CallContext.LogicalSetData(name, value);
#else
                
                var key = $"ctx_bl_{name}";
                var slot = Thread.GetNamedDataSlot(key) ?? Thread.AllocateNamedDataSlot(key);
                Thread.SetData(slot, value);
#endif
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
#if FRAMEWORK
                CallContext.LogicalSetData(name, null);
#else
                var slot = Thread.GetNamedDataSlot($"ctx_bl_{name}");
                if (slot != null)
                {
                    Thread.FreeNamedDataSlot($"ctx_bl_{name}");
                }
#endif
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