using System;
using System.Collections.Generic;
using System.IO;

namespace Beyova
{
    /// <summary>
    /// Interface IBinaryStorageHelper
    /// </summary>
    /// <typeparam name="TBinaryStorageMetaData">The type of the t binary storage meta data.</typeparam>
    /// <typeparam name="TBinaryStorageCriteria">The type of the t binary storage criteria.</typeparam>
    public interface IBinaryStorageHelper<TBinaryStorageMetaData, TBinaryStorageCriteria> : ICloudPresignBinaryStorageApi<TBinaryStorageMetaData>
        where TBinaryStorageMetaData : BinaryStorageMetaData, new()
        where TBinaryStorageCriteria : BinaryStorageMetaDataCriteria, new()
    {
        /// <summary>
        /// Deletes the data.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        void DeleteBinaryStorage(Guid? identifier);

        /// <summary>
        /// Queries the binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;BinaryStorageMetaData&gt;.</returns>
        List<TBinaryStorageMetaData> QueryBinaryStorage(TBinaryStorageCriteria criteria);

        /// <summary>
        /// Queries the user binary storage.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List&lt;TBinaryStorageMetaData&gt;.</returns>
        List<TBinaryStorageMetaData> QueryUserBinaryStorage(TBinaryStorageCriteria criteria);

        /// <summary>
        /// Gets the containers.
        /// </summary>
        /// <returns>IEnumerable&lt;TCloudContainer&gt;.</returns>
        IEnumerable<string> GetContainers();

        /// <summary>
        /// Creates the BLOB download credential. This is blob operation only, no database involved.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>BinaryStorageActionCredential.</returns>
        BinaryStorageActionCredential CreateBlobDownloadCredential(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Creates the BLOB upload credential. This is blob operation only, no database involved.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="hash">The hash. This value is used only when blob service provider needs to set hash (MD5) when creating credential of upload action.</param>
        /// <param name="contentType">Type of the content. This value is used only when blob service provider needs to set content type (MIME) when creating credential of upload action.</param>
        /// <returns>
        /// BinaryStorageActionCredential.
        /// </returns>
        BinaryStorageActionCredential CreateBlobUploadCredential(BinaryStorageIdentifier identifier, CryptoKey hash = null, string contentType = null);

        /// <summary>
        /// Uploads the binary by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <param name="data">The data.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        string UploadBinaryByCredential(BinaryStorageActionCredential credential, byte[] data, string contentType);

        /// <summary>
        /// Downloads the binary by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Stream.</returns>
        Stream DownloadBinaryStreamByCredential(BinaryStorageActionCredential credential);

        /// <summary>
        /// Downloads the binary bytes by credential.
        /// </summary>
        /// <param name="credential">The credential.</param>
        /// <returns>Byte[].</returns>
        Byte[] DownloadBinaryBytesByCredential(BinaryStorageActionCredential credential);

        /// <summary>
        /// Deletes the binary.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        void DeleteBinary(BinaryStorageIdentifier identifier);

        /// <summary>
        /// Gets the binary storage meta data by identifiers.
        /// </summary>
        /// <param name="identifiers">The identifiers.</param>
        /// <returns>System.Collections.Generic.List&lt;TBinaryStorageMetaData&gt;.</returns>
        List<TBinaryStorageMetaData> GetBinaryStorageMetaDataByIdentifiers(List<BinaryStorageIdentifier> identifiers);
    }
}