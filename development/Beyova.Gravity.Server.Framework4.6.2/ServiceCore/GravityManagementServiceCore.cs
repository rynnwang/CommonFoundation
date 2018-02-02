using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Beyova.SaasPlatform;
using Beyova.Gravity.DataAccessController;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityManagementServiceCore.
    /// </summary>
    public class GravityManagementServiceCore : IProductManagementService<ProductInfo, ProductCriteria>
    {
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
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
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
        /// Requests the command.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? RequestCommand(GravityCommandRequest request)
        {
            try
            {
                request.CheckNullObject(nameof(request));

                using (var controller = new GravityCommandRequestAccessController())
                {
                    return controller.RequestCommand(request);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(request);
            }
        }

        ///// <summary>
        ///// Commits the command result.
        ///// </summary>
        ///// <param name="commandResult">The command result.</param>
        ///// <returns>System.Nullable&lt;Guid&gt;.</returns>
        //public Guid? CommitCommandResult(GravityCommandResult commandResult)
        //{
        //    try
        //    {
        //        commandResult.CheckNullObject(nameof(commandResult));

        //        using (var controller = new GravityCommandResultAccessController())
        //        {
        //            return controller.CommitCommandResult(commandResult);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.Handle(commandResult);
        //    }
        //}

        #endregion

        #region Product Info

        /// <summary>
        /// Creates the or update product.
        /// </summary>
        /// <param name="productInfo">The product information.</param>
        /// <returns>System.Nullable&lt;Guid&gt;.</returns>
        public Guid? CreateOrUpdateProduct(ProductInfo productInfo)
        {
            try
            {
                productInfo.CheckNullObject(nameof(productInfo));

                if (!productInfo.Key.HasValue)
                {
                    productInfo.Name.CheckEmptyString(nameof(productInfo.Name));
                    RebuildProductSecurityInfo(productInfo);
                }

                using (var controller = new ProductInfoAccessController())
                {
                    return controller.CreateOrUpdateProduct(productInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(productInfo);
            }
        }

        /// <summary>
        /// Queries the product information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// List&lt;ProductInfo&gt;.
        /// </returns>
        public List<ProductInfo> QueryProductInfo(ProductCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new ProductInfoAccessController())
                {
                    return controller.QueryProductInfo(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        /// <summary>
        /// Deletes the product information.
        /// </summary>
        /// <param name="key">The key.</param>
        public void DeleteProductInfo(Guid? key)
        {
            try
            {
                key.CheckNullObject(nameof(key));

                using (var controller = new ProductInfoAccessController())
                {
                    controller.DeleteProductInfo(key);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Renews the product RSA keys.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RenewProductSecurityInfo(Guid? key)
        {
            try
            {
                key.CheckNullObject(nameof(key));

                using (var controller = new ProductInfoAccessController())
                {
                    var product = controller.QueryProductInfo(new ProductCriteria { Key = key }).FirstOrDefault();

                    if (product == null)
                    {
                        ExceptionFactory.CreateInvalidObjectException(nameof(key), key);
                    }

                    RebuildProductSecurityInfo(product);
                    controller.CreateOrUpdateProduct(product);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Fills the new RSA keys.
        /// </summary>
        /// <param name="product">The product.</param>
        private void RebuildProductSecurityInfo(ProductInfo product)
        {
            if (product != null)
            {
                var keys = EncodingOrSecurityExtension.CreateRsaKeys(2048);
                product.PublicKey = keys.PublicKey;
                product.PrivateKey = keys.PrivateKey;
                product.Token = this.CreateRandomString(64);
            }
        }

        #endregion

        #region Product Client

        /// <summary>
        /// Queries the product client.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;ProductClient&gt;.</returns>
        public List<ProductClient> QueryProductClient(ProductClientCriteria criteria)
        {
            try
            {
                criteria.CheckNullObject(nameof(criteria));

                using (var controller = new ProductClientAccessController())
                {
                    return controller.QueryProductClient(criteria);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(criteria);
            }
        }

        #endregion

        #region Operation

        /// <summary>
        /// Generates the entry file bytes.
        /// </summary>
        /// <param name="productKey">The product key.</param>
        /// <param name="gravityServiceUri">The gravity service URI.</param>
        /// <param name="configurationName">Name of the configuration.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] GenerateEntryFileBytes(Guid? productKey, Uri gravityServiceUri, string configurationName = null)
        {
            try
            {
                var productInfo = this.QueryProductInfo(new ProductCriteria { Key = productKey }).FirstOrDefault();

                productInfo.CheckNullObject(nameof(productInfo));
                configurationName = configurationName.SafeToString(this.QueryRemoteConfigurationObject(new RemoteConfigurationCriteria { OwnerKey = productKey }).FirstOrDefault()?.Name);

                var entryObject = new GravityEntryFile
                {
                    ConfigurationName = configurationName,
                    GravityServiceUri = gravityServiceUri,
                    PublicKey = productInfo.PublicKey,
                    MemberIdentifiableKey = productInfo.Token,
                    IssuedStamp = productInfo.CreatedStamp.Value,
                    IssuedTo = productInfo.Name
                };

                return Encoding.UTF8.GetBytes(entryObject.ToJson()).EncryptR3DES();
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { productKey, configurationName, gravityServiceUri });
            }
        }

        #endregion
    }
}
