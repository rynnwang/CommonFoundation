using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStorageContainer : IDisposable
    {
        /// <summary>
        /// Puts the item.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="overwriteIfExsits">if set to <c>true</c> [overwrite if exsits].</param>
        void PutItem(string relativePath, byte[] bytes, bool overwriteIfExsits = false);

        /// <summary>
        /// Puts the item.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="overwriteIfExsits">if set to <c>true</c> [overwrite if exsits].</param>
        void PutItem(string relativePath, Stream stream, bool overwriteIfExsits = false);

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="relativePath">The relative.</param>
        /// <returns></returns>
        Stream GetItem(string relativePath);

        /// <summary>
        /// Gets the item paths.
        /// </summary>
        /// <returns></returns>
        string[] GetItemPaths();

        /// <summary>
        /// Writes to destination.
        /// </summary>
        void WriteToDestination();
    }
}
