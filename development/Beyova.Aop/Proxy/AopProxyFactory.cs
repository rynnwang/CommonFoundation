using System;
using System.Collections.Generic;

namespace Beyova.AOP
{
    /// <summary>
    /// Class AopProxyFactory.
    /// </summary>
    public static class AopProxyFactory
    {
        /// <summary>
        /// The aop factory output namespace
        /// </summary>
        private const string AopFactoryOutputNamespace = "Beyova.Aop.Tmp";

        /// <summary>
        /// The proxy namespace format
        /// </summary>
        private const string ProxyNamespaceFormat = "Beyova.Aop.Tmp._{0}";

        /// <summary>
        /// The proxy options
        /// </summary>
        private static Dictionary<Type, AopProxyOptions> proxyOptionsCollection = new Dictionary<Type, AopProxyOptions>();

        /// <summary>
        /// The proxy instances
        /// </summary>
        private static Dictionary<Type, object> proxyInstances = new Dictionary<Type, object>();

        /// <summary>
        /// The locker
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// Creates the aop interface proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="injectionDelegates">The injection delegates.</param>
        /// <returns></returns>
        public static object CreateAopInterfaceProxy<T>(MethodInjectionDelegates injectionDelegates = null)
            where T : class, new()
        {
            return InternalAsAopInterfaceProxy(new T(), false, injectionDelegates);
            // Cannot return AS T. Because in this case, T would be class not interface, As T would always be failed so that only null can be returned.
        }

        /// <summary>
        /// As aop interface proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="injectionDelegates">The injection delegates.</param>
        /// <returns></returns>
        public static object AsAopInterfaceProxy<T>(this T instance, MethodInjectionDelegates injectionDelegates = null)
            where T : class
        {
            return InternalAsAopInterfaceProxy(instance, false, injectionDelegates) as T;
        }

        /// <summary>
        /// As the aop interface proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="reuseInstance">if set to <c>true</c> [reuse instance].</param>
        /// <param name="injectionDelegates">The injection delegates.</param>
        /// <returns></returns>
        private static object InternalAsAopInterfaceProxy<T>(this T instance, bool reuseInstance, MethodInjectionDelegates injectionDelegates)
            where T : class
        {
            var type = typeof(T);
            object proxiedInstance = null;

            try
            {
                instance.CheckNullObject(nameof(instance));

                if (!proxyOptionsCollection.ContainsKey(type))
                {
                    lock (locker)
                    {
                        if (!proxyOptionsCollection.ContainsKey(type))
                        {
                            AopProxyOptions proxyOptions = new AopProxyOptions
                            {
                                Instance = instance,
                                MethodInjectionDelegates = injectionDelegates
                            };

                            PrepareProxy<T>(proxyOptions);
                            proxyOptionsCollection.Add(type, proxyOptions);

                            proxiedInstance = CreateInstance(proxyOptions, instance);
                            proxyInstances[type] = proxiedInstance;
                            return type.IsInterface ? proxiedInstance as T : proxiedInstance;
                        }
                    }
                }

                return (reuseInstance ? proxyInstances[type] : CreateInstance(proxyOptionsCollection[type], instance)) as T;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { T = type.FullName });
            }
        }

        #region Make Proxy

        /// <summary>
        /// Prepares the proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyOptions">The proxy options.</param>
        private static void PrepareProxy<T>(AopProxyOptions proxyOptions)
            where T : class
        {
            if (proxyOptions != null)
            {
                var type = typeof(T);
                var typeName = string.Format("{0}AopProxy", type.Name);

                try
                {
                    //var aopAttribute = type.GetCustomAttribute<BaseAOPAttribute>(true);
                    //proxyOptions.MethodInjectionDelegates = aopAttribute?.MethodInjectionDelegates ?? proxyOptions.MethodInjectionDelegates;

                    var generator = new AopProxyGenerator<T>(string.Format(ProxyNamespaceFormat, type.Namespace), typeName);
                    var code = generator.GenerateCode();

                    code.CheckEmptyString(nameof(code));

                    TempAssemblyProvider assemblyProvider = new TempAssemblyProvider();
                    assemblyProvider.CreateTempAssembly(code.AsArray(), TempAssemblyProvider.GetCurrentAppDomainAssemblyLocations());

                    proxyOptions.ProxiedType = ReflectionExtension.SmartGetType(typeName);
                }
                catch (Exception ex)
                {
                    throw new Beyova.Diagnostic.InitializationFailureException(typeName, ex, minor: "AopProxyGeneration", data: new { type = type?.FullName });
                }
            }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyOptions">The proxy options.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>System.Object.</returns>
        private static object CreateInstance<T>(AopProxyOptions proxyOptions, T instance)
            where T : class
        {
            return (proxyOptions != null && instance != null) ? Activator.CreateInstance(proxyOptions.ProxiedType, instance, proxyOptions.MethodInjectionDelegates) : null;
        }

        #endregion Make Proxy
    }
}