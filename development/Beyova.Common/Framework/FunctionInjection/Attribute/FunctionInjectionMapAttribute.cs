using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class FunctionInjectionMapAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the target.
        /// </summary>
        /// <value>
        /// The name of the target.
        /// </value>
        public string TargetName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [expect as low priority].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [expect as low priority]; otherwise, <c>false</c>.
        /// </value>
        public bool ExpectAsLowPriority { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionInjectionMapAttribute" /> class.
        /// </summary>
        /// <param name="targetName">Name of the target.</param>
        /// <param name="expectAsLowPriority">if set to <c>true</c> [expect as low priority].</param>
        public FunctionInjectionMapAttribute(string targetName, bool expectAsLowPriority = false)
        {
            TargetName = targetName;
            ExpectAsLowPriority = expectAsLowPriority;
        }
    }
}