using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NameValueDictionary<T> : Dictionary<string, T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NameValueDictionary{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public NameValueDictionary(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameValueDictionary{T}"/> class.
        /// </summary>
        public NameValueDictionary() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameValueDictionary{T}"/> class.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        public NameValueDictionary(IDictionary<string, T> dictionary) : base(dictionary, StringComparer.OrdinalIgnoreCase)
        {
        }

        #endregion
    }
}
