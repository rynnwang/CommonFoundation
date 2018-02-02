using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(MappingTableConverter))]
    public sealed class MappingTable : MappingTable<string>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTable" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="valueUnique">if set to <c>true</c> [value unique].</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        public MappingTable(int capacity, bool valueUnique = false, bool caseSensitive = false) : base(capacity, valueUnique, caseSensitive, caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTable" /> class.
        /// </summary>
        /// <param name="valueUnique">if set to <c>true</c> [value unique].</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        public MappingTable(bool valueUnique = false, bool caseSensitive = false) : this(defaultCapacity, valueUnique, caseSensitive)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTable" /> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="valueUnique">if set to <c>true</c> [value unique].</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        public MappingTable(IDictionary<string, string> dictionary, bool valueUnique = false, bool caseSensitive = false) : this(dictionary?.Count.Ensure(x => x > 1, defaultCapacity) ?? defaultCapacity, valueUnique, caseSensitive)
        {
            this.AddRange(dictionary);
        }

        #endregion

        /// <summary>
        /// Gets or sets the <see cref="System.String"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.String"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public new string this[string key]
        {
            get { return this.TryGetValue(key, key); }
            set
            {
                TryCheckValueDuplication(value);
                base[key] = value;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonConverter(typeof(MappingTableConverter))]
    public class MappingTable<T> : Dictionary<string, T>, IMappingTable
    {
        /// <summary>
        /// The default capacity
        /// </summary>
        protected const int defaultCapacity = 64;

        /// <summary>
        /// The value comparer
        /// </summary>
        public bool CaseSensitive { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether [value unique].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [value unique]; otherwise, <c>false</c>.
        /// </value>
        public bool ValueUnique { get; protected set; }

        /// <summary>
        /// The value comparer
        /// </summary>
        protected IEqualityComparer<T> _valueComparer;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTable"/> class.
        /// </summary>
        public MappingTable(bool valueUnique = false) : this(valueUnique, false, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTable" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="valueUnique">if set to <c>true</c> [value unique].</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <param name="valueComparer">The value comparer.</param>
        public MappingTable(int capacity, bool valueUnique = false, bool caseSensitive = false, IEqualityComparer<T> valueComparer = null) : base(capacity, caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase)
        {
            _valueComparer = valueComparer ?? EqualityComparer<T>.Default;
            this.ValueUnique = valueUnique;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTable" /> class.
        /// </summary>
        /// <param name="valueUnique">if set to <c>true</c> [value unique].</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <param name="valueComparer">The value comparer.</param>
        public MappingTable(bool valueUnique, bool caseSensitive = false, IEqualityComparer<T> valueComparer = null) : this(defaultCapacity, valueUnique, caseSensitive, valueComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingTable" /> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="valueUnique">if set to <c>true</c> [value unique].</param>
        /// <param name="caseSensitive">if set to <c>true</c> [case sensitive].</param>
        /// <param name="valueComparer">The value comparer.</param>
        public MappingTable(IDictionary<string, T> dictionary, bool valueUnique = false, bool caseSensitive = false, EqualityComparer<T> valueComparer = null) : this(dictionary?.Count.Ensure(x => x > 1, defaultCapacity) ?? defaultCapacity, valueUnique, caseSensitive, valueComparer)
        {
            this.AddRange(dictionary);
        }

        #endregion

        /// <summary>
        /// Checks the value duplication.
        /// </summary>
        /// <param name="value">The value.</param>
        protected void TryCheckValueDuplication(T value)
        {
            value.CheckNullObject(nameof(value));

            if (this.ValueUnique)
            {
                foreach (var one in this.Values)
                {
                    if (_valueComparer.Equals(one, value))
                    {
                        throw ExceptionFactory.CreateInvalidObjectException((value as IIdentifier)?.Key?.ToString(), data: value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <value>
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public new T this[string key]
        {
            get { return this.TryGetValue(key); }
            set
            {
                TryCheckValueDuplication(value);
                base[key] = value;
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public new void Add(string key, T value)
        {
            key.CheckEmptyString(nameof(key));
            TryCheckValueDuplication(value);

            base.Add(key, value);
        }

        /// <summary>
        /// Gets the mapping value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        internal T InternalGetMappingValue(string key, T defaultValue)
        {
            return string.IsNullOrWhiteSpace(key) ? defaultValue : this.TryGetValue(key, defaultValue);
        }

        /// <summary>
        /// Gets the mapped value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        internal string InternalGetMappedValue(T value, string defaultValue)
        {
            return this.SafeTryGetKey(value, defaultValue, this._valueComparer);
        }
    }
}
