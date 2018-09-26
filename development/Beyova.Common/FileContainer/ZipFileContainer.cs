using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class ZipFileContainer : StorageContainer
    {
        /// <summary>
        /// Gets or sets the destination path.
        /// </summary>
        /// <value>
        /// The destination path.
        /// </value>
        public string DestinationPath { get; protected set; }

        /// <summary>
        /// Gets or sets the destination stream.
        /// </summary>
        /// <value>
        /// The destination stream.
        /// </value>
        public Stream DestinationStream { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileContainer" /> class.
        /// </summary>
        /// <param name="destinationPath">The destination path.</param>
        public ZipFileContainer(string destinationPath) : base()
        {
            this.DestinationPath = destinationPath;
            destinationPath.EnsureFileDirectoryExistence();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipFileContainer"/> class.
        /// </summary>
        /// <param name="destination">The destination.</param>
        public ZipFileContainer(Stream destination) : base()
        {
            this.DestinationStream = destination;
        }

        /// <summary>
        /// Extracts to destination.
        /// </summary>
        protected override void ExtractToDestination()
        {
            string currentPath = null;

            try
            {
                var destination = DestinationStream ?? new MemoryStream();

                using (var archive = new ZipArchive(destination, ZipArchiveMode.Create, true))
                {
                    foreach (var current in this._data)
                    {
                        currentPath = current.Key;

                        var entry = archive.CreateEntry(current.Key);

                        using (var entryStream = entry.Open())
                        {
                            current.Value.CopyTo(entryStream);
                            entryStream.Flush();
                        }
                    }

                    if (DestinationStream != null)
                    {
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(this.DestinationPath))
                        {
                            destination.SaveTo(this.DestinationPath);
                        }

                        destination.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { currentPath });
            }
        }

        /// <summary>
        /// Standardizes the path.
        /// </summary>
        /// <param name="originalPath">The original path.</param>
        /// <returns></returns>
        protected override string StandardizePath(string originalPath)
        {
            return originalPath.TrimStart('/', '\\');
        }
    }
}
