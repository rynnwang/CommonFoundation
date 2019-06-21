using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MatrixHashSet<T> : MatrixHashSet<string, T>
    {
        // For Json serialization working well, constructor with no parameter is needed.
        // Constructor with default value for parameter based constructor would NOT work.

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{T}" /> class.
        /// </summary>
        public MatrixHashSet() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{T}" /> class.
        /// </summary>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public MatrixHashSet(bool keyCaseSensitive)
            : base(keyCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{T}" /> class.
        /// </summary>
        /// <param name="MatrixHashSet">The matrix list.</param>
        public MatrixHashSet(MatrixHashSet<T> MatrixHashSet) : base(MatrixHashSet)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{T}"/> class.
        /// </summary>
        /// <param name="MatrixHashSet">The matrix list.</param>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public MatrixHashSet(MatrixHashSet<T> MatrixHashSet, bool keyCaseSensitive) : base(MatrixHashSet, keyCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Determines whether [is key valid] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [is key valid] [the specified key]; otherwise, <c>false</c>.</returns>
        protected override bool IsKeyValid(string key)
        {
            return !string.IsNullOrWhiteSpace(key);
        }
    }

    /// <summary>
    /// Class MatrixHashSet.
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    public class MatrixHashSet<TKey, TValue> : MatrixContainer<TKey, HashSet<TValue>, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{TKey,TValue}" /> class.
        /// </summary>
        public MatrixHashSet() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public MatrixHashSet(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public MatrixHashSet(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="valueComparer">The value comparer.</param>
        public MatrixHashSet(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) : base(keyComparer, valueComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="MatrixHashSet">The matrix list.</param>
        public MatrixHashSet(MatrixHashSet<TKey, TValue> MatrixHashSet) : base(MatrixHashSet)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixHashSet{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="MatrixHashSet">The matrix list.</param>
        /// <param name="comparer">The comparer.</param>
        public MatrixHashSet(MatrixHashSet<TKey, TValue> MatrixHashSet, IEqualityComparer<TKey> comparer) : base(MatrixHashSet, comparer)
        {
        }

        /// <summary>
        /// News the container.
        /// </summary>
        /// <param name="valueComparer">The value comparer.</param>
        /// <param name="valueCapacity">The value capacity.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected override HashSet<TValue> NewContainer(IEqualityComparer<TValue> valueComparer, int valueCapacity, IEnumerable<TValue> values)
        {
            return values.HasItem() ? new HashSet<TValue>(values, valueComparer) : new HashSet<TValue>(valueComparer);
        }
    }
}