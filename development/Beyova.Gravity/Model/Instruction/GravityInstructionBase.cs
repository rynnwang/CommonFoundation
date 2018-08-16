using System;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityInstructionBase.
    /// </summary>
    public class GravityInstructionBase : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>The action.</value>
        public string Action { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the gravity member key.
        /// </summary>
        /// <value>
        /// The gravity member key.
        /// </value>
        public Guid? GravityMemberKey { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public JToken Parameters { get; set; }
    }
}