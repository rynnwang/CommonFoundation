using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class TagCollection : HashSet<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagCollection"/> class.
        /// </summary>
        public TagCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TagCollection"/> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public TagCollection(IEnumerable<string> items) : base(items, StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}