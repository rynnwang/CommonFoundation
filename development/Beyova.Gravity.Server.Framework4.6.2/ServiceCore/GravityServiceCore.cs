using System;
using System.Collections.Generic;
using System.Linq;
using Beyova.Gravity.DataAccessController;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityServiceCore.
    /// </summary>
    public class GravityServiceCore
    {
        /// <summary>
        /// Gets the product information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>ProductInfo.</returns>
        public ProductInfo GetProductInfoByToken(string token)
        {
            try
            {
                token.CheckEmptyString(nameof(token));

                using (var controller = new ProductInfoAccessController())
                {
                    return controller.GetProductInfoByToken(token);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(token);
            }
        }

        #region RemoteConfigurationObject

        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <param name="productKey">The product key.</param>
        /// <param name="name">The name.</param>
        /// <returns>RemoteConfigurationObject.</returns>
        public RemoteConfigurationObject RetrieveConfiguration(Guid? productKey, string name)
        {
            try
            {
                using (var controller = new RemoteConfigurationObjectAccessController())
                {
                    return controller.QueryRemoteConfigurationObject(new RemoteConfigurationCriteria
                    {
                        Name = name,
                        OwnerKey = productKey
                    }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { productKey, name });
            }
        }

        /// <summary>
        /// Queries the remote configuration object.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;RemoteConfigurationObject&gt;.</returns>
        public List<RemoteConfigurationObject> QueryRemoteConfigurationObject(RemoteConfigurationCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new RemoteConfigurationObjectAccessController())
                {
                    return controller.QueryRemoteConfigurationObject(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Creates the or update remote configuration object.
        /// </summary>
        /// <param name="configurationObject">The configuration object.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateRemoteConfigurationObject(RemoteConfigurationObject configurationObject)
        {
            try
            {
                configurationObject.CheckNullObject(nameof(configurationObject));
                configurationObject.OwnerKey.CheckNullObject(nameof(configurationObject.OwnerKey));

                using (var controller = new RemoteConfigurationObjectAccessController())
                {
                    return controller.CreateOrUpdateRemoteConfigurationObject(configurationObject, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(configurationObject);
            }
        }

        /// <summary>
        /// Queries the central configuration snapshot.
        /// </summary>
        /// <param name="configurationKey">The configuration key.</param>
        /// <param name="snapshotKey">The snapshot key.</param>
        /// <returns>List&lt;RemoteConfigurationInfo&gt;.</returns>
        public List<RemoteConfigurationInfo> QueryCentralConfigurationSnapshot(Guid? configurationKey, Guid? snapshotKey)
        {
            try
            {
                using (var controller = new RemoteConfigurationInfoAccessController())
                {
                    return controller.QueryCentralConfigurationSnapshot(configurationKey, snapshotKey);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { configurationKey, snapshotKey });
            }
        }

        /// <summary>
        /// Deletes the central configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteCentralConfiguration(Guid? key)
        {
            try
            {
                key.CheckNullObject(nameof(key));

                using (var controller = new RemoteConfigurationObjectAccessController())
                {
                    controller.DeleteCentralConfiguration(key, ContextHelper.GetCurrentOperatorKey());
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        #endregion

        #region Heartbeat

        /// <summary>
        /// Saves the heartbeat information.
        /// </summary>
        /// <param name="productKey">The product key.</param>
        /// <param name="heartbeat">The heartbeat.</param>
        /// <returns>Client Key.</returns>
        public Guid? SaveHeartbeatInfo(Guid? productKey, Heartbeat heartbeat)
        {
            try
            {
                using (var controller = new HeartbeatInfoAccessController())
                {
                    return controller.SaveHeartbeatInfo(heartbeat, productKey);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { productKey, heartbeat });
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets the pending command request.
        /// </summary>
        /// <param name="clientKey">The client key.</param>
        /// <returns>List&lt;GravityCommandRequest&gt;.</returns>
        public List<GravityCommandRequest> GetPendingCommandRequest(Guid? clientKey)
        {
            try
            {
                using (var controller = new GravityCommandRequestAccessController())
                {
                    return controller.GetPendingCommandRequest(clientKey);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(clientKey);
            }
        }

        /// <summary>
        /// Commits the command result.
        /// </summary>
        /// <param name="commandResult">The command result.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CommitCommandResult(GravityCommandResult commandResult)
        {
            try
            {
                commandResult.CheckNullObject(nameof(commandResult));
                commandResult.ClientKey.CheckNullObject(nameof(commandResult.ClientKey));
                commandResult.Content.CheckNullObject(nameof(commandResult.Content));
                commandResult.RequestKey.CheckNullObject(nameof(commandResult.RequestKey));

                using (var controller = new GravityCommandResultAccessController())
                {
                    return controller.CommitCommandResult(commandResult);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(commandResult);
            }
        }

        #endregion

    }
}
