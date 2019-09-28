using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova
{
    /// <summary>
    /// Extensions for IOExtension
    /// </summary>
    public static class IOExtension
    {
        #region Stream

        /// <summary>
        /// Determines whether [is relative path] [the specified path].
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if [is relative path] [the specified path]; otherwise, <c>false</c>.</returns>
        public static bool IsRelativePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var root = Path.GetPathRoot(path);
            return root.IsInString("", "\\", "/");
        }

        /// <summary>
        /// Saves to.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="absoluteFullPath">The absolute full path.</param>
        /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="fileOptions">The file options.</param>
        public static void SaveTo(this Stream stream, string absoluteFullPath, bool overwriteIfExists = false, int bufferSize = 512, FileOptions fileOptions = FileOptions.None)
        {
            if (stream != null && !string.IsNullOrWhiteSpace(absoluteFullPath))
            {
                try
                {
                    if (File.Exists(absoluteFullPath) && overwriteIfExists)
                    {
                        File.Delete(absoluteFullPath);
                    }

                    using (var fileStream = File.Create(absoluteFullPath, bufferSize < 512 ? 512 : bufferSize, fileOptions))
                    {
                        if (stream.CanSeek)
                        {
                            stream.Position = 0;
                        }

                        stream.CopyTo(fileStream);
                        fileStream.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex.Handle(new { absoluteFullPath, bufferSize, fileOptions });
                }
            }
        }

        /// <summary>
        /// Copies to memory stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static MemoryStream CopyToMemoryStream(this Stream stream)
        {
            if (stream != null)
            {
                try
                {
                    var memoryStream = new MemoryStream();
                    stream.CopyTo(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream;
                }
                catch (Exception ex)
                {
                    throw ex.Handle();
                }
            }

            return null;
        }

        #endregion Stream

        #region IO

        /// <summary>
        /// The dot
        /// </summary>
        private const string dot = ".";

        /// <summary>
        /// Gets the sub directory. If directory is not null, get sub directory instance in type <see cref="DirectoryInfo"/>.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="subDirectoryName">Name of the sub directory.</param>
        /// <returns></returns>
        private static DirectoryInfo InternalGetSubDirectory(this DirectoryInfo directory, string subDirectoryName)
        {
            DirectoryInfo result = null;
            if (directory == null || string.IsNullOrWhiteSpace(subDirectoryName))
            {
                return directory;
            }

            if (directory.Exists)
            {
                result = directory.GetDirectories().FirstOrDefault(x => x.Name.Equals(subDirectoryName, StringComparison.OrdinalIgnoreCase));
            }

            return result ?? new DirectoryInfo(Path.Combine(directory.FullName, subDirectoryName));
        }

        /// <summary>
        /// Gets the sub directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="subDirectoryNames">The sub directory names.</param>
        /// <returns></returns>
        public static DirectoryInfo GetSubDirectory(this DirectoryInfo directory, params string[] subDirectoryNames)
        {
            DirectoryInfo result = directory;

            if (directory != null && subDirectoryNames.HasItem())
            {
                foreach (var item in subDirectoryNames)
                {
                    result = InternalGetSubDirectory(result, item);
                }
            }

            return result;
        }

        /// <summary>
        /// Clears the files.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void ClearFiles(this DirectoryInfo directory)
        {
            if (directory != null && directory.Exists)
            {
                foreach (var one in directory.GetFiles())
                {
                    one.Delete();
                }
            }
        }

        /// <summary>
        /// Checks the directory exist.
        /// </summary>
        /// <param name="directory">The directory.</param>
        public static void CheckDirectoryExist(this DirectoryInfo directory)
        {
            if (directory == null || !directory.Exists)
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(directory), directory?.FullName);
            }
        }

        /// <summary>
        /// Combines the extension.
        /// </summary>
        /// <param name="pureFileName">Name of the pure file.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>System.String.</returns>
        public static string CombineExtension(this string pureFileName, string extension)
        {
            if (!string.IsNullOrWhiteSpace(extension))
            {
                extension = dot + extension.Replace(dot, string.Empty);
            }

            return pureFileName.SafeToString() + extension;
        }

        /// <summary>
        /// Gets the name of the pure file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>System.String.</returns>
        public static string GetPureFileName(this string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                // If file name is like ".gitignore", then still consider full tern is file name.
                return (fileName.SubStringBeforeLastMatch(dot)).SafeToString(fileName);
            }

            return fileName;
        }

        /// <summary>
        /// Gets the temporary folder.
        /// If the tempIdentity is null, method would assign one.
        /// If the folder of tempIdentity is not existed, it would be created.
        /// </summary>
        /// <param name="tempIdentity">The temporary identity.</param>
        /// <returns>
        /// DirectoryInfo.
        /// </returns>
        public static DirectoryInfo GetTempFolder(ref Guid? tempIdentity)
        {
            if (tempIdentity == null)
            {
                tempIdentity = Guid.NewGuid();
            }

            var path = Path.Combine(Path.GetTempPath(), tempIdentity.Value.ToString());
            var directory = new DirectoryInfo(path);

            if (!directory.Exists)
            {
                directory.Create();
            }

            return directory;
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string ReadFileContents(this string path, Encoding encoding = null)
        {
            try
            {
                path.CheckEmptyString(nameof(path));

                if (File.Exists(path))
                {
                    using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var streamReader = new StreamReader(stream, encoding ?? Encoding.UTF8))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle(path);
            }
        }

        /// <summary>
        /// Reads the file contents.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string ReadFileContents(this FileInfo fileInfo, Encoding encoding = null)
        {
            return (fileInfo != null) ? ReadFileContents(fileInfo.FullName, encoding) : null;
        }

        /// <summary>
        /// Reads the file bytes.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] ReadFileBytes(this string path)
        {
            try
            {
                path.CheckEmptyString(nameof(path));
                if (!File.Exists(path))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(path), data: new { path }, reason: "NotFound");
                }
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return stream.ReadStreamToBytes(true);
                }

            }
            catch (Exception ex)
            {
                throw ex.Handle(path);
            }
        }

        /// <summary>
        /// Reads the bytes to stream.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="destinationStream">The destination stream.</param>
        public static void ReadBytesToStream(this string path, Stream destinationStream)
        {
            try
            {
                path.CheckEmptyString(nameof(path));
                destinationStream.CheckNullObject(nameof(destinationStream));

                if (!File.Exists(path))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(path), data: new { path }, reason: "NotFound");
                }
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    stream.CopyTo(destinationStream);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(path);
            }
        }

        /// <summary>
        /// Reads the file bytes.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <returns></returns>
        public static byte[] ReadFileBytes(this FileInfo fileInfo)
        {
            return (fileInfo != null && fileInfo.Exists) ? ReadFileBytes(fileInfo.FullName) : null;
        }

        /// <summary>
        /// Gets the absolute URI.
        /// If the relative Uri is started with ~/ or /, then directory should be the root directory of site.
        /// If the relative Uri is started with ../ or ./, then directory should be the current directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="relativeUri">The relative URI.</param>
        /// <returns>System.String.</returns>
        public static string GetAbsoluteUri(this DirectoryInfo directory, string relativeUri)
        {
            string result = string.Empty;

            if (directory != null && !string.IsNullOrWhiteSpace(relativeUri))
            {
                if (relativeUri.StartsWith("../"))
                {
                    result = GetAbsoluteUri(directory.Parent, relativeUri.Substring(3));
                }
                else if (relativeUri.StartsWith("./"))
                {
                    result = GetAbsoluteUri(directory, relativeUri.Substring(2));
                }
                else if (relativeUri.StartsWith("~/"))
                {
                    result = directory.FullName(false) + relativeUri.Substring(1).Replace('/', '\\');
                }
                else if (relativeUri.StartsWith("/"))
                {
                    result = directory.FullName(false) + relativeUri.Replace('/', '\\');
                }
                // If relativeUri is like C:\, http://, ftp://, etc.
                else if (relativeUri.Contains(':'))
                {
                    result = relativeUri;
                }
                else
                {
                    result = directory.FullName(true) + relativeUri.Replace('/', '\\');
                }
            }

            return result;
        }

        /// <summary>
        /// Generates the relative resource URI.
        /// </summary>
        /// <param name="currentDirectory">The current directory.</param>
        /// <param name="targetAbsoluteUri">The target absolute URI.</param>
        /// <returns>System.String.</returns>
        public static string GenerateRelativeResourceUri(this DirectoryInfo currentDirectory, string targetAbsoluteUri)
        {
            string result = string.Empty;

            if (currentDirectory != null && !string.IsNullOrWhiteSpace(targetAbsoluteUri))
            {
                var currentFolder = currentDirectory.ToString();
                var baseFolder = currentFolder.FindCommonStartSubString(targetAbsoluteUri, true);

                if (!string.IsNullOrWhiteSpace(baseFolder))
                {
                    var depth = currentFolder.Substring(baseFolder.Length).Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).Length;
                    result = String.Concat(Enumerable.Repeat("../", depth)) + targetAbsoluteUri.Substring(baseFolder.Length).TrimStart('/', '\\');
                }
            }

            return result;
        }

        /// <summary>
        /// Fulls the name.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="endWithSlash">if set to <c>true</c> [end with slash].</param>
        /// <returns>System.String.</returns>
        public static string FullName(this DirectoryInfo directory, bool endWithSlash = true)
        {
            string result = string.Empty;

            if (directory != null)
            {
                if (endWithSlash)
                {
                    result = directory.FullName.EndsWith("\\") ? directory.FullName : (directory.FullName + "\\");
                }
                else
                {
                    result = directory.FullName.TrimEnd('\\');
                }
            }

            return result;
        }

        /// <summary>
        /// Ensures the file directory existence.
        /// </summary>
        /// <param name="fileFullPath">The file full path.</param>
        public static void EnsureFileDirectoryExistence(this string fileFullPath)
        {
            try
            {
                fileFullPath.CheckEmptyString(nameof(fileFullPath));

                var directory = Path.GetDirectoryName(fileFullPath);
                if (!string.IsNullOrWhiteSpace(directory))
                {
                    new DirectoryInfo(directory).EnsureExistence();
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { fileFullPath });
            }
        }

        #endregion IO

        #region Bytes

        /// <summary>
        /// To the hexadecimal.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <param name="hasPrefix">if set to <c>true</c> [has prefix].</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string ToHex(this byte[] bytes, bool hasPrefix = false)
        {
            // It is the second implementation for converting byte array to string.
            // http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa

            if (bytes != null && bytes.Length > 0)
            {
                char[] c = new char[bytes.Length * 2];
                int b;
                for (int i = 0; i < bytes.Length; i++)
                {
                    b = bytes[i] >> 4;
                    c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                    b = bytes[i] & 0xF;
                    c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
                }
                var result = new string(c);

                return hasPrefix ? ("0x" + result) : result;
            }

            return string.Empty;
        }

        /// <summary>
        /// Reads the stream to bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="closeWhenFinish">if set to <c>true</c> [close when finish].</param>
        /// <returns>System.Byte[][].</returns>
        public static byte[] ReadStreamToBytes(this Stream stream, bool closeWhenFinish = false)
        {
            long originalPosition = 0;

            try
            {
                stream.CheckNullObject(nameof(stream));

                if (stream.CanSeek)
                {
                    originalPosition = stream.Position;
                    stream.Position = 0;
                }

                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            }
            finally
            {
                if (stream != null)
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = originalPosition;
                    }

                    if (closeWhenFinish)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Reads the stream to bytes asynchronous.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="closeWhenFinish">if set to <c>true</c> [close when finish].</param>
        /// <returns></returns>
        public static async Task<byte[]> ReadStreamToBytesAsync(this Stream stream, bool closeWhenFinish = false)
        {
            long originalPosition = 0;

            try
            {
                stream.CheckNullObject(nameof(stream));

                if (stream.CanSeek)
                {
                    originalPosition = stream.Position;
                    stream.Position = 0;
                }

                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            }
            finally
            {
                if (stream != null)
                {
                    if (stream.CanSeek)
                    {
                        stream.Position = originalPosition;
                    }

                    if (closeWhenFinish)
                    {
                        stream.Close();
                        stream.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// To the stream.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>The <see cref="Stream" />  for byte array.</returns>
        public static Stream ToStream(this byte[] bytes)
        {
            try
            {
                bytes.CheckNullObject(nameof(bytes));

                return new MemoryStream(bytes);
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        #endregion Bytes

        #region Directory

        /// <summary>
        /// Ensures the existence.
        /// </summary>
        /// <param name="directoryInfo">The directory information.</param>
        public static void EnsureExistence(this DirectoryInfo directoryInfo)
        {
            if (directoryInfo != null && !directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }

        #endregion Directory
    }
}