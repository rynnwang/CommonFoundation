using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class AdminUserInfo
    /// </summary>
    public class AdminUserInfo : BaseCredential, IUserEssential, IIdentifier, ICredential, IProductIdentifier, IThirdPartyIdentifier
    {
        /// <summary>
        /// Gets or sets the name of the login.
        /// </summary>
        /// <value>The name of the login.</value>
        public string LoginName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the cellphone.
        /// </summary>
        /// <value>
        /// The cellphone.
        /// </value>
        public string Cellphone { get; set; }

        /// <summary>
        /// Gets or sets the third party identifier.
        /// </summary>
        /// <value>The third party identifier.</value>
        public string ThirdPartyId { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public List<string> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the avatar key.
        /// </summary>
        /// <value>The avatar key.</value>
        public Guid? AvatarKey { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>
        /// The culture code.
        /// </value>
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public Guid? ProductKey { get; set; }

        /// <summary>
        /// Gets or sets the nation code.
        /// </summary>
        /// <value>
        /// The nation code.
        /// </value>
        public string NationCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the time zone standard.
        /// </summary>
        /// <value>
        /// The name of the time zone standard.
        /// </value>
        public string TimeZoneStandardName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminUserInfo"/> class.
        /// </summary>
        public AdminUserInfo() { this.Permissions = new List<string>(); }
    }
}