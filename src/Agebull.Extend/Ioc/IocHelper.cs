using System;
using System.Collections.Generic;

namespace Agebull.Common.Ioc
{
    /// <summary>
    ///  �򵥵�����ע����
    /// </summary>
    public class IocHelper
    {
        /// <summary>
        /// ��ע�����
        /// </summary>
        public static Dictionary<Type, Func<object>> InterfaceDictionary = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// ע��ӿ�ʵ��
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TClass"></typeparam>
        public static void Regist<TInterface, TClass>()
            where TClass : class, new()
        {
            if (InterfaceDictionary.ContainsKey(typeof(TInterface)))
                InterfaceDictionary[typeof(TInterface)] = () => new TClass();
            else
                InterfaceDictionary.Add(typeof(TInterface), () => new TClass());
        }

        /// <summary>
        /// ���ɽӿ�ʵ��
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
            if (InterfaceDictionary.ContainsKey(typeof(TInterface)))
                return InterfaceDictionary[typeof(TInterface)]() as TInterface;
            throw new Exception($"�ӿ�{typeof(TInterface)}������Ҫ��ʵ��û��ע��");
        }
    }
}