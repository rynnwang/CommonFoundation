using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GlobalEntity<T>
        where T : IGlobalObjectName
    {
        #region Properties

        /// <summary>
        /// Gets or sets the object.
        /// </summary>
        /// <value>The object.</value>
        public T Object { get; set; }

        /// <summary>
        /// Gets or sets the names.
        /// </summary>
        /// <value>
        /// The names.
        /// </value>
        public Dictionary<string, string> Names { get; set; }

        #endregion Properties

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalEntity{T}"/> class.
        /// </summary>
        public GlobalEntity() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalEntity{T}"/> class.
        /// </summary>
        /// <param name="genricObject">The genric object.</param>
        public GlobalEntity(T genricObject) : this()
        {
            Object = genricObject;
        }
    }
}