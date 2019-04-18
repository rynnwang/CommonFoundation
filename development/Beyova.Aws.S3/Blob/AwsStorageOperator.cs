using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Beyova;
using Beyova.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;

namespace Beyova.Aws
{
    /// <summary>
    /// Class AwsStorageOperator.
    /// </summary>
    public class AwsStorageOperator : CloudBinaryStorageOperator<S3Bucket, S3Object>, ICloudBinaryStorageOperator
    {
        #region Fields

        /// <summary>
        /// The BLOB client
        /// </summary>
        internal IAmazonS3 blobClient;

        #endregion Fields

        /// <summary>
        /// To the aws credentials.
        /// </summary>
        /// <param name="apiEndpoint">The API endpoint.</param>
        /// <returns>AWSCredentials.</returns>
        protected static AWSCredentials ToAWSCredentials(ApiEndpoint apiEndpoint)
        {
            if (apiEndpoint != null)
            {
                var accessKey = apiEndpoint.Token;
                var accessSecretKey = apiEndpoint.SecondaryToken;

                return new Amazon.Runtime.BasicAWSCredentials(accessKey, accessSecretKey);
            }

            return null;
        }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsStorageOperator" /> class.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="region">The region.</param>
        public AwsStorageOperator(AWSCredentials credential, RegionEndpoint region)
        {
            blobClient = new AmazonS3Client(credential, region);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsStorageOperator"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="region">The region.</param>
        public AwsStorageOperator(ApiEndpoint endpoint, string region)
            : this(ToAWSCredentials(endpoint), Amazon.RegionEndpoint.GetBySystemName(region))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsStorageOperator"/> class.
        /// </summary>
        /// <param name="serviceEndpoint">The service endpoint.</param>
        public AwsStorageOperator(RegionalServiceEndpoint serviceEndpoint)
            : this(serviceEndpoint, serviceEndpoint?.Region)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsStorageOperator" /> class.
        /// </summary>
        /// <param name="serviceEndpoint">The service endpoint.</param>
        public AwsStorageOperator(AwsBlobEndpoint serviceEndpoint)
                    : this(ToAWSCredentials(serviceEndpoint), serviceEndpoint?.Region)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsStorageOperator"/> class.
        /// </summary>
        public AwsStorageOperator() : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AwsStorageOperator"/> class.
        /// </summary>
        /// <param name="initializeBlobClient">if set to <c>true</c> [initialize BLOB client].</param>
        protected AwsStorageOperator(bool initializeBlobClient)
        {
            if (initializeBlobClient)
            {
                blobClient = new AmazonS3Client();
            }
        }

        #endregion Constructor

        #region Protected methods

        /// <summary>
        /// Creates the BLOB URI.
        /// </summary>
        /// <param name="bucketName">Name of the bucket.</param>
        /// <param name="blobIdentifier">The BLOB identifier(S3 key).</param>
        /// <param name="verb">Http verb.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">Hash.</param>
        /// <param name="contentType">Content type.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        protected BinaryStorageActionCredential CreateBlobUri(string bucketName,
            string blobIdentifier, HttpVerb verb, int expireOffsetInMinute = 10, CryptoKey hash = null, string contentType = null)
        {
            try
            {
                bucketName.CheckEmptyString(nameof(bucketName));
                blobIdentifier.CheckEmptyString(nameof(blobIdentifier));

                return GenerateBlobUri(bucketName, blobIdentifier, verb, expireOffsetInMinute, hash, contentType);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { bucketName, expireOffsetInMinute });
            }
        }

