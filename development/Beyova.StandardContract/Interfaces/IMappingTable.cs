using System.Collections;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface IMappingTable
    /// </summary>
    public interface IMappingTable : IDictionary
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether [case sensitive].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [case sensitive]; otherwise, <c>false</c>.
        /// </value>
        bool CaseSensitive { get; }

        /// <summary>
        /// Gets a value indicating whether [value unique].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [value unique]; otherwise, <c>false</c>.
        /// </value>
        bool ValueUnique { get; }

        #endregion Properties
    }
}