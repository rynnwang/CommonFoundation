using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class SingleParameterInstanceConstructure : SingleParameterInstanceConstructure<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SingleParameterInstanceConstructure"/> class.
        /// </summary>
        public SingleParameterInstanceConstructure() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleParameterInstanceConstructure{T}"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameter">The parameter.</param>
        public SingleParameterInstanceConstructure(Type type, string parameter) : base(type, parameter)
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleParameterInstanceConstructure<T>
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public T Parameter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleParameterInstanceConstructure{T}"/> class.
        /// </summary>
        public SingleParameterInstanceConstructure()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleParameterInstanceConstructure{T}"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="parameter">The parameter.</param>
        public SingleParameterInstanceConstructure(Type type, T parameter)
        {
            Type = type;
            Parameter = parameter;
        }
    }
}