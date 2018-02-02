using System;
using System.Collections;
using System.Collections.Generic;
using Beyova.ExceptionSystem;

namespace Beyova
{
    /// <summary>
    /// Class NuGetPackageAssemblyParameter is used in <see cref="BeyovaComponentAttribute"/> to define NuGet parameters for CI system to read.
    /// </summary>
    public class NuGetPackageAssemblyParameter
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
    }
}
