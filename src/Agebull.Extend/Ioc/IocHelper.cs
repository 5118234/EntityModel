using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Agebull.Common.Ioc
{
    /// <summary>
    ///     �򵥵�����ע����(����ڲ�ʹ��,�벻Ҫ����)
    /// </summary>
    public class IocHelper
    {
        private static IServiceProvider _provider;

        private static IServiceCollection _serviceCollection;

        private static readonly AsyncLocal<ScopeData> Local = new AsyncLocal<ScopeData>();

        /// <summary>
        ///     ����ע�����
        /// </summary>
        public IServiceProvider ServiceProvider => _provider;

        /// <summary>
        ///     ȫ������
        /// </summary>
        public static IServiceCollection ServiceCollection =>
            _serviceCollection ?? (_serviceCollection = new ServiceCollection());

        /// <summary>
        ///     ���ɽӿ�ʵ��
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider ScopeProvider
        {
            get
            {
                if (Local.Value != null)
                    return Local.Value.Scope.ServiceProvider;
                if (_provider == null) _provider = ServiceCollection.BuildServiceProvider();
                Local.Value = new ScopeData
                {
                    Scope = _provider.GetService<IServiceScopeFactory>().CreateScope(),
                    TaskId = Task.CurrentId
                };
                return Local.Value.Scope.ServiceProvider;
            }
        }

        /// <summary>
        ///     ��ʾʽ�������ö���(����)
        /// </summary>
        /// <param name="service"></param>
        public static void SetServiceCollection(IServiceCollection service)
        {
            if (_serviceCollection != null)
                foreach (var dod in _serviceCollection)
                    service.Add(dod);
            _serviceCollection = service;
        }

        /// <summary>
        ///     ���ɽӿ�ʵ��
        /// </summary>
        /// <returns></returns>
        public static void Update()
        {
            _provider = ServiceCollection.BuildServiceProvider();
            Local.Value = new ScopeData
            {
                Scope = _provider.GetService<IServiceScopeFactory>().CreateScope(),
                TaskId = Task.CurrentId
            };
        }

        /// <summary>
        ///     ������������Χ(�����ͷ�,����Ҫ�ȵ�GC����,��Դʹ���ʲ��ɿ�)
        /// </summary>
        public static void DisposeScope()
        {
            if (Local.Value != null && Local.Value.TaskId == Task.CurrentId)
                Local.Value.Scope.Dispose();
        }

        /// <summary>
        ///     ���ɽӿ�ʵ��
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns></returns>
        public static TInterface Create<TInterface>()
            where TInterface : class
        {
            return ScopeProvider.GetService<TInterface>();
        }

        /// <summary>
        ///     ���ɽӿ�ʵ��(���û��ע����ʹ��Ĭ��ʵ��)
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

        /// <summary>
        ///     �ڲ���Χ����,��¼TaskID��Ϊ����������
        /// </summary>
        private class ScopeData
        {
            public IServiceScope Scope;
            public int? TaskId;
        }

        #region ��������

        /// <summary>
        ///     Adds a transient service of the type specified in <paramref name="serviceType" /> with an
        ///     implementation of the type specified in <paramref name="implementationType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(Type serviceType, Type implementationType)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddTransient(serviceType, implementationType);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <paramref name="serviceType" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddTransient(serviceType, implementationFactory);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddTransient<TService, TImplementation>();
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <paramref name="serviceType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient(Type serviceType)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddTransient(serviceType);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService>() where TService : class
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddTransient<TService>();
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddTransient(implementationFactory);
        }

        /// <summary>
        ///     Adds a transient service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> using the
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Transient" />
        public static IServiceCollection AddTransient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddTransient(implementationFactory);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> with an
        ///     implementation of the type specified in <paramref name="implementationType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(Type serviceType, Type implementationType)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddScoped(serviceType, implementationType);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddScoped(serviceType, implementationFactory);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddScoped<TService, TImplementation>();
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <paramref name="serviceType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped(Type serviceType)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddScoped(serviceType);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService>() where TService : class
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddScoped<TService>();
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddScoped(implementationFactory);
        }

        /// <summary>
        ///     Adds a scoped service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> using the
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped" />
        public static IServiceCollection AddScoped<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddScoped(implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        ///     implementation of the type specified in <paramref name="implementationType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationType">The implementation type of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType, Type implementationType)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton(serviceType, implementationType);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType,
            Func<IServiceProvider, object> implementationFactory)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton(serviceType, implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton<TService, TImplementation>();
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register and the implementation to use.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton(serviceType);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>() where TService : class
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton<TService>();
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with a
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton(implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///     implementation type specified in <typeparamref name="TImplementation" /> using the
        ///     factory specified in <paramref name="implementationFactory" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton(implementationFactory);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <paramref name="serviceType" /> with an
        ///     instance specified in <paramref name="implementationInstance" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="serviceType">The type of the service to register.</param>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton(Type serviceType, object implementationInstance)
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton(serviceType, implementationInstance);
        }

        /// <summary>
        ///     Adds a singleton service of the type specified in <typeparamref name="TService" /> with an
        ///     instance specified in <paramref name="implementationInstance" /> to the
        ///     specified <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="implementationInstance">The instance of the service.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <seealso cref="F:Microsoft.Extensions.DependencyInjection.ServiceLifetime.Singleton" />
        public static IServiceCollection AddSingleton<TService>(TService implementationInstance) where TService : class
        {
            _provider = null;
            Local.Value = null;
            return _serviceCollection.AddSingleton(typeof(TService), implementationInstance);
        }

        #endregion
    }
}