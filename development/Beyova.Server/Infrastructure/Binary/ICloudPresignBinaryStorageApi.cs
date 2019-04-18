using Beyova.Api.RestApi;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICloudPresignBinaryStorageApi : ICloudPresignBinaryStorageApi<BinaryStorageMetaData>
    {
    }

    /// <summary>
    /// Interface ICloudPresignBinaryStorageApi
    /// </summary>
    public interface ICloudPresignBinaryStorageApi<TBinaryStorageMetaData>
        where TBinaryStorageMetaData : BinaryStorageMetaData
    {
        /// <summary>
        /// Creates the binary upload credential.
        /// </summary>
        /// <param name="meta">The meta.</param>
        /// <param name="expireOffset">The expire offset.</param>
        /// <returns></returns>
        [ApiOperation(nameof(BinaryStorageActionCredential), HttpConstants.HttpMethod.Post, "UploadCredential")]
        BinaryStorageActionCredential CreateBinaryUploadCredential(TBinaryStorageMetaData meta, int? expireOffset = null);

        /// <summary>
        /// Creates the binary download credential.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="expireOffset">The expire offset.</param>
        /// <returns></returns>
        [ApiOperation(nameof(BinaryStorageActionCredential), HttpConstants.HttpMethod.Post, "DownloadCredential")]
        BinaryStorageActionCredential CreateBinaryDownloadCredential(BinaryStorageIdentifier identifier, int? expireOffset = null);

        /// <summary>
        /// Commits the binary storage.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [ApiOperation(nameof(BinaryStorageMetaData), HttpConstants.HttpMethod.Post, "Commit")]
        TBinaryStorageMetaData CommitBinaryStorage(BinaryStorageCommitRequest request);
    }
}