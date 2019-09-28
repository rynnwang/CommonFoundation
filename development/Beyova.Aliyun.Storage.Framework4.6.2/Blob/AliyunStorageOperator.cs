using Aliyun.OSS;
using Beyova;
using Beyova.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace Beyova.Alibaba
{
    /// <summary>
    /// Class AliyunStorageOperator.
    /// </summary>
    public class AliyunStorageOperator : CloudBinaryStorageOperator<Bucket, OssObjectSummary>, ICloudBinaryStorageOperator
    {
        #region Fields

        /// <summary>
        /// The client
        /// </summary>
        internal OssClient client;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunStorageOperator"/> class.
        /// </summary>
        /// <param name="serviceEndpoint">The service endpoint.</param>
        public AliyunStorageOperator(ApiEndpoint serviceEndpoint)
            : this(serviceEndpoint.Host, serviceEndpoint.Account, serviceEndpoint.Token)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AliyunStorageOperator"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="accessKeyId">The access key identifier.</param>
        /// <param name="accessKeySecret">The access key secret.</param>
        public AliyunStorageOperator(string endpoint, string accessKeyId, string accessKeySecret)
        {
            client = new OssClient(endpoint, accessKeyId, accessKeySecret);
        }

        #endregion Constructor

        #region CloudBinaryStorageOperator interfaces

        /// <summary>
        /// Creates the BLOB upload credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="contentDisposition">The content disposition.</param>
        /// <returns></returns>
        public BinaryStorageActionCredential CreateBlobUploadCredential(string containerName,
           string blobIdentifier, int expireOffsetInMinute, CryptoKey hash, string contentType, string contentDisposition)
        {
            return CreateBlobCredential(containerName, blobIdentifier, expireOffsetInMinute, SignHttpMethod.Put, (req) =>
            {
                if (hash != null)
                {
                    req.ContentMd5 = hash.ToString();
                }

                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    req.ContentType = contentType;
                }

                if (!string.IsNullOrWhiteSpace(contentDisposition))
                {
                    req.AddUserMetadata(HttpConstants.HttpHeader.ContentDisposition, contentType);
                }
            });
        }

        /// <summary>
        /// Creates the BLOB upload credential.
        /// </summary>
        /// <param name="containerName">Name of the container(S3 bucketName).</param>
        /// <param name="blobIdentifier">The BLOB identifier(S3 key).</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">Hash.</param>
        /// <param name="contentType">Content type.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public override BinaryStorageActionCredential CreateBlobUploadCredential(string containerName,
            string blobIdentifier, int expireOffsetInMinute, CryptoKey hash = null, string contentType = null)
        {
            return CreateBlobUploadCredential(containerName, blobIdentifier, expireOffsetInMinute, hash, contentType, null);
        }

        /// <summary>
        /// Creates the BLOB download credential.
        /// </summary>
        /// <param name="containerName">Name of the container(S3 bucketName).</param>
        /// <param name="blobIdentifier">The BLOB identifier(S3 key).</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public override BinaryStorageActionCredential CreateBlobDownloadCredential(string containerName,
            string blobIdentifier, int expireOffsetInMinute)
        {
            return CreateBlobCredential(containerName, blobIdentifier, expireOffsetInMinute, SignHttpMethod.Get);
        }

        /// <summary>
        /// Creates the BLOB credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="method">The method.</param>
        /// <param name="putUserMeta">The put user meta.</param>
        /// <returns></returns>
        private BinaryStorageActionCredential CreateBlobCredential(string containerName,
            string blobIdentifier, int expireOffsetInMinute, SignHttpMethod method, Action<GeneratePresignedUriRequest> putUserMeta = null)
        {
            var expiredStamp = DateTime.UtcNow.AddMinutes(expireOffsetInMinute);
            GeneratePresignedUriRequest presignRequest = new GeneratePresignedUriRequest(containerName, blobIdentifier);
            presignRequest.Expiration = expiredStamp;
            presignRequest.Method = method;

            putUserMeta?.Invoke(presignRequest);

            return new BinaryStorageActionCredential
            {
                Container = containerName,
                Identifier = blobIdentifier,
                CredentialExpiredStamp = expiredStamp,
                CredentialUri = client.GeneratePresignedUri(presignRequest).ToString()
            };
        }

        /// <summary>
        /// Check Existence of the specified identifier.
        /// </summary>
        /// <param name="identifier">The storage identifier.</param>
        /// <returns><c>true</c> if existed, <c>false</c> otherwise.</returns>
        public override bool Exists(BinaryStorageIdentifier identifier)
        {
            try
            {
                identifier.CheckNullObject(nameof(identifier));

                if (!string.IsNullOrWhiteSpace(identifier.Container))
                {
                    return client.DoesObjectExist(identifier.Container, identifier.Identifier);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }

            return false;
        }

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        public override void DeleteBlob(BinaryStorageIdentifier storageIdentifier)
        {
            try
            {
                storageIdentifier.CheckNullObject(nameof(storageIdentifier));
                storageIdentifier.Container.CheckEmptyString(nameof(storageIdentifier.Container));
                storageIdentifier.Identifier.CheckEmptyString(nameof(storageIdentifier.Identifier));

                client.DeleteObject(storageIdentifier.Container, storageIdentifier.Identifier);
            }
            catch (Exception ex)
            {
                throw ex.Handle(storageIdentifier);
            }
        }

        /// <summary>
        /// Fetches the cloud meta. Returned object would only includes (md5, length, name, content type).
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>Beyova.BinaryStorageMetaData.</returns>
        public override BinaryStorageMetaData FetchCloudMeta(BinaryStorageIdentifier identifier)
        {
            try
            {
                identifier.CheckNullObject(nameof(identifier));
                identifier.Container.CheckEmptyString(nameof(identifier.Container));
                identifier.Identifier.CheckEmptyString(nameof(identifier.Identifier));

                var meta = client.GetObjectMetadata(identifier.Container, identifier.Identifier);
                var result = new BinaryStorageMetaData(identifier)
                {
                    Hash = meta.ContentMd5,
                    Length = meta.ContentLength,
                    Name = meta.ContentDisposition,
                    ContentType = meta.ContentType
                };

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(identifier);
            }
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>
        /// List&lt;BinaryStorageMetaBase&gt;.
        /// </returns>
        public override List<BinaryStorageMetaData> QueryBinaryBlobByContainer(string containerName, string contentType = null,
CryptoKey hash = null, long? length = null, int limitCount = 10)
        {
            try
            {
                containerName.CheckEmptyString(nameof(containerName));

                var objectList = client.ListObjects(new ListObjectsRequest(containerName)
                {
                    MaxKeys = limitCount
                }).ObjectSummaries;

                List<BinaryStorageMetaData> binaryStorageMetaBaseList = new List<BinaryStorageMetaData>();
                foreach (var item in objectList)
                {
                    var key = item.Key;
                    BinaryStorageIdentifier identifier = new BinaryStorageIdentifier
                    {
                        Container = containerName,
                        Identifier = key
                    };
                    var meta = FetchCloudMeta(identifier);

                    if ((string.IsNullOrWhiteSpace(contentType) || meta.ContentType.Equals(contentType, StringComparison.OrdinalIgnoreCase))
                            && (hash == null || meta.Hash == hash)
                            && (!length.HasValue || meta.Length == length))
                    {
                        binaryStorageMetaBaseList.Add(meta);
                    }

                    if (binaryStorageMetaBaseList.Count >= limitCount)
                    {
                        return binaryStorageMetaBaseList;
                    }
                }

                return binaryStorageMetaBaseList;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, limitCount });
            }
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="hash">The MD5.(no-use)</param>
        /// <param name="length">The length.(no-use)</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>
        /// IEnumerable&lt;TCloudBlobObject&gt;.
        /// </returns>
        public override IEnumerable<OssObjectSummary> QueryBlob(Bucket container, string contentType = null, CryptoKey hash = null,
            long? length = null, int limitCount = 100)
        {
            try
            {
                container.CheckNullObject(nameof(container));
                container.Name.CheckNullObject(nameof(container.Name));

                return QueryBlob(container.Name, limitCount);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new
                {
                    container = container?.Name,
                    contentType,
                    hash,
                    length,
                    limitCount
                });
            }
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="contentType">Type of the content.(no-use)</param>
        /// <param name="hash">The hash.</param>
        /// <param name="length">The length.(no-use)</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>
        /// IEnumerable&lt;CloudBlockBlob&gt;.
        /// </returns>
        public override IEnumerable<OssObjectSummary> QueryBlob(string containerName, string contentType = null, CryptoKey hash = null, long? length = null, int limitCount = 100)
        {
            try
            {
                return QueryBlob(containerName, limitCount);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, limitCount });
            }
        }

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns></returns>
        private IEnumerable<OssObjectSummary> QueryBlob(string bucketName, int limitCount = 100)
        {
            try
            {
                bucketName.CheckEmptyString(nameof(bucketName));
                return client.ListObjects(new ListObjectsRequest(bucketName)
                {
                    MaxKeys = limitCount
                }).ObjectSummaries;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { bucketName, limitCount });
            }
        }

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>System.Collections.Generic.List&lt;System.String&gt;.</returns>
        public override List<string> GetContainers()
        {
            try
            {
                var buckets = client.ListBuckets();
                return (from one in buckets select one.Name).ToList();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion CloudBinaryStorageOperator interfaces
    }
}