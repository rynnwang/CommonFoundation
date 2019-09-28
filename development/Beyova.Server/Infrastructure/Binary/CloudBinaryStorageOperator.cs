using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;

namespace Beyova
{
    /// <summary>
    /// Class CloudBinaryStorageOperator.
    /// </summary>
    /// <typeparam name="TCloudContainer">The type of the T cloud container.</typeparam>
    /// <typeparam name="TCloudBlobObject">The type of the T cloud BLOB object.</typeparam>
    public abstract class CloudBinaryStorageOperator<TCloudContainer, TCloudBlobObject> : ICloudBinaryStorageOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CloudBinaryStorageOperator{TCloudContainer, TCloudBlobObject}"/> class.
        /// </summary>
        protected CloudBinaryStorageOperator()
        {
        }

        /// <summary>
        /// Creates the BLOB upload credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <param name="hash">The hash. This value is used only when blob service provider needs to set hash (MD5) when creating credential of upload action.</param>
        /// <param name="contentType">Type of the content. This value is used only when blob service provider needs to set content type (MIME) when creating credential of upload action.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public abstract BinaryStorageActionCredential CreateBlobUploadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute, CryptoKey hash = null, string contentType = null);

        /// <summary>
        /// Creates the BLOB download credential.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="blobIdentifier">The BLOB identifier.</param>
        /// <param name="expireOffsetInMinute">The expire offset in minute.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        public abstract BinaryStorageActionCredential CreateBlobDownloadCredential(string containerName, string blobIdentifier, int expireOffsetInMinute);

        /// <summary>
        /// Deletes the BLOB.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        public abstract void DeleteBlob(BinaryStorageIdentifier storageIdentifier);

        /// <summary>
        /// Downloads the binary bytes by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>System.Byte[].</returns>
        public virtual byte[] DownloadBinaryBytesByCredentialUri(string blobUri)
        {
            try
            {
                using (var stream = DownloadBinaryStreamByCredentialUri(blobUri))
                {
                    return stream.ReadStreamToBytes(true);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(blobUri);
            }
        }

        /// <summary>
        /// Downloads the binary stream by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <returns>
        /// Stream.
        /// </returns>
        public virtual Stream DownloadBinaryStreamByCredentialUri(string blobUri)
        {
            try
            {
                blobUri.CheckEmptyString(nameof(blobUri));

                var httpRequest = blobUri.CreateHttpWebRequest();
                var stream = httpRequest.ReadResponseAsStream()?.Body;
                if (stream != null)
                {
                    stream.Position = 0;
                }

                return stream;
            }
            catch (Exception ex)
            {
                throw ex.Handle(blobUri);
            }
        }

        /// <summary>
        /// Uploads the binary bytes by credential.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="dataBytes">The data bytes.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public virtual string UploadBinaryBytesByCredentialUri(string blobUri, byte[] dataBytes, string contentType, string fileName = null)
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
        /// Uploads the binary stream by credential URI.
        /// </summary>
        /// <param name="blobUri">The BLOB URI.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public virtual string UploadBinaryStreamByCredentialUri(string blobUri, Stream stream, string contentType, string fileName = null)
        {
            try
            {
                blobUri.CheckEmptyString(nameof(blobUri));
                stream.CheckNullObject(nameof(stream));

                // Upload a file using the pre-signed URL.

                var streamBytes = stream.ReadStreamToBytes(true);
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                var md5hash = md5.ComputeHash(streamBytes).EncodeBase64();
                HttpWebRequest httpRequest = WebRequest.Create(blobUri) as HttpWebRequest;
                httpRequest.Method = "PUT";
                httpRequest.ContentType = contentType;
                httpRequest.Headers[HttpConstants.HttpHeader.ContentMD5] = md5hash;
                if (!string.IsNullOrEmpty(fileName))
                {
                    httpRequest.Headers[HttpConstants.HttpHeader.ContentDisposition] = fileName;
                }

                using (Stream dataStream = httpRequest.GetRequestStream())
                {
                    // Upload blob stream.
                    dataStream.Write(streamBytes, 0, streamBytes.Length);
                }

                HttpWebResponse response = httpRequest.GetResponse() as HttpWebResponse;
                return response.GetResponseHeader(HttpConstants.HttpHeader.ETag);
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
        /// <returns>BinaryStorageMetaBase.</returns>
        public abstract BinaryStorageMetaData FetchCloudMeta(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public abstract List<string> GetContainers();

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="hash">The MD5.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>
        /// IEnumerable&lt;TCloudBlobObject&gt;.
        /// </returns>
        public abstract IEnumerable<TCloudBlobObject> QueryBlob(TCloudContainer container, string contentType, CryptoKey hash, long? length, int limitCount);

        /// <summary>
        /// Queries the BLOB.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>
        /// IEnumerable&lt;TCloudBlobObject&gt;.
        /// </returns>
        public abstract IEnumerable<TCloudBlobObject> QueryBlob(string container, string contentType, CryptoKey hash, long? length, int limitCount);

        /// <summary>
        /// Queries the binary BLOB by container.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="length">The length.</param>
        /// <param name="limitCount">The limit count.</param>
        /// <returns>
        /// List&lt;BinaryStorageMeta&gt;.
        /// </returns>
        public abstract List<BinaryStorageMetaData> QueryBinaryBlobByContainer(string containerName, string contentType, CryptoKey hash, long? length, int limitCount);

        /// <summary>
        /// Check Existence of the specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns><c>true</c> if existed, <c>false</c> otherwise.</returns>
        public abstract bool Exists(BinaryStorageIdentifier identifier);
    }
}