using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class BinaryStorageActionCredential.
    /// </summary>
    public class BinaryStorageActionCredential : BinaryStorageIdentifier
    {
        /// <summary>
        /// Gets or sets the storage URI.
        /// </summary>
        /// <value>The storage URI.</value>
        [JsonProperty(PropertyName = "storageUri")]
        public string StorageUri { get; set; }

        /// <summary>
        /// Gets or sets the credential URI.
        /// <remarks>Credential uri, which is used to upload, download or any other action on target.</remarks>
        /// </summary>
        /// <value>The credential URI.</value>
        [JsonProperty(PropertyName = "credentialUri")]
        public string CredentialUri { get; set; }

        /// <summary>
        /// Gets or sets the credential expired stamp.
        /// </summary>
        /// <value>The credential expired stamp.</value>
        [JsonProperty(PropertyName = "credentialExpiredStamp")]
        public DateTime? CredentialExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the name of the original.
        /// </summary>
        /// <value>
        /// The name of the original.
        /// </value>
        [JsonProperty(PropertyName = "originalName")]
        public string OriginalName { get; set; }
    }
}