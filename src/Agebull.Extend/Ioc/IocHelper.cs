using System;
using System.Threading;
using System.Threading.Tasks;
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
        public IServiceProvider ServiceProvider => _provider;

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
        public static IServiceCollection ServiceCollection => _serviceCollection ?? (_serviceCollection = new ServiceCollection());
        /// <summary>
        /// �ڲ���Χ����,��¼TaskID��Ϊ����������
        /// </summary>
        class ScopeData
        {
            public IServiceScope Scope;
            public int? TaskId;
        }

        private static readonly AsyncLocal<ScopeData> local = new AsyncLocal<ScopeData>();

        /// <summary>
        /// ���ɽӿ�ʵ��
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ScopeProvider
        {
            get
            {
                if (local.Value != null)
                    return local.Value.Scope.ServiceProvider;
                if (_provider == null)
                {
                    _provider = ServiceCollection.BuildServiceProvider();
                }
                local.Value = new ScopeData
                {
                    Scope = _provider.GetService<IServiceScopeFactory>().CreateScope(),
                    TaskId = Task.CurrentId
                };
                return local.Value.Scope.ServiceProvider;
            }
        }
        /// <summary>
        /// ������������Χ(�����ͷ�,����Ҫ�ȵ�GC����,��Դʹ���ʲ��ɿ�)
        /// </summary>
        public static void DisposeScope()
        {
            if (local.Value != null && local.Value.TaskId == Task.CurrentId)
                local.Value.Scope.Dispose();
        }
        /// <summary>
        /// ���ɽӿ�ʵ��
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
            return ScopeProvider.GetService<TInterface>();
        }

        /// <summary>
        /// ���ɽӿ�ʵ��(���û��ע����ʹ��Ĭ��ʵ��)
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TDefault"></typeparam>
        /// <returns></returns>
        public static TInterface CreateBut<TInterface, TDefault>()
            where TInterface : class
            where TDefault : class, TInterface, new()
        {
            return ScopeProvider.GetService<TInterface>() ?? new TDefault();
        }
    }
}