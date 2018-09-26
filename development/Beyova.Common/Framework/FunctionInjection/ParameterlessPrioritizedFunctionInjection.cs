using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public delegate T ParameterlessFunctionInjection<T>();

    /// <summary>
    /// Class <c>ParameterlessPrioritizedFunctionInjection</c> is commonly as a kind of DI, used when specific property or method contract is defined, but need depedency to determine implementation.
    /// It designs as chain mode, so that when multiple implementation is applied, the right one would be picked to use.
    /// </summary>
    /// <typeparam name="T">Return  type of {T}</typeparam>
    public class ParameterlessPrioritizedFunctionInjection<T>
    {
        /// <summary>
        /// The injection chain
        /// </summary>
        protected List<ParameterlessFunctionInjection<T>> _injectionChain = new List<ParameterlessFunctionInjection<T>>();

        /// <summary>
        /// The is static
        /// </summary>
        protected bool _isStatic;

        /// <summary>
        /// The cached result
        /// </summary>
        protected T _cachedResult;

        /// <summary>
        /// The has cache value
        /// </summary>
        protected bool _hasCacheValue = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterlessPrioritizedFunctionInjection{T}" /> class.
        /// </summary>
        /// <param name="defaultInjection">The default injection.</param>
        /// <param name="isStatic">if set to <c>true</c> [is static].</param>
        public ParameterlessPrioritizedFunctionInjection(ParameterlessFunctionInjection<T> defaultInjection, bool isStatic = false)
        {
            _injectionChain.AddIfNotNull(defaultInjection);
            _isStatic = isStatic;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterlessPrioritizedFunctionInjection{T}"/> class.
        /// </summary>
        public ParameterlessPrioritizedFunctionInjection() : this(null)
        {
        }

        /// <summary>
        /// Prepends the specified predict.
        /// </summary>
        /// <param name="predict">The predict.</param>
        public void Prepend(ParameterlessFunctionInjection<T> predict)
        {
            if (predict != null)
            {
                _injectionChain.Insert(0, predict);
            }
        }

        /// <summary>
        /// Appends the specified predict.
        /// </summary>
        /// <param name="predict">The predict.</param>
        public void Append(ParameterlessFunctionInjection<T> predict)
        {
            if (predict != null)
            {
                _injectionChain.Add(predict);
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ParameterlessPrioritizedFunctionInjection{T}"/>.
        /// </summary>
        /// <param name="injectionObject">The injection object.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator T(ParameterlessPrioritizedFunctionInjection<T> injectionObject)
        {
            return injectionObject.Invoke();
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <returns></returns>
        public T Invoke()
        {
            if (_hasCacheValue)
            {
                return _cachedResult;
            }

            if (!_injectionChain.HasItem())
            {
                throw new NotImplementedException();
            }

            T result = default(T);

            try
            {
                result = (_injectionChain.FirstOrDefault()).Invoke();

                if (_isStatic)
                {
                    _cachedResult = result;
                    _hasCacheValue = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }
    }
}
