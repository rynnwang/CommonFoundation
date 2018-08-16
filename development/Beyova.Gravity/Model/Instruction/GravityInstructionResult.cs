using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityCommandResult.
    /// </summary>
    public class GravityInstructionResult : GravityInstructionResultBase
    {
        /// <summary>
        /// Gets or sets the detail.
        /// </summary>
        /// <value>
        /// The detail.
        /// </value>
        public JToken Detail { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public ExceptionInfo Exception { get; set; }
    }
}