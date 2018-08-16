using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Interface IGravityInstructionInvoker
    /// </summary>
    public interface IGravityInstructionInvoker
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        string Type { get; }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        JToken Invoke(string action, JToken parameters);
    }
}