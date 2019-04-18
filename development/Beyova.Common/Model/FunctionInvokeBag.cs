using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class FunctionInvokeBag<TParameter, TOutput> : FunctionInvokeBag<TParameter>
    {
        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public Func<TParameter, TOutput> Function { get; set; }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <returns></returns>
        public TOutput Invoke()
        {
            return (IsValidParameter(Parameter) && Function != null) ? Function.Invoke(Parameter) : default(TOutput);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TProvisioningParameter">The type of the provisioning parameter.</typeparam>
    /// <typeparam name="TInputParameter">The type of the input parameter.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    public class FunctionInvokeBag<TProvisioningParameter, TInputParameter, TOutput> : FunctionInvokeBag<TProvisioningParameter>
    {
        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public Func<TProvisioningParameter, TInputParameter, TOutput> Function { get; set; }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <returns></returns>
        public TOutput Invoke(TInputParameter inputParameter)
        {
            return (IsValidParameter(Parameter) && Function != null) ? Function.Invoke(Parameter, inputParameter) : default(TOutput);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FunctionInvokeBag<T>
    {
        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public T Parameter { get; set; }

        /// <summary>
        /// Determines whether [is valid parameter] [the specified parameter].
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if [is valid parameter] [the specified parameter]; otherwise, <c>false</c>.
        /// </returns>
        protected virtual bool IsValidParameter(T parameter)
        {
            return parameter != null;
        }
    }
}