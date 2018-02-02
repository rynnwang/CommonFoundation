using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Interface ISimpleUserInfo
    /// </summary>
    public interface ISimpleUserInfo : IIdentifier, ICredential
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
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
        /// Gets or sets the avatar key.
        /// </summary>
        /// <value>The avatar key.</value>
        Guid? AvatarKey { get; set; }

        /// <summary>
        /// Gets or sets the avatar URL.
        /// </summary>
        /// <value>The avatar URL.</value>
        string AvatarUrl { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        string Language { get; set; }

        /// <summary>
        /// Gets or sets the time zone. Unit: minute
        /// </summary>
        /// <value>The time zone.</value>
        int? TimeZone { get; set; }
    }
}