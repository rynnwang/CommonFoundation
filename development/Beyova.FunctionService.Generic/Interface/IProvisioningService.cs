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
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Configuration, HttpConstants.HttpMethod.Put)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.RemoteConfigurationAdministrator)]
        Guid? CreateOrUpdateConfiguration(RemoteConfigurationObject configuration);

        /// <summary>
        /// Queries the configuration.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Configuration, HttpConstants.HttpMethod.Post)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.RemoteConfigurationAdministrator)]
        List<RemoteConfigurationObject> QueryConfiguration(RemoteConfigurationCriteria criteria);

        /// <summary>
        /// Queries the configuration snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.Configuration, HttpConstants.HttpMethod.Post, GenericFunctionalServiceConstants.ActionName.Snapshot)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.RemoteConfigurationAdministrator)]
        List<RemoteConfigurationObject> QueryConfigurationSnapshot(RemoteConfigurationCriteria criteria);

        #endregion Service Configuration management

        #region App Provisioning

        /// <summary>
        /// Queries the application provisioning.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Post)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppProvisioningAdministration)]
        List<TAppProvisioning> QueryAppProvisioning(AppProvisioningCriteria criteria);

        /// <summary>
        /// Queries the application provisioning snapshot.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Post, GenericFunctionalServiceConstants.ResourceName.Snapshot)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppProvisioningAdministration)]
        List<TAppProvisioning> QueryAppProvisioningSnapshot(AppProvisioningCriteria criteria);

        /// <summary>
        /// Creates the or update application provisioning.
        /// </summary>
        /// <param name="appProvisioning">The application provisioning.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Put)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppProvisioningAdministration)]
        Guid? CreateOrUpdateAppProvisioning(TAppProvisioning appProvisioning);

        /// <summary>
        /// Deletes the application provisioning.
        /// </summary>
        /// <param name="key">The key.</param>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Delete)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppProvisioningAdministration)]
        void DeleteAppProvisioning(Guid? key);

        /// <summary>
        /// Gets the application provisioning.
        /// </summary>
        /// <param name="platformKey">The platform key.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppProvisioning, HttpConstants.HttpMethod.Get)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppProvisioningAdministration)]
        [TokenRequired(false)]
        TAppProvisioning GetAppProvisioning(Guid? platformKey, string name = null);

        #endregion App Provisioning
    }
}