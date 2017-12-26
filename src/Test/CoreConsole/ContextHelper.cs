using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;

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
        private static readonly Dictionary<Guid, Dictionary<string, object>> Contexts = new Dictionary<Guid, Dictionary<string, object>>();
#endif

        /// <summary>
        /// �õ������߼������Ķ���
        /// </summary>
        public static T LogicalGetData<T>(string name)
            where T : class
        {
#if FRAMEWORK
            if (!InUnitTest)
                return CallContext.LogicalGetData(name) as T;
            object value;
            if (!Contexts.TryGetValue(name, out value))
                return null;
            return value as T;
#else
            Guid key;
            if (Thread.CurrentPrincipal == null || !Guid.TryParse(Thread.CurrentPrincipal.Identity.Name, out key) || !Contexts.ContainsKey(key) || !Contexts[key].ContainsKey(name))
            {
                return null;
            }
            return Contexts[key][name] as T;
#endif
        }

        /// <summary>
        /// ���õ����߼������Ķ���
        /// </summary>
        public static void LogicalSetData<T>(string name, T value)
            where T : class
        {
#if FRAMEWORK
            if (!InUnitTest)
                CallContext.LogicalSetData(name, value);
            else if (Contexts.ContainsKey(name))
                Contexts[name] = value;
            else
                Contexts.Add(name, value);
#else
            Guid key;
            if (Thread.CurrentPrincipal == null || !Guid.TryParse(Thread.CurrentPrincipal.Identity.Name, out key))
            {
                key = Guid.NewGuid();
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(key.ToString()), new[] { "ctx" });
            }

            if (!Contexts.ContainsKey(key))
            {
                Contexts.Add(key, new Dictionary<string, object> { { name, value } });
            }
            else if (!Contexts[key].ContainsKey(name))
            {
                Contexts[key].Add(name, value);
            }
            else
            {
                Contexts[key][name] = value;
            }
#endif
        }

        /// <summary>
        /// ��������߼������Ķ���
        /// </summary>
        public static void Remove(string name)
        {
#if FRAMEWORK
            if (!InUnitTest)
            {
                CallContext.LogicalSetData(name, null);
            }
            if (Contexts.ContainsKey(name))
                Contexts.Remove(name);
#else
            Guid key;
            if (Thread.CurrentPrincipal == null || !Guid.TryParse(Thread.CurrentPrincipal.Identity.Name, out key) || !Contexts.ContainsKey(key) || !Contexts[key].ContainsKey(name))
            {
                return;
            }
            Contexts[key].Remove(name);
            if (Contexts[key].Count ==0)
            {
                Contexts.Remove(key);
            }
#endif
        }


        /// <summary>
        /// �Ƿ��ڵ�Ԫ���Ի����У���Ԫ���Ի�����CallContext�����ݣ�
        /// </summary>
        public static bool InUnitTest { get; set; }

    }
}