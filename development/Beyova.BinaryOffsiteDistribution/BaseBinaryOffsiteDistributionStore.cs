using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Beyova;

namespace Beyova
{
    /// <summary>
    /// Class BaseBinaryOffsiteDistributionStore
    /// </summary>
    public abstract class BaseBinaryOffsiteDistributionStore<TStorageOperator>
        where TStorageOperator : class, ICloudBinaryStorageOperator
    {
        #region Inline class

        /// <summary>
        /// Class RuntimeBinaryOffsiteDistributionContextObject.
        /// </summary>
        protected class RuntimeBinaryOffsiteDistributionContextObject
        {
            /// <summary>
            /// Gets or sets the storage identifier.
            /// </summary>
            /// <value>The storage identifier.</value>
            public BinaryStorageIdentifier StorageIdentifier
            {
                get; set;
            }

            /// <summary>
            /// Gets or sets the host region.
            /// </summary>
            /// <value>The host region.</value>
            public string HostRegion { get; set; }

            /// <summary>
            /// Gets or sets the storage operator.
            /// </summary>
            /// <value>The storage operator.</value>
            public TStorageOperator StorageOperator { get; set; }

            /// <summary>
            /// Gets or sets the data stream.
            /// </summary>
            /// <value>The data stream.</value>
            public Stream DataStream { get; set; }

            /// <summary>
            /// Gets or sets the type of the content.
            /// </summary>
            /// <value>The type of the content.</value>
            public string ContentType { get; set; }

            /// <summary>
            /// Gets or sets the hash.
            /// </summary>
            /// <value>The hash.</value>
            public string Hash { get; set; }

            /// <summary>
            /// Gets or sets the customized object.
            /// </summary>
            /// <value>The customized object.</value>
            public Dictionary<string, object> CustomizedObject { get; set; }
        }

        #endregion Inline class

        /// <summary>
        /// Gets or sets the cloud binary operators.
        /// </summary>
        /// <value>The cloud binary operators.</value>
        public Dictionary<string, TStorageOperator> CloudBinaryOperators { get; protected set; }

        /// <summary>
        /// The _SQL connection
        /// </summary>
        protected string _sqlConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBinaryOffsiteDistributionStore{TStorageOperator}" /> class.
        /// </summary>
        /// <param name="sqlConnection">The SQL connection.</param>
        /// <param name="binaryOperators">The binary operators.</param>
        public BaseBinaryOffsiteDistributionStore(string sqlConnection, IDictionary<string, TStorageOperator> binaryOperators)
        {
            this._sqlConnection = sqlConnection;
            CloudBinaryOperators = new Dictionary<string, TStorageOperator>(binaryOperators, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Dispatches the offsite distribution.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="hostRegionFilters">The host region filters.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="customizedObject">The customized object.</param>
        public void DispatchOffsiteDistribution(BinaryStorageIdentifier storageIdentifier, byte[] data, string[] hostRegionFilters, string contentType = null, string hash = null, Dictionary<string, object> customizedObject = null)
        {
            try
            {
                storageIdentifier.CheckNullObject(nameof(storageIdentifier));
                data.CheckNullObject(nameof(data));
                storageIdentifier.Identifier.CheckNullObject("storageIdentifier.Identifier");
                storageIdentifier.Container.CheckNullObject("storageIdentifier.Container");

                foreach (var one in this.GetHostRegions(hostRegionFilters))
                {
                    TStorageOperator storageOperator;
                    if (CloudBinaryOperators.TryGetValue(one, out storageOperator))
                    {
                        var stream = data.ToStream();
                        RunDispatch(
                            new RuntimeBinaryOffsiteDistributionContextObject
                            {
                                DataStream = stream,
                                CustomizedObject = customizedObject,
                                HostRegion = one,
                                StorageIdentifier = storageIdentifier,
                                StorageOperator = storageOperator,
                                ContentType = contentType,
                                Hash = hash
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { storageIdentifier, hostRegionFilters, contentType, hash });
            }
        }

        /// <summary>
        /// Confirms the dispatch offsite distribution.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        /// <param name="dataStream">The data stream.</param>
        /// <param name="hostRegion">The host region.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="customizedObject">The customized object.</param>
        public void RetryDispatchOffsiteDistribution(BinaryStorageIdentifier storageIdentifier, Stream dataStream, string hostRegion, string contentType = null, string hash = null, Dictionary<string, object> customizedObject = null)
        {
            try
            {
                hostRegion.CheckEmptyString(nameof(hostRegion));
                storageIdentifier.CheckNullObject(nameof(storageIdentifier));
                dataStream.CheckNullObject(nameof(dataStream));
                storageIdentifier.Identifier.CheckNullObject("storageIdentifier.Identifier");
                storageIdentifier.Container.CheckNullObject("storageIdentifier.Container");

                TStorageOperator storageOperator;
                if (CloudBinaryOperators.TryGetValue(hostRegion, out storageOperator))
                {
                    RunDispatch(
                        new RuntimeBinaryOffsiteDistributionContextObject
                        {
                            DataStream = dataStream,
                            CustomizedObject = customizedObject,
                            HostRegion = hostRegion,
                            StorageIdentifier = storageIdentifier,
                            StorageOperator = storageOperator,
                            ContentType = contentType,
                            Hash = hash
                        });
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { storageIdentifier, hostRegion, contentType, hash });
            }
        }

        /// <summary>
        /// Gets the binary storage download credential.
        /// </summary>
        /// <param name="hostRegion">The host region.</param>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public BinaryStorageActionCredential GetBinaryStorageDownloadCredential(string hostRegion, string containerName, string blobIdentifier, int expireOffsetInMinute)
        {
            try
            {
                hostRegion.CheckEmptyString(nameof(hostRegion));
                containerName.CheckEmptyString(nameof(containerName));
                blobIdentifier.CheckEmptyString(nameof(blobIdentifier));

                TStorageOperator storageOperator;
                return (this.CloudBinaryOperators.TryGetValue(hostRegion, out storageOperator)) ? storageOperator.CreateBlobDownloadCredential(containerName, blobIdentifier, expireOffsetInMinute) : null;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { hostRegion, containerName, blobIdentifier, expireOffsetInMinute });
            }
        }

        /// <summary>
        /// Gets the binary offsite distribution by identifier batch.
        /// </summary>
        /// <param name="identifiers">The identifiers.</param>
        /// <param name="container">The container.</param>
        /// <returns>System.Collections.Generic.List&lt;Beyova.BinaryOffsiteDistribution&gt;.</returns>
        public List<BinaryOffsiteDistribution> GetBinaryOffsiteDistributionByIdentifierBatch(List<Guid> identifiers, string container = null)
        {
            try
            {
                identifiers.CheckNullObject(nameof(identifiers));

                using (var controller = new BinaryOffsiteDistributionAccessController(this._sqlConnection))
                {
                    return controller.GetBinaryOffsiteDistributionByIdentifiers(identifiers, container);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new
                {
                    identifiers,
                    container
                });
            }
        }

        /// <summary>
        /// Gets the binary offsite distribution by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        public BinaryOffsiteDistribution GetBinaryOffsiteDistributionByKey(Guid? key)
        {
            try
            {
                key.CheckNullObject(nameof(key));

                using (var controller = new BinaryOffsiteDistributionAccessController(this._sqlConnection))
                {
                    return controller.GetBinaryOffsiteDistributionByKey(key);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(key);
            }
        }

        /// <summary>
        /// Deletes the binary offsite distribution.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        public void DeleteBinaryOffsiteDistribution(BinaryStorageIdentifier storageIdentifier)
        {
            try
            {
                storageIdentifier.CheckNullObject(nameof(storageIdentifier));
                storageIdentifier.Identifier.CheckEmptyString(nameof(storageIdentifier.Identifier));

                using (var controller = new BinaryOffsiteDistributionAccessController(this._sqlConnection))
                {
                    controller.DeleteBinaryOffsiteDistribution(storageIdentifier);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(storageIdentifier);
            }
        }

        /// <summary>
        /// Gets the binary offsite distribution by country.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        /// <param name="country">The country.</param>
        /// <returns>BinaryOffsiteDistribution.</returns>
        public BinaryOffsiteDistribution GetBinaryOffsiteDistributionByCountry(BinaryStorageIdentifier storageIdentifier, string country)
        {
            try
            {
                storageIdentifier.CheckNullObject(nameof(storageIdentifier));
                storageIdentifier.Identifier.CheckEmptyString(nameof(storageIdentifier.Identifier));
                country.CheckEmptyString(nameof(country));

                using (var controller = new BinaryOffsiteDistributionAccessController(this._sqlConnection))
                {
                    return controller.GetBinaryOffsiteDistributionByCountry(storageIdentifier, country);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { country, storageIdentifier });
            }
        }

        #region Internal

        /// <summary>
        /// Gets the host regions.
        /// </summary>
        /// <param name="hostRegionFilters">The host region filters.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        protected IEnumerable<string> GetHostRegions(string[] hostRegionFilters)
        {
            return hostRegionFilters.HasItem() ? CloudBinaryOperators.Keys.Where(x => hostRegionFilters.Contains(x, true)) : CloudBinaryOperators.Keys;
        }

        /// <summary>
        /// Runs the dispatch.
        /// </summary>
        /// <param name="contextObject">The context object.</param>
        protected void RunDispatch(RuntimeBinaryOffsiteDistributionContextObject contextObject)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(RunDispatch), contextObject);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { storageIdentifier = contextObject?.StorageIdentifier, hostRegion = contextObject?.HostRegion });
            }
        }

        /// <summary>
        /// Runs the dispatch.
        /// </summary>
        /// <param name="inputObject">The input object.</param>
        protected void RunDispatch(object inputObject)
        {
            const int expireMinute = 60;
            Stream stream = null;

            try
            {
                var runtimeObject = inputObject as RuntimeBinaryOffsiteDistributionContextObject;

                if (runtimeObject != null)
                {
                    runtimeObject.StorageIdentifier.CheckNullObject(nameof(runtimeObject.StorageIdentifier));
                    runtimeObject.DataStream.CheckNullObject(nameof(runtimeObject.DataStream));
                    runtimeObject.StorageIdentifier.Identifier.CheckEmptyString(nameof(runtimeObject.StorageIdentifier.Identifier));
                    runtimeObject.StorageIdentifier.Container.CheckEmptyString(nameof(runtimeObject.StorageIdentifier.Container));

                    stream = runtimeObject.DataStream;

                    using (var controller = new BinaryOffsiteDistributionAccessController(_sqlConnection))
                    {
                        // Customized action.
                        if (OnInitializeBinaryOffsiteDistribution(runtimeObject))
                        {
                            return;
                        }

                        // Initialize in database
                        var origialDistribution = new BinaryOffsiteDistribution
                        {
                            Container = runtimeObject.StorageIdentifier.Container,
                            Identifier = runtimeObject.StorageIdentifier.Identifier,
                            HostRegion = runtimeObject.HostRegion
                        };
                        var distribution = controller.CreateOrUpdateBinaryOffsiteDistribution(origialDistribution, DateTime.UtcNow.AddMinutes(expireMinute));

                        if (distribution == null)
                        {
                            return;
                        }

                        // Upload
                        var uploadCredential = runtimeObject.StorageOperator.CreateBlobUploadCredential(runtimeObject.StorageIdentifier.Container, runtimeObject.StorageIdentifier.Identifier, expireMinute, runtimeObject.Hash);
                        runtimeObject.StorageOperator.UploadBinaryStreamByCredentialUri(uploadCredential.CredentialUri, stream, runtimeObject.ContentType);

                        var meta = runtimeObject.StorageOperator.FetchCloudMeta(runtimeObject.StorageIdentifier);
                        if (meta == null || string.IsNullOrWhiteSpace(meta.Hash) || (!string.IsNullOrWhiteSpace(runtimeObject.Hash) && runtimeObject.Hash.Equals(meta.Hash, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw ExceptionFactory.CreateInvalidObjectException(nameof(meta), meta, "HashInconsistence");
                        }

                        // Customized action.
                        if (OnCommitBinaryOffsiteDistribution(runtimeObject))
                        {
                            return;
                        }

                        // Commit.
                        distribution = controller.CommitBinaryOffsiteDistribution(distribution);

                        // Customized action
                        OnBinaryOffsiteDistributionCompleted(distribution, runtimeObject);
                    }
                }
            }
            catch (Exception ex)
            {
                Framework.ApiTracking.LogException(ex.Handle().ToExceptionInfo());
            }
            finally
            {
                stream.SafeDispose();
            }
        }

        /// <summary>
        /// Called when [initialize binary offsite distribution].
        /// </summary>
        /// <param name="contectObject">The contect object.</param>
        /// <returns>
        ///   <c>true</c> if needs to interrupt, <c>false</c> otherwise.</returns>
        protected virtual bool OnInitializeBinaryOffsiteDistribution(RuntimeBinaryOffsiteDistributionContextObject contectObject)
        {
            return false;
        }

        /// <summary>
        /// Called when [commit binary offsite distribution].
        /// </summary>
        /// <param name="contectObject">The contect object.</param>
        /// <returns>
        ///   <c>true</c> if needs to interrupt, <c>false</c> otherwise.</returns>
        protected virtual bool OnCommitBinaryOffsiteDistribution(RuntimeBinaryOffsiteDistributionContextObject contectObject)
        {
            return false;
        }

        /// <summary>
        /// Called when [binary offsite distribution completed].
        /// </summary>
        /// <param name="distribution">The distribution.</param>
        /// <param name="contectObject">The contect object.</param>
        protected virtual void OnBinaryOffsiteDistributionCompleted(BinaryOffsiteDistribution distribution, RuntimeBinaryOffsiteDistributionContextObject contectObject)
        {
        }

        #endregion Internal
    }
}