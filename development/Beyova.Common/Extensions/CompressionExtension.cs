using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class CompressionExtension.
    /// </summary>
    public static class CompressionExtension
    {
        #region GZip

        /// <summary>
        /// </summary>
        /// <param name="bytesObject">The bytes object.</param>
        /// <returns></returns>
        public static byte[] GZipCompressBytes(this byte[] bytesObject)
        {
            if (bytesObject != null && bytesObject.Length > 0)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                        {
                            gZipStream.Write(bytesObject, 0, bytesObject.Length);
                        }

                        memoryStream.Position = 0;

                        var compressedData = new byte[memoryStream.Length];
                        memoryStream.Read(compressedData, 0, compressedData.Length);

                        var gZipBuffer = new byte[compressedData.Length + 4];
                        Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
                        Buffer.BlockCopy(BitConverter.GetBytes(bytesObject.Length), 0, gZipBuffer, 0, 4);

                        return compressedData;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(bytesObject);
                }
            }

            return null;
        }

        /// <summary>
        /// Gzip compress bytes to string.
        /// </summary>
        /// <param name="bytesObject">The bytes object.</param>
        /// <returns></returns>
        public static string GZipCompressBytesToString(this byte[] bytesObject)
        {
            var compresssedBytes = GZipCompressBytes(bytesObject);
            return compresssedBytes == null ? null : Convert.ToBase64String(compresssedBytes);
        }

        /// <summary>
        /// Gzip compress.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string GZipCompress(this string stringObject, Encoding encoding = null)
        {
            var buffer = string.IsNullOrWhiteSpace(stringObject) ? null : (encoding ?? Encoding.UTF8).GetBytes(stringObject);
            return GZipCompressBytesToString(buffer);
        }

        /// <summary>
        /// Gzip compress object to string.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns></returns>
        public static string GZipCompressObjectToString(this object anyObject)
        {
            if (anyObject != null)
            {
                var binaryFormatter = new BinaryFormatter();
                byte[] bytes = null;

                using (var memoryStream = new MemoryStream())
                {
                    binaryFormatter.Serialize(memoryStream, anyObject);
                    bytes = memoryStream.ToArray();
                }

                return bytes.GZipCompressBytesToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Decompresses the specified string object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>System.String.</returns>
        public static string GZipDecompress(this string stringObject, Encoding encoding = null)
        {
            return (encoding ?? Encoding.UTF8).GetString(GZipDecompressStringToBytes(stringObject));
        }

        /// <summary>
        /// Decompresses the string automatic object.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Object.</returns>
        public static object GZipDecompressStringToObject(this string stringObject)
        {
            object result = null;
            var bytes = stringObject.GZipDecompressStringToBytes();

            if (bytes != null && bytes.Length > 0)
            {
                var binaryFormatter = new BinaryFormatter();
                using (var memoryStream = new MemoryStream(bytes))
                {
                    result = binaryFormatter.Deserialize(memoryStream);
                }
            }

            return result;
        }

        /// <summary>
        /// Decompresses the string automatic bytes.
        /// </summary>
        /// <param name="stringObject">The string object.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="OperationFailureException">Decompress</exception>
        public static byte[] GZipDecompressStringToBytes(this string stringObject)
        {
            if (!string.IsNullOrWhiteSpace(stringObject))
            {
                try
                {
                    byte[] gZipBuffer = Convert.FromBase64String(stringObject);
                    return GZipDecompress(gZipBuffer);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(stringObject);
                }
            }

            return new byte[] { };
        }

        /// <summary>
        /// Decompresses the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.Byte[][].</returns>
        /// <exception cref="OperationFailureException">Decompress</exception>
        public static byte[] GZipDecompress(this byte[] bytes)
        {
            if (bytes != null && bytes.Length > 0)
            {
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        int dataLength = BitConverter.ToInt32(bytes, 0);
                        memoryStream.Write(bytes, 4, bytes.Length - 4);

                        var buffer = new byte[dataLength];

                        memoryStream.Position = 0;
                        using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                        {
                            gZipStream.Read(buffer, 0, buffer.Length);
                        }

                        return buffer;
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(bytes);
                }
            }

            return null;
        }

        #endregion

        /// <summary>
        /// Extracts the zip file.
        /// </summary>
        /// <param name="zipFilePath">The zip file path.</param>
        /// <param name="extractPath">The extract path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <exception cref="OperationFailureException">ExtractZipFile</exception>
        public static void ExtractZipFile(this string zipFilePath, string extractPath, Encoding encoding = null)
        {
            if (!string.IsNullOrWhiteSpace(zipFilePath) && !string.IsNullOrWhiteSpace(extractPath))
            {
                try
                {
                    ZipFile.ExtractToDirectory(zipFilePath, extractPath, encoding ?? Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { zipFilePath, extractPath });
                }
            }
        }

        /// <summary>
        /// Gets the archive entry by path.
        /// </summary>
        /// <param name="archive">The archive.</param>
        /// <param name="entryPathToExtract">The entry path to extract.
        /// <remarks><example>Payload/xxx.app/Info.plist</example></remarks>
        /// </param>
        /// <returns>ZipArchiveEntry.</returns>
        private static ZipArchiveEntry GetArchiveEntryByPath(this ZipArchive archive, string entryPathToExtract)
        {
            try
            {
                archive.CheckNullObject(nameof(archive));
                entryPathToExtract.CheckEmptyString(nameof(entryPathToExtract));

                return archive.GetEntry(entryPathToExtract);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { entryPathToExtract });
            }
        }

        /// <summary>
        /// Extracts the zip by entry path.
        /// </summary>
        /// <param name="zipFileStreamToOpen">The zip file stream to open.</param>
        /// <param name="entryPathToExtract">The entry path to extract.
        /// <remarks><example>Payload/xxx.app/Info.plist</example></remarks></param>
        /// <param name="destinationDirectoryPath">The destination directory path.</param>
        public static void ExtractZipByEntryPath(this Stream zipFileStreamToOpen, string entryPathToExtract, string destinationDirectoryPath)
        {
            try
            {
                zipFileStreamToOpen.CheckNullObject(nameof(zipFileStreamToOpen));
                entryPathToExtract.CheckEmptyString(nameof(entryPathToExtract));

                using (var archive = new ZipArchive(zipFileStreamToOpen, ZipArchiveMode.Read))
                {
                    archive.GetEntry(entryPathToExtract).ExtractToFile(destinationDirectoryPath);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { entryPathToExtract, destinationDirectoryPath });
            }
        }

        /// <summary>
        /// Extracts the zip by entry path.
        /// </summary>
        /// <param name="zipFileStreamToOpen">The zip file stream to open.</param>
        /// <param name="entryPathToExtract">The entry path to extract.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ExtractZipByEntryPath(this Stream zipFileStreamToOpen, string entryPathToExtract)
        {
            try
            {
                zipFileStreamToOpen.CheckNullObject(nameof(zipFileStreamToOpen));
                entryPathToExtract.CheckEmptyString(nameof(entryPathToExtract));

                using (var archive = new ZipArchive(zipFileStreamToOpen, ZipArchiveMode.Read))
                {
                    var archiveEntry = archive.GetArchiveEntryByPath(entryPathToExtract);
                    if (archiveEntry != null)
                    {
                        using (var stream = archiveEntry.Open())
                        {
                            return stream.ToBytes();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { entryPathToExtract });
            }

            return null;
        }

        /// <summary>
        /// Zips as bytes.
        /// <remarks>
        /// In parameter items, if you want to add folder, please use path like {directory}/{fileName}.
        /// <example>
        /// Folder1/Folder11/File1.txt
        /// </example>
        /// </remarks>
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="compressionLevel">The compression level.</param>
        /// <returns></returns>
        public static byte[] ZipAsBytes(this Dictionary<string, byte[]> items, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                items.CheckNullObject(nameof(items));

                using (var memoryStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var one in items)
                        {
                            var entry = archive.CreateEntry(one.Key);

                            using (var entryStream = entry.Open())
                            {
                                entryStream.Write(one.Value, 0, one.Value.Length);
                                entryStream.Flush();
                            }
                        }
                    }

                    return memoryStream.ToBytes();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { compressionLevel, items = items.Keys });
            }
        }

        /// <summary>
        /// Zips as bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="compressionLevel">The compression level.</param>
        /// <returns></returns>
        public static byte[] ZipAsBytes(this byte[] bytes, string fileName, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                bytes.CheckNullObject(nameof(bytes));
                fileName.CheckEmptyString(nameof(fileName));

                var fileToZip = new Dictionary<string, byte[]> { { fileName, bytes } };
                return ZipAsBytes(fileToZip, compressionLevel);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { compressionLevel, fileName });
            }
        }

        /// <summary>
        /// Zips to path.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="compressionLevel">The compression level.</param>
        public static void ZipToPath(this Dictionary<string, byte[]> items, string destinationPath, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                destinationPath.CheckNullObject(nameof(destinationPath));
                File.WriteAllBytes(destinationPath, items.ZipAsBytes(compressionLevel));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { items = items?.Keys, destinationPath });
            }
        }

        /// <summary>
        /// Zips to path.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="compressionLevel">The compression level.</param>
        public static void ZipToPath(this byte[] bytes, string fileName, string destinationPath, CompressionLevel compressionLevel = CompressionLevel.Optimal)
        {
            try
            {
                destinationPath.CheckNullObject(nameof(destinationPath));
                fileName.CheckEmptyString(nameof(fileName));

                File.WriteAllBytes(destinationPath, bytes.ZipAsBytes(fileName, compressionLevel));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { destinationPath, fileName });
            }
        }

        /// <summary>
        /// Zips as bytes.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="archiveItems">The archive items.</param>
        /// <returns></returns>
        public static byte[] ZipAsBytes(this object anyObject, params KeyValuePair<string, byte[]>[] archiveItems)
        {
            return ZipAsBytes(archiveItems.ToDictionary());
        }
    }
}