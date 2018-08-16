using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public delegate T FunctionInjection<T>();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrioritizedFunctionInjection<T>
    {
        /// <summary>
        /// The injection chain
        /// </summary>
        protected List<FunctionInjection<T>> injectionChain = new List<FunctionInjection<T>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritizedFunctionInjection{T}"/> class.
        /// </summary>
        /// <param name="defaultInjection">The default injection.</param>
        public PrioritizedFunctionInjection(FunctionInjection<T> defaultInjection)
        {
            injectionChain.AddIfNotNull(defaultInjection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritizedFunctionInjection{T}"/> class.
        /// </summary>
        public PrioritizedFunctionInjection() : this(null)
        {

        }

        /// <summary>
        /// Prepends the specified predict.
        /// </summary>
        /// <param name="predict">The predict.</param>
        public void Prepend(FunctionInjection<T> predict)
        {
            if (predict != null)
            {
                injectionChain.Insert(0, predict);
            }
        }

        /// <summary>
        /// Appends the specified predict.
        /// </summary>
        /// <param name="predict">The predict.</param>
        public void Append(FunctionInjection<T> predict)
        {
            if (predict != null)
            {
                injectionChain.Add(predict);
            }
        }

        /// <summary>
        /// Gets the injection.
        /// </summary>
        /// <value>
        /// The injection.
        /// </value>
        public FunctionInjection<T> Injection
        {
            get
            {
                return Invoke;
            }
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <returns></returns>
        public T Invoke()
        {
            int priority = 0;
            T result = default(T);

            try
            {
                foreach (var one in injectionChain)
                {
                    if (one != null)
                    {
                        result = one.Invoke();

                        if (InValid(result))
                        {
                            break;
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { priority });
            }
        }

        /// <summary>
        /// Ins the valid.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected bool InValid(T obj)
        {
            return !obj.Equals(default(T));
        }
    }
}
