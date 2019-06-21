using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// Class MatrixCollection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MatrixCollection<T> : MatrixCollection<string, T>
    {
        // For Json serialization working well, constructor with no parameter is needed.
        // Constructor with default value for parameter based constructor would NOT work.

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{T}" /> class.
        /// </summary>
        public MatrixCollection() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{T}" /> class.
        /// </summary>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public MatrixCollection(bool keyCaseSensitive)
            : base(keyCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{T}" /> class.
        /// </summary>
        /// <param name="MatrixCollection">The matrix list.</param>
        public MatrixCollection(MatrixCollection<T> MatrixCollection) : base(MatrixCollection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{T}"/> class.
        /// </summary>
        /// <param name="MatrixCollection">The matrix list.</param>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        public MatrixCollection(MatrixCollection<T> MatrixCollection, bool keyCaseSensitive) : base(MatrixCollection, keyCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
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
    /// Class MatrixCollection.
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    public class MatrixCollection<TKey, TValue> : MatrixContainer<TKey, Collection<TValue>, TValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{TKey,TValue}" /> class.
        /// </summary>
        public MatrixCollection() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Dictionary`2" /> can contain.</param>
        public MatrixCollection(int capacity) : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.Dictionary`2" /> class that is empty, has the default initial capacity, and uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1" />.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1" /> implementation to use when comparing keys, or null to use the default <see cref="T:System.Collections.Generic.EqualityComparer`1" /> for the type of the key.</param>
        public MatrixCollection(IEqualityComparer<TKey> comparer) : base(comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="valueComparer">The value comparer.</param>
        public MatrixCollection(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer) : base(keyComparer, valueComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="MatrixCollection">The matrix list.</param>
        public MatrixCollection(MatrixCollection<TKey, TValue> MatrixCollection) : base(MatrixCollection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixCollection{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="MatrixCollection">The matrix list.</param>
        /// <param name="comparer">The comparer.</param>
        public MatrixCollection(MatrixCollection<TKey, TValue> MatrixCollection, IEqualityComparer<TKey> comparer) : base(MatrixCollection, comparer)
        {
        }

        /// <summary>
        /// News the container.
        /// </summary>
        /// <param name="valueComparer">The value comparer.</param>
        /// <param name="valueCapacity">The value capacity.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected override Collection<TValue> NewContainer(IEqualityComparer<TValue> valueComparer, int valueCapacity, IEnumerable<TValue> values)
        {
            return values.HasItem() ? new Collection<TValue>(values?.ToList()) : new Collection<TValue>();
        }
    }
}