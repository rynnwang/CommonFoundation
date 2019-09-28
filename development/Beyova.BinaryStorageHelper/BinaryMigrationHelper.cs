using Beyova;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Beyova.Binary
{
    /// <summary>
    /// 
    /// </summary>
    public class BinaryMigrationHelper
    {
        /// <summary>
        /// Gets the source storage operator.
        /// </summary>
        /// <value>
        /// The source storage operator.
        /// </value>
        public ICloudBinaryStorageOperator SourceStorageOperator { get; private set; }

        /// <summary>
        /// Gets the destination storage operator.
        /// </summary>
        /// <value>
        /// The destination storage operator.
        /// </value>
        public ICloudBinaryStorageOperator DestinationStorageOperator { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryMigrationHelper"/> class.
        /// </summary>
        /// <param name="sourceStorageOperator">The source storage operator.</param>
        /// <param name="destinationStorageOperator">The destination storage operator.</param>
        public BinaryMigrationHelper(ICloudBinaryStorageOperator sourceStorageOperator, ICloudBinaryStorageOperator destinationStorageOperator)
        {
            sourceStorageOperator.CheckNullObject(nameof(sourceStorageOperator));
            destinationStorageOperator.CheckNullObject(nameof(destinationStorageOperator));

            SourceStorageOperator = sourceStorageOperator;
            DestinationStorageOperator = destinationStorageOperator;
        }

        /// <summary>
        /// Migrates the storage.
        /// </summary>
        /// <param name="meta">The meta.</param>
        public void MigrateStorage(BinaryStorageMetaData meta)
        {
            if (meta != null && !string.IsNullOrWhiteSpace(meta.Container) && !string.IsNullOrWhiteSpace(meta.Identifier))
            {
                try
                {
                    var downloadActionCredential = SourceStorageOperator.CreateBlobDownloadCredential(meta.Container, meta.Identifier, 5);
                    var uploadActionCredential = DestinationStorageOperator.CreateBlobUploadCredential(meta.Container, meta.Identifier, 5, meta.Hash, meta.ContentType);

                    var stream = SourceStorageOperator.DownloadBinaryStreamByCredentialUri(downloadActionCredential.CredentialUri);
                    DestinationStorageOperator.UploadBinaryStreamByCredentialUri(uploadActionCredential.CredentialUri, stream, meta.ContentType, meta.Name);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { meta });
                }
            }
        }

        /// <summary>
        /// Migrates the storage.
        /// </summary>
        /// <param name="metaCollection">The meta collection.</param>
        public void MigrateStorage(IEnumerable<BinaryStorageMetaData> metaCollection)
        {
            if (metaCollection.HasItem())
            {
                if (metaCollection.Count() == 1)
                {
                    MigrateStorage(metaCollection.First());
                }
                else
                {
                    var parallelResult = Parallel.ForEach(metaCollection, new ParallelOptions { MaxDegreeOfParallelism = 3 }, MigrateStorage);
                }
            }
        }
    }
}