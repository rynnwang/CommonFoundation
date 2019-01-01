using System;

namespace Beyova
{
    /// <summary>
    /// Interface IUserEssential
    /// </summary>
    public interface IUserEssential : IIdentifier, ICredential, IPermissionIdentifiers
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the cellphone.
        /// </summary>
        /// <value>
        /// The cellphone.
        /// </value>
        CellphoneNumber Cellphone { get; set; }

        /// <summary>
        /// Gets or sets the avatar key.
        /// </summary>
        /// <value>The avatar key.</value>
        Guid? AvatarKey { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>
        /// The culture code.
        /// </value>
        string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the time zone standard.
        /// </summary>
        /// <value>
        /// The name of the time zone standard.
        /// </value>
        string TimeZoneStandardName { get; set; }
    }
}