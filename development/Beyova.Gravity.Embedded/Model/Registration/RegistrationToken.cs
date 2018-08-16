using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class RegistrationToken.
    /// </summary>
    public class RegistrationToken : IExpirable
    {
        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>
        /// The expired stamp.
        /// </value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public GravityOptions Options { get; set; }
    }
}