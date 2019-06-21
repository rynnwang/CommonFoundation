using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class MatrixList.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MatrixList<T> : MatrixList<string, T>
    {
        // For Json serialization working well, constructor with no parameter is needed.
        // Constructor with default value for parameter based constructor would NOT work.

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{T}" /> class.
        /// </summary>
        public MatrixList() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{T}" /> class.
        /// </summary>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public MatrixList(bool keyCaseSensitive)
            : base(keyCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{T}" /> class.
        /// </summary>
        /// <param name="matrixList">The matrix list.</param>
        public MatrixList(MatrixList<T> matrixList) : base(matrixList)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{T}"/> class.
        /// </summary>
        /// <param name="matrixList">The matrix list.</param>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public MatrixList(MatrixList<T> matrixList, bool keyCaseSensitive) : base(matrixList, keyCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
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
    /// Class MatrixList.
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    public class MatrixList<TKey, TValue> : MatrixContainer<TKey, List<TValue>, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{TKey,TValue}" /> class.
        /// </summary>
        public MatrixList() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public MatrixList(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public MatrixList(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="valueComparer">The value comparer.</param>
        public MatrixList(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) : base(keyComparer, valueComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="matrixList">The matrix list.</param>
        public MatrixList(MatrixList<TKey, TValue> matrixList) : base(matrixList)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="matrixList">The matrix list.</param>
        /// <param name="comparer">The comparer.</param>
        public MatrixList(MatrixList<TKey, TValue> matrixList, IEqualityComparer<TKey> comparer) : base(matrixList, comparer)
        {
        }

        /// <summary>
        /// News the container.
        /// </summary>
        /// <param name="valueComparer">The value comparer.</param>
        /// <param name="valueCapacity">The value capacity.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected override List<TValue> NewContainer(IEqualityComparer<TValue> valueComparer, int valueCapacity, IEnumerable<TValue> values)
        {
            return values.HasItem() ? new List<TValue>(values) : new List<TValue>(valueCapacity);
        }
    }
}