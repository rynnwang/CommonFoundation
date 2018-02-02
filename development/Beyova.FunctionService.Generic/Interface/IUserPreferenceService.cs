using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.Api.RestApi;

namespace Beyova.FunctionService.Generic
{
    /// <summary>
    /// Interface IUserPreferenceService
    /// </summary>
    [ApiContract("v1", GenericFunctionalServiceConstants.ServiceName.UserPreferenceService)]
    [TokenRequired(true)]
    public interface IUserPreferenceService : IUserPreferenceService<UserPreference, UserPreferenceCriteria>
    {
    }

    /// <summary>
    /// Interface IUserPreferenceService
    /// </summary>
    /// <typeparam name="TUserPreference">The type of the t user preference.</typeparam>
    /// <typeparam name="TUserPreferenceCriteria">The type of the t user preference criteria.</typeparam>
    [TokenRequired(true)]
    public interface IUserPreferenceService<TUserPreference, TUserPreferenceCriteria>
           where TUserPreference : UserPreference
           where TUserPreferenceCriteria : UserPreferenceCriteria
    {
        /// <summary>
        /// Creates or updates default preference.
        /// </summary>
        /// <param name="preference">The preference.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Preference, HttpConstants.HttpMethod.Put)]
        [TokenRequired(true)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.PreferenceAdministrator)]
        Guid? CreateOrUpdateDefaultPreference(TUserPreference preference);

        /// <summary>
        /// Queries the default preference.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TUserPreference&gt;.</returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Preference, HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.PreferenceAdministrator)]
        List<TUserPreference> QueryDefaultPreference(TUserPreferenceCriteria criteria);

        /// <summary>
        /// Creates the or update user preference.
        /// </summary>
        /// <param name="preference">The preference.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.UserPreference, HttpConstants.HttpMethod.Put)]
        [TokenRequired(true)]
        Guid? CreateOrUpdateUserPreference(TUserPreference preference);

        /// <summary>
        /// Queries the default preference.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TUserPreference&gt;.</returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.UserPreference, HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        List<TUserPreference> QueryUserPreference(TUserPreferenceCriteria criteria);
    }
}