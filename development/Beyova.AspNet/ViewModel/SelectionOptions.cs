using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Web
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TSelection">The type of the selection.</typeparam>
    public abstract class SelectionOptions<TModel, TSelection>
    {
        /// <summary>
        /// Gets or sets the option items.
        /// </summary>
        /// <value>
        /// The option items.
        /// </value>
        public List<TModel> OptionItems { get; set; }

        /// <summary>
        /// Gets or sets the selection.
        /// </summary>
        /// <value>
        /// The selection.
        /// </value>
        public TSelection Selection { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the class values.
        /// </summary>
        /// <value>
        /// The class values.
        /// </value>
        public string ClassValues { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }
    }
}
