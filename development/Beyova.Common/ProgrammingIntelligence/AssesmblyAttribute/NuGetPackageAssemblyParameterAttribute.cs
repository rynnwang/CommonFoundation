using System;
using System.Collections;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class NuGetPackageAssemblyParameterAttribute is used in assembly to define NuGet parameters for CI system to read.
    /// </summary>
    public class NuGetPackageAssemblyParameterAttribute
    {
        /// <summary>
        /// Gets or sets the NuGet identifier.
        /// </summary>
        /// <value>
        /// The NuGet identifier.
        /// </value>
        public string NuGetId { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the nu get specification path. It should be relative path.
        /// </summary>
        /// <value>
        /// The nu get specification path.
        /// </value>
        public string NuGetSpecificationPath { get; set; }

        /// <summary>
        /// Gets or sets the output bin path. It should be relative path.
        /// </summary>
        /// <value>
        /// The output bin path.
        /// </value>
        public string OutputBinPath { get; set; }

          /// <summary>
        /// Gets the next nu get version.
        /// </summary>
        /// <param name="existedHighestVersion">The existed highest version.</param>
        /// <returns></returns>
        public string GetNextNuGetVersion(string existedHighestVersion, BeyovaComponentInfo componentInfo)
        {
            Version expectedVersion, highestVersion;

            if (!componentInfo.Version.TryParse((this.Version).SafeToString(componentInfo?.Version), out expectedVersion))
            {
                expectedVersion = new Version("1.0.0.0");
            }

            if (Version.TryParse(existedHighestVersion, out highestVersion) && expectedVersion <= highestVersion)
            {
                expectedVersion = new Version(highestVersion.Major, highestVersion.Minor, highestVersion.Build + 1, 0);
            }

            return expectedVersion.ToString();
        }
    }
}
