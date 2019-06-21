using Newtonsoft.Json;
using System;

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
        [JsonProperty("optionValue")]
        public T OptionValue { get; set; }

        /// <summary>
        /// Gets or sets the option text.
        /// </summary>
        /// <value>
        /// The option text.
        /// </value>
        [JsonProperty("optionText")]
        public string OptionText { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class FlexibleSelectOption : FlexibleSelectOption<Int32>
    {
    }
}