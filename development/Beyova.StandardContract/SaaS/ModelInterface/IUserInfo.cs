using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Interface IUserInfo
    /// </summary>
    public interface IUserInfo<TFunctionalRole> : IIdentifier, ICredential, ISimpleUserInfo
        where TFunctionalRole : struct, IConvertible
    {
        /// <summary>
        /// Gets or sets the functional role.
        /// </summary>
        /// <value>The functional role.</value>
        TFunctionalRole FunctionalRole { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        Gender Gender { get; set; }
    }
}