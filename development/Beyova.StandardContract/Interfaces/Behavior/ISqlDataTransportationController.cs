using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface ISqlDataImportController
    /// </summary>
    public interface ISqlDataTransportationController<T>
    {
        /// <summary>
        /// Imports the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        int Import(IEnumerable<SqlDataRow<T>> entities);

        /// <summary>
        /// Exports the specified from row identifier.
        /// </summary>
        /// <param name="fromRowId">From row identifier.</param>
        /// <param name="fromStamp">From stamp.</param>
        /// <param name="count">The count.</param>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        List<SqlDataRow<T>> Export(long? fromRowId, DateTime? fromStamp, int count, Dictionary<string, object> filters);
    }
}