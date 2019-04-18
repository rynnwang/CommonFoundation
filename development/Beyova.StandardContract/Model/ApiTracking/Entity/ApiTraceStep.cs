using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Class ApiTraceStep.
    /// </summary>
    public class ApiTraceStep
    {
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>The parent.</value>
        [JsonIgnore]
        public ApiTraceStep Parent { get; set; }

        /// <summary>
        /// Gets or sets the full name of the method.
        /// </summary>
        /// <value>The full name of the method.</value>
        [JsonProperty(PropertyName = "methodFullName")]
        public string MethodFullName { get; set; }

        /// <summary>
        /// Gets or sets the entry stamp.
        /// </summary>
        /// <value>The entry stamp.</value>
        [JsonProperty(PropertyName = "entryStamp")]
        public DateTime? EntryStamp { get; set; }

        /// <summary>
        /// Gets or sets the exit stamp.
        /// </summary>
        /// <value>The exit stamp.</value>
        [JsonProperty(PropertyName = "exitStamp")]
        public DateTime? ExitStamp { get; set; }

        /// <summary>
        /// Gets or sets the exception key.
        /// </summary>
        /// <value>The exception key.</value>
        [JsonProperty(PropertyName = "exceptionKey")]
        public Guid? ExceptionKey { get; set; }

        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>The detail.</value>
        public List<ApiTraceStep> InnerTraces { get; set; }

        /// <summary>
        /// Gets or sets the debug information.
        /// </summary>
        /// <value>
        /// The debug information.
        /// </value>
        public DebugInfo DebugInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceStep"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="methodFullName">Full name of the method.</param>
        /// <param name="entryStamp">The entry stamp.</param>
        internal ApiTraceStep(ApiTraceStep parent, string methodFullName = null, DateTime? entryStamp = null)
            : this()
        {
            Parent = parent;
            MethodFullName = methodFullName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTraceLog"/> class.
        /// </summary>
        public ApiTraceStep()
        {
            InnerTraces = new List<ApiTraceStep>();
            DebugInfo = new Diagnostic.DebugInfo();
        }
    }
}