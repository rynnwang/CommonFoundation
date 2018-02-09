using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.Api.RestApi;

namespace Beyova.FunctionService.Generic
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TAppPlatform">The type of the application platform.</typeparam>
    [TokenRequired]
    public interface IAppPlatformService<TAppPlatform>
        where TAppPlatform : AppPlatform
    {
        /// <summary>
        /// Creates the application platform.
        /// </summary>
        /// <param name="appPlatform">The application platform.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.AppPlatform, HttpConstants.HttpMethod.Put)]
        [ApiPermission(ApiPermissions.ProvisioningSystem.AppPlatformAdministration)]
        Guid? CreateAppPlatform(TAppPlatform appPlatform);

        /// <summary>
        /// Gets the application platforms.
        /// </summary>
        /// <param name="bundleId">The bundle identifier.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.AppPlatform, HttpConstants.HttpMethod.Get)]
        [ApiPermission(ApiPermissions.ProvisioningSystem.AppPlatformAdministration)]
        List<BaseObject<TAppPlatform>> GetAppPlatforms(string bundleId = null);
    }
}