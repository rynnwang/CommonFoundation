using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FlexibleSelectOption<T>
        where T : IConvertible
    {
        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        public T OptionValue { get; set; }

        /// <summary>
        /// Gets or sets the option text.
        /// </summary>
        /// <value>
        /// The option text.
        /// </value>
        public string OptionText { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FlexibleSelectOption : FlexibleSelectOption<Int32>
    {
    }
}