using System;
using System.Collections.Generic;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class MultipleSelectionOptions<TModel> : MultipleSelectionOptions<TModel, Guid>
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public class MultipleSelectionOptions<TModel, TIdentifier> : SelectionOptions<TModel, IEnumerable<TIdentifier>>
    {
    }
}