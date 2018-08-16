using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface IUserInfo
    /// </summary>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    public class UserInfo<TFunctionalRole> : IUserInfo<TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
    {
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        /// <value>The permissions.</value>
        public List<string> Permissions { get; set; }

        /// <summary>
        /// Gets or sets the functional role.
        /// </summary>
        /// <value>The functional role.</value>
        public TFunctionalRole FunctionalRole { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public Gender Gender { get; set; }

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
    }
}