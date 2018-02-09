using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.Api.RestApi;

namespace Beyova.FunctionService.Generic
{
    /// <summary>
    /// Interface IProvisioningService
    /// </summary>
    [TokenRequired]
    public interface IProvisioningService<TAppProvisioning>
         where TAppProvisioning : AppProvisioningBase
    {
        #region Service Configuration management

        /// <summary>
        /// Creates the or update configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        [ApiOperation(ApiResourceNames.Configuration, HttpConstants.HttpMethod.Put)]
        [ApiPermission(ApiPermissions.RemoteConfigurationAdministrator)]
        Guid? CreateOrUpdateConfiguration(RemoteConfigurationObject configuration);

        /// <summary>
        /// Queries the configuration.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation(ApiResourceNames.Configuration, HttpConstants.HttpMethod.Post)]
        [ApiPermission(ApiPermissions.RemoteConfigurationAdministrator)]
        List<RemoteConfigurationObject> QueryConfiguration(RemoteConfigurationCriteria criteria);

        /// <summary>
        /// Queries the configuration snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation(ApiResourceNames.Configuration, HttpConstants.HttpMethod.Post, GenericFunctionalServiceConstants.ActionName.Snapshot)]
        [ApiPermission(ApiPermissions.RemoteConfigurationAdministrator)]
        List<RemoteConfigurationObject> QueryConfigurationSnapshot(RemoteConfigurationCriteria criteria);

        #endregion Service Configuration management

        #region App Provisioning

        /// <summary>
        /// Queries the application provisioning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.AppProvisioning, HttpConstants.HttpMethod.Post)]
        [ApiPermission(ApiPermissions.ProvisioningSystem.AppProvisioningAdministration)]
        List<TAppProvisioning> QueryAppProvisioning(AppProvisioningCriteria criteria);

        /// <summary>
        /// Queries the application provisioning snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.AppProvisioning, HttpConstants.HttpMethod.Post, GenericFunctionalServiceConstants.ActionName.Snapshot)]
        [ApiPermission(ApiPermissions.ProvisioningSystem.AppProvisioningAdministration)]
        List<TAppProvisioning> QueryAppProvisioningSnapshot(AppProvisioningCriteria criteria);

        /// <summary>
        /// Creates the or update application provisioning.
        /// </summary>
        /// <param name="appProvisioning">The application provisioning.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.AppProvisioning, HttpConstants.HttpMethod.Put)]
        [ApiPermission(ApiPermissions.ProvisioningSystem.AppProvisioningAdministration)]
        Guid? CreateOrUpdateAppProvisioning(TAppProvisioning appProvisioning);

        /// <summary>
        /// Deletes the application provisioning.
        /// </summary>
        /// <param name="key">The key.</param>
        [ApiOperation(ApiResourceNames.AppProvisioning, HttpConstants.HttpMethod.Delete)]
        [ApiPermission(ApiPermissions.ProvisioningSystem.AppProvisioningAdministration)]
        void DeleteAppProvisioning(Guid? key);

        /// <summary>
        /// Gets the application provisioning.
        /// </summary>
        /// <param name="platformKey">The platform key.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.AppProvisioning, HttpConstants.HttpMethod.Get)]
        [ApiPermission(ApiPermissions.ProvisioningSystem.AppProvisioningAdministration)]
        [TokenRequired(false)]
        TAppProvisioning GetAppProvisioning(Guid? platformKey, string name = null);

        #endregion App Provisioning
    }
}