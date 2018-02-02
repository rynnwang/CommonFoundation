using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Interface IUserCriteria
    /// </summary>
    public interface IUserCriteria<TFunctionalRole> : ISimpleUserCriteria
          where TFunctionalRole : struct, IConvertible
    {
        /// <summary>
        /// Gets or sets the functional role.
        /// </summary>
        /// <value>The functional role.</value>
        TFunctionalRole? FunctionalRole { get; set; }
    }
}