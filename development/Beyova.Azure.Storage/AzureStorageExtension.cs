using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace Beyova.Azure
{
    /// <summary>
    /// Class AzureStorageExtension.
    /// </summary>
    public static class AzureStorageExtension
    {
        private const string metaDuration = "duration";

        private const string metaHeight = "height";

        private const string metaWidth = "width";

        #region Utility

        /// <summary>
        /// The disposition format
        /// </summary>
        private const string dispositionFormat = "attachment; filename=\"{0}\"";

        /// <summary>
        /// The regex
        /// </summary>
        private static Regex regex = new Regex(string.Format(dispositionFormat.Replace(" ", "\\s").Replace("\"", "(\")?"), "?<name>([0-9a-zA-Z\\-_%@\\.]+)"), RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Converts the name to content disposition.
        /// </summary>
        /// <param name="anyBlobName">Name of any BLOB.</param>
        /// <returns>System.String.</returns>
        public static string ConvertNameToContentDisposition(this string anyBlobName)
        {
            return string.IsNullOrWhiteSpace(anyBlobName) ? anyBlobName : string.Format(dispositionFormat, anyBlobName.Trim().ToUrlEncodedText());
        }

        /// <summary>
        /// Converts the name of the content disposition to.
        /// </summary>
        /// <param name="contentDisposition">The content disposition.</param>
        /// <returns>System.String.</returns>
        public static string ConvertContentDispositionToName(this string contentDisposition)
        {
            var match = regex.Match(contentDisposition.SafeToString());
            if (match.Success)
            {
                return match.Result("${name}");
            }

            return contentDisposition;
        }

        /// <summary>
        /// Fills the meta.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="blobProperties">The BLOB properties.</param>
        public static void FillMeta(this BinaryStorageMetaData meta, BlobProperties blobProperties)
        {
            if (meta != null && blobProperties != null)
            {
                meta.Hash = blobProperties.ContentMD5;
                meta.ContentType = blobProperties.ContentType;
                meta.Name = blobProperties.ContentDisposition.ConvertContentDispositionToName();
            }
        }

        /// <summary>
        /// Sets the BLOB property.
        /// </summary>
        /// <param name="blobProperties">The BLOB properties.</param>
        /// <param name="meta">The meta.</param>
        public static void FillMeta(this BlobProperties blobProperties, BinaryStorageMetaBase meta)
        {
            if (meta != null && blobProperties != null)
            {
                blobProperties.ContentType = meta.ContentType.SafeToString(HttpConstants.ContentType.BinaryDefault);
                blobProperties.ContentDisposition = meta.Name?.ConvertNameToContentDisposition();
            }
        }

        /// <summary>
        /// Fills the meta.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="metaData">The meta data.</param>
        public static void FillMeta(this Dictionary<string, string> meta, BinaryStorageMetaData metaData)
        {
            if (meta != null && metaData != null)
            {
                if (metaData.Duration != null)
                {
                    meta.Merge(metaDuration, metaData.Duration.ToString());
                }

                if (metaData.Height != null)
                {
                    meta.Merge(metaHeight, metaData.Height.ToString());
                }

                if (metaData.Width != null)
                {
                    meta.Merge(metaWidth, metaData.Width.ToString());
                }
            }
        }

        #endregion Utility

        /// <summary>
        /// Connections the string to credential.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <returns>Microsoft.WindowsAzure.Storage.CloudStorageAccount.</returns>
        public static CloudStorageAccount ConnectionStringToCredential(this string storageConnectionString)
        {
            return ToCloudStorageAccount(AsAzureBlobEndpoint(storageConnectionString));
        }

        /// <summary>
        /// To the cloud storage account.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>Microsoft.WindowsAzure.Storage.CloudStorageAccount.</returns>
        public static CloudStorageAccount ToCloudStorageAccount(this AzureBlobEndpoint endpoint)
        {
            return ToCloudStorageAccount(endpoint, endpoint.Region);
        }

        /// <summary>
        /// To the cloud storage account.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="region">The region.</param>
        /// <returns>Microsoft.WindowsAzure.Storage.CloudStorageAccount.</returns>
        public static CloudStorageAccount ToCloudStorageAccount(this Api.ApiEndpoint endpoint, AzureServiceProviderRegion region)
        {
            if (endpoint != null)
            {
                var useHttps = endpoint.Protocol.SafeEquals(HttpConstants.HttpProtocols.Https, StringComparison.InvariantCultureIgnoreCase);
                var accountKey = endpoint.Token;
                var accountName = endpoint.Account;
                var customBlobDomain = endpoint.Host;

                return new CloudStorageAccount(
                    new StorageCredentials(accountName, accountKey),
                    string.IsNullOrWhiteSpace(customBlobDomain)
                        ? GetStorageEndpointUri(useHttps, accountName, "blob", region)
                        : new Uri(customBlobDomain),
                    GetStorageEndpointUri(useHttps, accountName, "queue", region),
                    GetStorageEndpointUri(useHttps, accountName, "table", region),
                    GetStorageEndpointUri(useHttps, accountName, "file", region));
            }

            return null;
        }

        /// <summary>
        /// As azure BLOB endpoint.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <returns>Beyova.AzureExtension.AzureBlobEndpoint.</returns>
        public static AzureBlobEndpoint AsAzureBlobEndpoint(this string storageConnectionString)
        {
            var keyValues = storageConnectionString.SafeToString().ParseToNameValueCollection(';');
            var useHttps = keyValues.Get("DefaultEndpointsProtocol")?.Equals(HttpConstants.HttpProtocols.Https, StringComparison.InvariantCultureIgnoreCase) ?? false;
            var accountKey = keyValues.Get("AccountKey");
            var accountName = keyValues.Get("AccountName");
            var customBlobDomain = keyValues.Get("CustomBlobDomain");
            AzureServiceProviderRegion region;
            Enum.TryParse(keyValues.Get("Region"), true, out region);

            return new AzureBlobEndpoint
            {
                Account = accountName,
                Token = accountKey,
                Protocol = useHttps ? HttpConstants.HttpProtocols.Https : HttpConstants.HttpProtocols.Http,
                Host = customBlobDomain,
                Region = region
            };
        }

        /// <summary>
        /// To the connection string.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <returns>System.String.</returns>
        public static string ToConnectionString(this AzureBlobEndpoint endpoint)
        {
            if (endpoint != null)
            {
                var values = new Dictionary<string, string>
                {
                    { "Region", endpoint.Region.ToString() },
                    { "DefaultEndpointsProtocol", endpoint.Protocol.SafeToString(HttpConstants.HttpProtocols.Https) }
                };
                values.AddIfBothNotNullOrEmpty("AccountKey", endpoint.Token);
                values.AddIfBothNotNullOrEmpty("AccountName", endpoint.Account);
                values.AddIfBothNotNullOrEmpty("CustomBlobDomain", endpoint.Host);

                return values.ToKeyValuePairString(';');
            }

            return string.Empty;
        }

        /// <summary>
        /// To the cloud storage account.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="region">The region.</param>
        /// <param name="useHttps">if set to <c>true</c> [use HTTPS].</param>
        /// <returns></returns>
        internal static CloudStorageAccount ToCloudStorageAccount(this StorageCredentials credential, AzureServiceProviderRegion region = AzureServiceProviderRegion.Global, bool useHttps = true)
        {
            return new CloudStorageAccount(
                credential,
                GetStorageEndpointUri(useHttps, credential.AccountName, "blob", region),
                GetStorageEndpointUri(useHttps, credential.AccountName, "queue", region),
                GetStorageEndpointUri(useHttps, credential.AccountName, "table", region),
                GetStorageEndpointUri(useHttps, credential.AccountName, "file", region));
        }

        /// <summary>
        /// Gets the storage endpoint URI.
        /// </summary>
        /// <param name="useHttps">if set to <c>true</c> [use HTTPS].</param>
        /// <param name="accountName">Name of the account.</param>
        /// <param name="feature">The feature.</param>
        /// <param name="region">The region.</param>
        /// <returns>Uri.</returns>
        internal static Uri GetStorageEndpointUri(bool useHttps, string accountName, string feature, AzureServiceProviderRegion region)
        {
            string uriFormat;

            switch (region)
            {
                case AzureServiceProviderRegion.China:
                    uriFormat = "{0}://{1}.{2}.core.chinacloudapi.cn";
                    break;

                default:
                    uriFormat = "{0}://{1}.{2}.core.windows.net";
                    break;
            }

            return new Uri(string.Format(uriFormat, useHttps ? HttpConstants.HttpProtocols.Https : HttpConstants.HttpProtocols.Http, accountName, feature));
        }

        /// <summary>
        /// To the HTTP request.
        /// </summary>
        /// <param name="actionCredential">The action credential.</param>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns></returns>
        public static HttpWebRequest CreateHttpRequest(this BinaryStorageActionCredential actionCredential, string httpMethod = HttpConstants.HttpMethod.Get)
        {
            if (actionCredential != null)
            {
                var httpRequest = actionCredential.CredentialUri.CreateHttpWebRequest(httpMethod);
                httpRequest.Headers.Set("x-ms-blob-type", "BlockBlob");

                return httpRequest;
            }

            return null;
        }
    }
}