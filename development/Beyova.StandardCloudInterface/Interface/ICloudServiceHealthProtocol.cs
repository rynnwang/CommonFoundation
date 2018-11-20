using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface ICloudServiceHealthProtocol
    /// </summary>
    /// <typeparam name="TInfrastructureObject">The type of the infrastructure object.</typeparam>
    /// <typeparam name="TInfrastructureObjectHealthStatus">The type of the infrastructure object health status.</typeparam>
    public interface ICloudServiceHealthProtocol<TInfrastructureObject, TInfrastructureObjectHealthStatus>
    {
        /// <summary>
        /// Gets the infrastructure object by resource group.
        /// </summary>
        /// <param name="groupId">The group identifier.</param>
        /// <returns></returns>
        List<TInfrastructureObject> GetInfrastructureObjectByResourceGroup(string groupId);

        /// <summary>
        /// Gets the infrastructure object health status.
        /// </summary>
        /// <param name="infrastructureObjectId">The infrastructure object identifier.</param>
        /// <returns></returns>
        TInfrastructureObjectHealthStatus GetInfrastructureObjectHealthStatus(string infrastructureObjectId);
    }
}