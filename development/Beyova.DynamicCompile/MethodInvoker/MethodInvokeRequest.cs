using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MethodInvokeRequest
    {
        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public List<MethodInvokeParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string MethodName { get; set; }
    }
}
