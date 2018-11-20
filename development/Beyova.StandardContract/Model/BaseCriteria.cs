using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TStartIndex">The type of the start index.</typeparam>
    public abstract class BaseCriteria<TStartIndex> : BaseCriteria
    {
        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        public TStartIndex StartIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCriteria{TStartIndex}"/> class.
        /// </summary>
        public BaseCriteria() : base()
        {
        }
    }

    /// <summary>
    /// Class BaseCriteria.
    /// </summary>
    public class BaseCriteria : ICriteria
    {
        #region Properties

        /// <summary>
        /// Gets or sets the key of the object.
        /// If this value is assigned, other criteria would be ignored.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order by.
        /// </summary>
        /// <value>
        /// The order by.
        /// </value>
        public List<DataOrderOption> OrderBy { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [order descending].
        /// </summary>
        /// <value><c>true</c> if [order descending]; otherwise, <c>false</c>.</value>
        [Obsolete("Use OrderBy")] public bool OrderDescending { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCriteria" /> class.
        /// </summary>
        public BaseCriteria()
        {
        }

        #endregion Constructor
    }
}