using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.Api.RestApi;

namespace Beyova.FunctionService.Generic
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TAppVersion">The type of the application version.</typeparam>
    [TokenRequired]
    public interface IAppVersionService<TAppVersion>
        where TAppVersion : AppVersion
    {
        /// <summary>
        /// Gets the application version.
        /// </summary>
        /// <param name="platformKey">The platform key.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppVersion, HttpConstants.HttpMethod.Get)]
        [TokenRequired(false)]
        TAppVersion GetAppVersion(Guid? platformKey);

        /// <summary>
        /// Gets the application version snapshot.
        /// </summary>
        /// <param name="snapshotKey">The snapshot key.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppVersion, HttpConstants.HttpMethod.Get, GenericFunctionalServiceConstants.ResourceName.Snapshot)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppVersionAdministration)]
        TAppVersion GetAppVersionSnapshot(Guid? snapshotKey);

        /// <summary>
        /// Queries the application version.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppVersion, HttpConstants.HttpMethod.Post)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppVersionAdministration)]
        List<TAppVersion> QueryAppVersion(AppVersionCriteria criteria);

        /// <summary>
        /// Queries the application version snapshots.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppVersion, HttpConstants.HttpMethod.Post, GenericFunctionalServiceConstants.ResourceName.Snapshot)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppVersionAdministration)]
        List<TAppVersion> QueryAppVersionSnapshots(AppVersionCriteria criteria);

        /// <summary>
        /// Creates the or update application version.
        /// </summary>
        /// <param name="appVersion">The application version.</param>
        /// <returns></returns>
        [ApiOperation(GenericFunctionalServiceConstants.ResourceName.AppVersion, HttpConstants.HttpMethod.Put)]
        [ApiPermission(GenericFunctionalServiceConstants.Permission.ProvisioningSystem.AppVersionAdministration)]
        TAppVersion CreateOrUpdateAppVersion(AppVersionBase appVersion);
    }
}