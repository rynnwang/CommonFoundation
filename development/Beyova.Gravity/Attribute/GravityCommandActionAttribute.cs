using System;
using System.Collections.Generic;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityCommandActionAttribute. It is used in client side to define what action(s) is supported to execute locally.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class GravityCommandActionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the invoker.
        /// </summary>
        /// <value>The invoker.</value>
        public HashSet<IGravityInstructionInvoker> Invokers { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityCommandActionAttribute"/> class.
        /// </summary>
        /// <param name="invokers">The invokers.</param>
        public GravityCommandActionAttribute(params IGravityInstructionInvoker[] invokers)
        {
            this.Invokers = new HashSet<IGravityInstructionInvoker>(invokers, new GenericEqualityComparer<IGravityInstructionInvoker, string>((x) => x.Type, StringComparer.OrdinalIgnoreCase));
        }
    }
}