using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DynamicStaticMethodOptions
    {
        /// <summary>
        /// Gets or sets the type of the return object.
        /// </summary>
        /// <value>
        /// The type of the return object.
        /// </value>
        public Type ReturnObjectType { get; set; }

        /// <summary>
        /// Gets or sets the dynamic code.
        /// </summary>
        /// <value>
        /// The dynamic code.
        /// </value>
        public string DynamicCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName { get; set; }
    }
}
