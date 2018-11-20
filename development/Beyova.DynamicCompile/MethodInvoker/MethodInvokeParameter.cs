using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public sealed class MethodInvokeParameter
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is optional.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is optional; otherwise, <c>false</c>.
        /// </value>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Gets or sets the sequence.
        /// </summary>
        /// <value>
        /// The sequence.
        /// </value>
        public int Sequence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is out.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is out; otherwise, <c>false</c>.
        /// </value>
        public bool IsOut { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in; otherwise, <c>false</c>.
        /// </value>
        public bool IsIn { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }
    }
}