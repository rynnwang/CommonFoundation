using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Class TextValueCollection.
    /// </summary>
    public class TextValueCollection : Dictionary<string, TextValue>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextValueCollection" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public TextValueCollection(int capacity) : base(capacity, StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextValueCollection" /> class.
        /// </summary>
        public TextValueCollection() : base(StringComparer.OrdinalIgnoreCase)
        {
        }
    }
}
