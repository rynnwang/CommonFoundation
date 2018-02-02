using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Interface ISimpleUserCriteria
    /// </summary>
    public interface ISimpleUserCriteria : IIdentifier
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the country iso2 code.
        /// </summary>
        /// <value>
        /// The country iso2 code.
        /// </value>
        string CountryIso2Code { get; set; }

        /// <summary>
        /// Gets or sets the cellphone.
        /// </summary>
        /// <value>
        /// The cellphone.
        /// </value>
        string Cellphone { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }
    }
}