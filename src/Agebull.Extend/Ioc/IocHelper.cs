using System;
using System.Collections.Generic;

namespace GoodLin.Common.Ioc
{
    /// <summary>
    /// ��ݼ����
    /// </summary>
    public class IocHelper
    {
        public static Dictionary<Type, Func<object>> InterfaceDictionary = new Dictionary<Type, Func<object>>();


        public static void Regist<TInterface, TClass>()
            where TClass : class, new()
        {
            if (InterfaceDictionary.ContainsKey(typeof(TInterface)))
                InterfaceDictionary[typeof(TInterface)] = () => new TClass();
            else
                InterfaceDictionary.Add(typeof(TInterface), () => new TClass());
        }


        public static TInterface Create<TInterface>()
            where TInterface : class
        {
            if (InterfaceDictionary.ContainsKey(typeof(TInterface)))
                return InterfaceDictionary[typeof(TInterface)]() as TInterface;
            throw new Exception($"�ӿ�{typeof(TInterface)}������Ҫ��ʵ��û��ע��");
        }
    }
}