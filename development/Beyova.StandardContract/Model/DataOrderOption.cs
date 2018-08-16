using System;

namespace Beyova
{
    /// <summary>
    /// Class DataOrderOption.
    /// </summary>
    public sealed class DataOrderOption
    {
        /// <summary>
        /// 
        /// </summary>
        public enum OrderMethod
        {
            /// <summary>
            /// The ascending
            /// </summary>
            Ascending = 0,
            /// <summary>
            /// The descending
            /// </summary>
            Descending = 1
        }

        /// <summary>
        /// Gets or sets the by.
        /// </summary>
        /// <value>
        /// The by.
        /// </value>
        public string By { get; set; }

        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public OrderMethod Method { get; set; }
    }
}