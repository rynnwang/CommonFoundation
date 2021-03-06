﻿using Beyova;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Beyova
{
    /// <summary>
    /// Class EnvironmentCore.
    /// </summary>
    public static class EnvironmentCore
    {
        /// <summary>
        /// The application base directory
        /// </summary>
        public static readonly string ApplicationBaseDirectory;

        /// <summary>
        /// The log directory
        /// </summary>
        public static readonly string LogDirectory;

        /// <summary>
        /// The application identifier
        /// </summary>
        public static readonly int ApplicationId;

        /// <summary>
        /// The machine name
        /// </summary>
        public static readonly string MachineName;

        /// <summary>
        /// The local machine host name
        /// </summary>
        public static readonly string LocalMachineHostName = string.Empty;

        /// <summary>
        /// The local machine ip address
        /// </summary>
        public static readonly string LocalMachineIpAddress = string.Empty;

        /// <summary>
        /// Gets the machine identifier.
        /// </summary>
        /// <returns></returns>
        public static MachineIdentifier GetMachineIdentifier()
        {
            return new MachineIdentifier
            {
                HostName = LocalMachineHostName,
                IpAddress = LocalMachineIpAddress,
                MachineName = MachineName
            };
        }

        /// <summary>
        /// Gets the name of the product.
        /// </summary>
        /// <value>The name of the product.</value>
        public static string ProductName { get; private set; }

        // public static ReadOnlyCollection<Assembly> DescendingAssemblyDependencyChain;

        /// <summary>
        /// The descending assembly dependency chain. Descending here means order by referenced amount.
        /// So result would be like: web -> core -> contract - > common -> json.net, etc.
        /// </summary>
        public static ReadOnlyCollection<Assembly> DescendingAssemblyDependencyChain { get; private set; }

        /// <summary>
        /// The ascending assembly dependency chain
        /// </summary>
        public static ReadOnlyCollection<Assembly> AscendingAssemblyDependencyChain { get; private set; }

        /// <summary>
        /// Gets or sets the common component information.
        /// </summary>
        /// <value>
        /// The common component information.
        /// </value>
        public static BeyovaComponentInfo CommonComponentInfo { get; private set; }

        /// <summary>
        /// Initializes static members of the <see cref="EnvironmentCore"/> class.
        /// </summary>
        static EnvironmentCore()
        {
            var baseDirectoryByAppDomain = AppDomain.CurrentDomain.BaseDirectory;
            var baseDirectoryByAssemblyLocation = Path.GetDirectoryName(typeof(EnvironmentCore).Assembly.Location);

            // NOTE:
            // In IIS Express cases, baseDirectoryByAssemblyLocation would be allocated into asp.net tmp folders, by each library.
            // In other cases, IIS or Console or Windows environments, baseDirectoryByAssemblyLocation should be correct.
            DirectoryInfo baseDirectory = new DirectoryInfo((baseDirectoryByAppDomain.StartsWith(baseDirectoryByAssemblyLocation, StringComparison.OrdinalIgnoreCase)) ?
                 baseDirectoryByAssemblyLocation
                 : Path.Combine(baseDirectoryByAppDomain, "bin"));

            if (baseDirectory?.Exists ?? false)
            {
                ApplicationBaseDirectory = baseDirectory.ToString();
            }
            else
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(baseDirectory), new
                {
                    baseDirectory = baseDirectory?.ToString(),
                    baseDirectoryByAppDomain,
                    baseDirectoryByAssemblyLocation
                });
            }

            LogDirectory = Path.Combine(ApplicationBaseDirectory, "logs");
            ApplicationId = System.AppDomain.CurrentDomain.Id;

            var dependencyChain = ReflectionExtension.GetAppDomainAssemblies().GetAssemblyDependencyChain(true);
            AscendingAssemblyDependencyChain = new List<Assembly>(dependencyChain).AsReadOnly();
            dependencyChain.Reverse();

            DescendingAssemblyDependencyChain = dependencyChain.AsReadOnly();       

            CommonComponentInfo = typeof(EnvironmentCore).Assembly.GetCustomAttribute<BeyovaComponentAttribute>()?.UnderlyingObject;

            try
            {
                MachineName = Environment.MachineName;
            }
            catch { MachineName = string.Empty; }

            try
            {
                LocalMachineHostName = Dns.GetHostName();

                var host = Dns.GetHostEntry(LocalMachineHostName);
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        LocalMachineIpAddress = ip.ToString();
                        break;
                    }
                }
            }
            catch { }

            try
            {
                ProductName = FindProductName();

                if (string.IsNullOrWhiteSpace(ProductName))
                {
                    ProductName = Assembly.GetEntryAssembly()?.FullName;
                }

                if (string.IsNullOrWhiteSpace(ProductName) && AppDomain.CurrentDomain != null)
                {
                    ProductName = AppDomain.CurrentDomain.FriendlyName;
                }
            }
            catch { ProductName = string.Empty; }
        }

        /// <summary>
        /// Gets the total memory. Unit: bytes
        /// </summary>
        /// <value>The total memory.</value>
        public static long TotalMemory
        {
            get
            {
                return GC.GetTotalMemory(false);
            }
        }

        /// <summary>
        /// Finds the name of the product.
        /// </summary>
        /// <returns>System.String.</returns>
        private static string FindProductName()
        {
            try
            {
                string result = null;
                foreach (var one in DescendingAssemblyDependencyChain)
                {
                    var component = one.GetCustomAttribute<BeyovaComponentAttribute>();
                    if (component != null && !string.IsNullOrWhiteSpace(component.UnderlyingObject.Id))
                    {
                        result = component.UnderlyingObject.Id;
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(result))
                {
                    result = DescendingAssemblyDependencyChain.FirstOrDefault()?.GetName()?.Name;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="defaultSubDirectoryName">Default name of the sub directory.</param>
        /// <returns>System.IO.DirectoryInfo.</returns>
        internal static DirectoryInfo GetDirectory(string directory, string defaultSubDirectoryName = null)
        {
            string directoryPath;
            if (string.IsNullOrWhiteSpace(directory))
            {
                directoryPath = string.IsNullOrWhiteSpace(defaultSubDirectoryName) ? EnvironmentCore.ApplicationBaseDirectory : Path.Combine(EnvironmentCore.ApplicationBaseDirectory, defaultSubDirectoryName);
            }
            else if (directory.Contains(':') || directory.StartsWith("\\\\"))
            {
                directoryPath = directory;
            }
            else
            {
                directoryPath = directory.TrimStart('/', '~', '.').TrimEnd('/', '\\').Replace('/', '\\');
                directoryPath = string.IsNullOrWhiteSpace(directoryPath) ? EnvironmentCore.ApplicationBaseDirectory : Path.Combine(EnvironmentCore.ApplicationBaseDirectory, directoryPath);
            }

            return new DirectoryInfo(directoryPath);
        }

        /// <summary>
        /// Gets the assembly hash.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string GetAssemblyHash()
        {
            try
            {
                StringBuilder builder = new StringBuilder(DescendingAssemblyDependencyChain.Count * 16);
                MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();

                foreach (var one in DescendingAssemblyDependencyChain)
                {
                    builder.Append(md5Provider.ComputeHash(File.ReadAllBytes(one.Location)).EncodeBase64());
                }

                return md5Provider.ComputeHash(Encoding.UTF8.GetBytes(builder.ToString())).EncodeBase64();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}