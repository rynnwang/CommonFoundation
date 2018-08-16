using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StorageContainer : IStorageContainer
    {
        /// <summary>
        /// The data
        /// </summary>
        protected Dictionary<string, Stream> _data = new Dictionary<string, Stream>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The already written
        /// </summary>
        protected bool _alreadyWritten = false;

        /// <summary>
        /// Attaches the specified relative path.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="overwriteIfExsits">if set to <c>true</c> [overwrite if exsits].</param>
        public void Attach(string relativePath, byte[] bytes, bool overwriteIfExsits = false)
        {
            if (!string.IsNullOrWhiteSpace(relativePath) && bytes.HasItem())
            {
                _data.Merge(relativePath, bytes.ToStream(), overwriteIfExsits);
            }
        }

        /// <summary>
        /// Attaches the specified relative path.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="overwriteIfExsits">if set to <c>true</c> [overwrite if exsits].</param>
        public void Attach(string relativePath, Stream stream, bool overwriteIfExsits = false)
        {
            if (!string.IsNullOrWhiteSpace(relativePath) && stream != null)
            {
                _data.Merge(relativePath, stream, overwriteIfExsits);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            if (!_alreadyWritten)
            {
                WriteToDestination();
            }
        }

        /// <summary>
        /// Gets the item paths.
        /// </summary>
        /// <returns></returns>
        public string[] GetItemPaths()
        {
            return _data.Keys.ToArray();
        }

        /// <summary>
        /// Extracts to destination.
        /// </summary>
        protected abstract void ExtractToDestination();

        /// <summary>
        /// Writes to destination.
        /// </summary>
        public void WriteToDestination()
        {
            try
            {
                if (_alreadyWritten)
                {
                    throw ExceptionFactory.CreateOperationForbiddenException("Already written");
                }

                ExtractToDestination();
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
            finally
            {
                _alreadyWritten = true;

                foreach (var one in _data)
                {
                    one.Value.SafeDispose();
                }
            }
        }
    }
}
