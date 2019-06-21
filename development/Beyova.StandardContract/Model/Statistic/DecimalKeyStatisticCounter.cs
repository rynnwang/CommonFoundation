using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class DecimalKeyStatisticCounter<TKey> : StatisticCounter<TKey, Decimal>
        where TKey : struct, IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecimalKeyStatisticCounter{TKey}"/> class.
        /// </summary>
        public DecimalKeyStatisticCounter() : base() { }

        /// <summary>
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        public DecimalKeyStatisticCounter(EqualityComparer<TKey> equalityComparer) : base(equalityComparer) { }

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
        protected override decimal Add(decimal orginalValue, decimal counter)
        {
            return orginalValue + counter;
        }
    }
}