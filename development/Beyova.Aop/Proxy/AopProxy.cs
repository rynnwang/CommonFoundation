namespace Beyova.AOP
{
    /// <summary>
    /// Class AopProxy. Which is base class of what your want to AOP.
    /// </summary>
    public abstract class AopProxy<T>
        where T : class
    {
        /// <summary>
        /// The instance
        /// </summary>
        protected T _instance;

        /// <summary>
        /// The injection delegates
        /// </summary>
        protected MethodInjectionDelegates _injectionDelegates;

        /// <summary>
        /// Initializes a new instance of the <see cref="AopProxy{T}" /> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="injectionDelegates">The injection delegates.</param>
        protected AopProxy(T instance, MethodInjectionDelegates injectionDelegates)
        {
            _instance = instance;
            _injectionDelegates = injectionDelegates ?? new MethodInjectionDelegates();
        }

        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="injectionDelegates">The injection delegates.</param>
        /// <returns></returns>
        public static AopProxy<T> Create(T instance, MethodInjectionDelegates injectionDelegates = null)
        {
            return AopProxyFactory.AsAopInterfaceProxy(instance, injectionDelegates) as AopProxy<T>;
        }
    }
}