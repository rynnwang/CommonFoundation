using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FunctionInjectionHostTypeMapAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the target static.
        /// </summary>
        /// <value>
        /// The type of the target static.
        /// </value>
        public Type TargetStaticType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionInjectionHostTypeMapAttribute"/> class.
        /// </summary>
        /// <param name="staticTypeOfFunctionInjectionContainer">The static type of function injection container.</param>
        public FunctionInjectionHostTypeMapAttribute(Type staticTypeOfFunctionInjectionContainer)
        {
            TargetStaticType = staticTypeOfFunctionInjectionContainer;
        }
    }
}