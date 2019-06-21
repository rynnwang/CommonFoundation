using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class StatisticCounter<TKey> : StatisticCounter<TKey, Int32>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticCounter{TKey}"/> class.
        /// </summary>
        public StatisticCounter() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticCounter{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        public StatisticCounter(EqualityComparer<TKey> equalityComparer) : base(equalityComparer) { }

        /// <summary>
        /// Adds the counter.
        /// </summary>
        /// <param name="key">The key.</param>
        public void AddCounter(TKey key)
        {
            AddCounter(key, 1);
        }

        /// <summary>
        /// Adds the specified orginal value.
        /// </summary>
        /// <param name="orginalValue">The orginal value.</param>
        /// <param name="counter">The counter.</param>
        /// <returns></returns>
        protected override int Add(int orginalValue, int counter)
        {
            return orginalValue + counter;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public abstract class StatisticCounter<TKey, TValue> : Dictionary<TKey, TValue>
        where TValue : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticCounter{TKey, TValue}"/> class.
        /// </summary>
        public StatisticCounter() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticCounter{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        public StatisticCounter(EqualityComparer<TKey> equalityComparer) : base(equalityComparer) { }

        /// <summary>
        /// Adds the counter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="counterValue">The counter value.</param>
        public void AddCounter(TKey key, TValue counterValue)
        {
            if (ContainsKey(key))
            {
                this[key] = Add(this[key], counterValue);
            }
            else
            {
                Add(key, counterValue);
            }
        }

        /// <summary>
        /// Adds the specified orginal value.
        /// </summary>
        /// <param name="orginalValue">The orginal value.</param>
        /// <param name="counter">The counter.</param>
        /// <returns></returns>
        protected abstract TValue Add(TValue orginalValue, TValue counter);
    }
}