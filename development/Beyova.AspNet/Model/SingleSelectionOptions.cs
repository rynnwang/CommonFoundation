using System;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    public class SingleSelectionOptions<TModel> : SingleSelectionOptions<TModel, Guid?>
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TIdentifier">The type of the identifier.</typeparam>
    public class SingleSelectionOptions<TModel, TIdentifier> : SelectionOptions<TModel, TIdentifier>
    {
    }
}