        /// <summary>
        /// Generates the BLOB URI.
        /// </summary>
        /// <param name="bucketName">The container(S3 bucketName).</param>
        /// <param name="blobIdentifier">The BLOB identifier(S3 key).</param>
        /// <param name="verb">Http verb.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">Hash.</param>
        /// <param name="contentType">Content type.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        protected BinaryStorageActionCredential GenerateBlobUri(string bucketName,
            string blobIdentifier, HttpVerb verb, int expireOffsetInMinute = 10, CryptoKey hash = null, string contentType = null)
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = blobIdentifier,
                Verb = verb,
                ContentType = contentType,
                Expires = DateTime.Now.AddMinutes(expireOffsetInMinute),
            };
            if (hash != null && hash.HasValue)
            {
                request.Headers["x-amz-meta-md5hash"] = hash.StringValue;
            }

            var url = blobClient.GetPreSignedURL(request);

            //Return the URI string for the bucket, including the SAS token.
            return new BinaryStorageActionCredential()
            {
                CredentialUri = url,
                StorageUri = CredentialUriToStorageUri(url), //remove token
                Container = bucketName,
                Identifier = blobIdentifier,
                CredentialExpiredStamp = request.Expires
            };
        }

        #endregion Protected methods

        #region Private methods

        /// <summary>
        /// get the storage uri from credential uri
        /// </summary>
        /// <param name="credentialUri">the credential uri</param>
        /// <returns>the storage uri</returns>
        private string CredentialUriToStorageUri(string credentialUri)
        {
            var uri = new Uri(credentialUri);
            var storageUri = uri.Scheme + "://" + uri.Host + uri.LocalPath;
            return storageUri;
        }

        /// <summary>
        /// Get bucket name by Blob Uri
        /// </summary>
        /// <param name="credentialUri">credential Uri</param>
        /// <returns>bucket name</returns>
        private string GetBucketNameByCredentialUri(string credentialUri)
        {
            try
            {
                credentialUri.CheckEmptyString(nameof(credentialUri));

                var uri = new Uri(credentialUri);
                var uriScheme = uri.Host;
                return uriScheme.Split('.').FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex.Handle(credentialUri);
            }
        }

        /// <summary>
        /// Get key string by Blob Uri
        /// </summary>
        /// <param name="credentialUri">credential Uri</param>
        /// <returns>key</returns>
        private string GetKeyByCredentialUri(string credentialUri)
        {
            try
            {
                credentialUri.CheckEmptyString(nameof(credentialUri));

                var uri = new Uri(credentialUri);
                var localPath = uri.LocalPath;
                return localPath.Remove(0, 1);
            }
            catch (Exception ex)
            {
                throw ex.Handle(credentialUri);
            }
        }

        #endregion Private methods

        #region Public methods

        /// <summary>
        /// Query S3 Objects
        /// </summary>
        /// <param name="bucketName">Bucket name</param>
        /// <param name="prefix">Limits the response to keys that begin with the specified prefix</param>
        /// <param name="marker">Specifies the key to start with when listing objects in a bucket</param>
        /// <param name="maxKeys"> Sets the maximum number of keys returned in the response. The response might contain fewer keys but will never contain more.</param>
        /// <returns>IEnumerable&lt;TCloudBlobObject&gt;</returns>
        public IEnumerable<S3Object> QueryS3Objects(string bucketName, string prefix = null,
            string marker = null, int maxKeys = 100)
        {
            try
            {
                bucketName.CheckNullObject(nameof(bucketName));

                var request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    MaxKeys = maxKeys,
                    Prefix = prefix,
                    Marker = marker
                };

                return blobClient.ListObjects(request).S3Objects;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { bucketName, prefix, marker, maxKeys });
            }
        }

        #endregion Public methods

        #region CloudBinaryStorageOperator interfaces

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
            return CreateBlobUri(containerName, blobIdentifier.SafeToString(Guid.NewGuid().ToString()), HttpVerb.PUT,
                expireOffsetInMinute, hash, contentType);
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
            return CreateBlobUri(containerName, blobIdentifier, HttpVerb.GET, expireOffsetInMinute);
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
                    using (var response = blobClient.GetObject(identifier.Container, identifier.Identifier))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not exist"))
                {
                    return false;
                }
                else
                {
                    throw ex.Handle(identifier);
                }
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
                storageIdentifier.CheckNullObject("storageIdentifier");
                storageIdentifier.Container.CheckEmptyString("storageIdentifier.Container");
                storageIdentifier.Identifier.CheckEmptyString("storageIdentifier.Identifier");

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = storageIdentifier.Container,
                    Key = storageIdentifier.Identifier
                };
                DeleteObjectResponse result = blobClient.DeleteObject(deleteObjectRequest);
            }
            catch (Exception ex)
            {
                throw ex.Handle(storageIdentifier);
            }
        }

        /// <summary>
        /// Downloads the binary bytes by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.Byte[].</returns>
        public override byte[] DownloadBinaryBytesByCredentialUri(string blobUri)
        {
            try
            {
                using (var stream = DownloadBinaryStreamByCredentialUri(blobUri))
                {
                    return stream.ReadStreamToBytes();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(blobUri);
            }
        }

        /// <summary>
        /// Downloads the binary MD5 stream by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.IO.Stream.</returns>
        public override Stream DownloadBinaryStreamByCredentialUri(string blobUri)
        {
            try
            {
                blobUri.CheckEmptyString(nameof(blobUri));
                var bucketName = GetBucketNameByCredentialUri(blobUri);
                var key = GetKeyByCredentialUri(blobUri);

                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };
                GetObjectResponse response = blobClient.GetObject(request);

                return response.ResponseStream;
            }
            catch (Exception ex)
            {
                throw ex.Handle(blobUri);
            }
        }

        /// <summary>
        /// Uploads the binary stream by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>ETag</returns>
        public override string UploadBinaryStreamByCredentialUri(string blobUri, Stream stream,
            string contentType, string fileName = null)
        {
            try
            {
                blobUri.CheckEmptyString("blobUri");
                stream.CheckNullObject("stream");

                // Upload a file using the pre-signed URL.
                var bucketName = GetBucketNameByCredentialUri(blobUri);
                var key = GetKeyByCredentialUri(blobUri);
                var streamBytes = stream.ReadStreamToBytes();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                var md5hash = md5.ComputeHash(streamBytes).EncodeBase64();
                HttpWebRequest httpRequest = WebRequest.Create(blobUri) as HttpWebRequest;
                httpRequest.Method = "PUT";
                httpRequest.ContentType = contentType;
                httpRequest.Headers["x-amz-meta-md5hash"] = md5hash;
                if (!string.IsNullOrEmpty(fileName))
                {
                    httpRequest.Headers["Content-Disposition"] = fileName;
                }

                using (Stream dataStream = httpRequest.GetRequestStream())
                {
                    // Upload blob stream.
                    dataStream.Write(streamBytes, 0, streamBytes.Length);
                }

                HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
                return response.GetResponseHeader("ETag");
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { blobUri, contentType, fileName });
            }
        }

        /// <summary>
        /// Uploads the binary bytes by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>ETag</returns>
        public override string UploadBinaryBytesByCredentialUri(string blobUri, byte[] dataBytes, string contentType, string fileName = null)
        {
            try
            {
                blobUri.CheckEmptyString(nameof(blobUri));
                dataBytes.CheckNullObject(nameof(dataBytes));

                using (var stream = dataBytes.ToStream())
                {
                    return UploadBinaryStreamByCredentialUri(blobUri, stream, contentType, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { blobUri, contentType, fileName });
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
                identifier.CheckNullObject("identifier");
                identifier.Container.CheckEmptyString("identifier.Container");
                identifier.Identifier.CheckEmptyString("identifier.Identifier");

                var request = new GetObjectMetadataRequest
                {
                    BucketName = identifier.Container,
                    Key = identifier.Identifier,
                };

                var blob = blobClient.GetObjectMetadata(request);
                var result = new BinaryStorageMetaData(identifier)
                {
                    Hash = blob.Metadata["x-amz-meta-md5hash"],
                    Length = blob.ContentLength,
                    Name = blob.Headers.ContentDisposition,
                    ContentType = blob.Headers.ContentType
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
        /// <param name="hash">The MD5.</param>
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

                var request = new ListObjectsRequest
                {
                    BucketName = containerName,
                };

                var s3ObjectList = blobClient.ListObjects(request).S3Objects;
                List<BinaryStorageMetaData> binaryStorageMetaBaseList = new List<BinaryStorageMetaData>();
                foreach (var s3Obj in s3ObjectList)
                {
                    var key = s3Obj.Key;
                    BinaryStorageIdentifier identifier = new BinaryStorageIdentifier
                    {
                        Container = containerName,
                        Identifier = key
                    };
                    var meta = FetchCloudMeta(identifier);

                    if ((string.IsNullOrWhiteSpace(contentType) || meta.ContentType.Equals(contentType, StringComparison.OrdinalIgnoreCase))
                                && (string.IsNullOrWhiteSpace(hash) || meta.Hash.Equals(hash))
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
        /// <param name="md5">The MD5.(no-use)</param>
        /// <param name="length">The length.(no-use)</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>IEnumerable&lt;TCloudBlobObject&gt;.</returns>
        public override IEnumerable<S3Object> QueryBlob(S3Bucket container, string contentType = null, CryptoKey md5 = null,
            long? length = null, int limitCount = 100)
        {
            try
            {
                container.CheckNullObject(nameof(container));
                return QueryS3Objects(container.BucketName, null, null, limitCount);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new
                {
                    container = container?.BucketName,
                    contentType,
                    md5,
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
        /// <param name="hash">The MD5.(no-use)</param>
        /// <param name="length">The length.(no-use)</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>IEnumerable&lt;CloudBlockBlob&gt;.</returns>
        public override IEnumerable<S3Object> QueryBlob(string containerName, string contentType = null, CryptoKey hash = null, long? length = null, int limitCount = 100)
        {
            try
            {
                containerName.CheckEmptyString("containerName");
                return QueryS3Objects(containerName, null, null, limitCount);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { containerName, limitCount });
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
                var buckets = blobClient.ListBuckets();
                return (from one in buckets.Buckets select one.BucketName).ToList();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion CloudBinaryStorageOperator interfaces

        /// <summary>
        /// Creates the s3 server irrelevant storage operator.
        /// </summary>
        /// <returns></returns>
        public static ICloudBinaryStorageOperator CreateCredentialIrrelavantStorageOperator()
        {
            return new AwsStorageOperator(false);
        }
    }
}