using System;
using Microsoft.Extensions.DependencyInjection;

namespace Agebull.Common.Ioc
{
    /// <summary>
    ///  �򵥵�����ע����(����ڲ�ʹ��,�벻Ҫ����)
    /// </summary>
    public class IocHelper
    {
        /// <summary>
        /// ����ע�����
        /// </summary>
        public IServiceProvider Provider => _provider;

        private static IServiceProvider _provider;

        /// <summary>
        /// ��ʾʽ�������ö���(����)
        /// </summary>
        /// <param name="service"></param>
        public static void SetServiceCollection(IServiceCollection service)
        {
            if (_serviceCollection != null)
            {
                foreach (var dod in _serviceCollection)
                    service.Add(dod);
            }
            _serviceCollection = service;
        }

        private static IServiceCollection _serviceCollection;

        /// <summary>
        /// ȫ������
        /// </summary>
        public static IServiceCollection ServiceCollection =>
            _serviceCollection ?? (_serviceCollection = new ServiceCollection());

        /// <summary>
        /// ע��IOC����
        /// </summary>
        public static void SetServiceProvider(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// ���ɽӿ�ʵ��
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
           if(_provider == null)
               throw new NotSupportedException("IocHelper��Provider����Ϊ��,�Ƿ����ǵ���SetServiceProvider������");
            return _provider.GetService<TInterface>();
        }

        /// <summary>
        /// ���ɽӿ�ʵ��(���û��ע����ʹ��Ĭ��ʵ��)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TDefault"></typeparam>
        /// <returns></returns>
        public static TInterface CreateBut<TInterface,TDefault>()
            where TInterface : class
            where TDefault : class, TInterface,new()
        {
            if (_provider == null)
                throw new NotSupportedException("IocHelper��Provider����Ϊ��,�Ƿ����ǵ���SetServiceProvider������");
            return _provider.GetService<TInterface>() ?? new TDefault();
        }
    }
}