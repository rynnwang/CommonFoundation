using System;
using System.Text.RegularExpressions;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class SqlDataRow.
    /// </summary>
    public class SqlDataRow<T>
    {
        /// <summary>
        /// Gets or sets the row identifier.
        /// </summary>
        /// <value>
        /// The row identifier.
        /// </value>
        public long? RowId { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public T Data { get; set; }
    }
}