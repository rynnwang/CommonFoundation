using System;
using System.Collections.Generic;
using System.Text;
using Beyova.ExceptionSystem;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// 
    /// </summary>
    public class MethodInvokeResult
    {
        /// <summary>
        /// Gets or sets the return object.
        /// </summary>
        /// <value>
        /// The return object.
        /// </value>
        public object ReturnObject { get; set; }

        /// <summary>
        /// Gets or sets the out objects.
        /// </summary>
        /// <value>
        /// The out objects.
        /// </value>
        public Dictionary<string, object> OutObjects { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; set; }
    }
}
