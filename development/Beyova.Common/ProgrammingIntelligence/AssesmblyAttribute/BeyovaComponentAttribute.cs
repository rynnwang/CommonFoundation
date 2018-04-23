using System;

namespace Beyova
{
    /// <summary>
    /// Class BeyovaComponentAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class BeyovaComponentAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the underlying object.
        /// </summary>
        /// <value>
        /// The underlying object.
        /// </value>
        public BeyovaComponentInfo UnderlyingObject
        {
            get; protected set;
        }

        /// <summary>
        /// Gets or sets the nu get parameter.
        /// </summary>
        /// <value>
        /// The nu get parameter.
        /// </value>
        public NuGetPackageAssemblyParameter NuGetParameter { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        public BeyovaComponentAttribute(string id, string version)
            : this(id, version, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="nugetParameter">The nuget parameter.</param>
        public BeyovaComponentAttribute(string id, string version, NuGetPackageAssemblyParameter nugetParameter)
                 : this(id, version, null, nugetParameter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="apiTrackingType">Type of the API tracking.</param>
        public BeyovaComponentAttribute(string id, string version, Type apiTrackingType)
            : this(id, version, apiTrackingType, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="apiTrackingType">Type of the API tracking.</param>
        /// <param name="nugetParameter">The nuget parameter.</param>
        public BeyovaComponentAttribute(string id, string version, Type apiTrackingType, NuGetPackageAssemblyParameter nugetParameter)
        {
            this.UnderlyingObject = new BeyovaComponentInfo(id, version, apiTrackingType);
            this.NuGetParameter = nugetParameter;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.UnderlyingObject?.ToString();
        }

        /// <summary>
        /// Gets the nu get package version.
        /// </summary>
        /// <returns></returns>
        public string GetNuGetPackageVersion()
        {
            var version = this.UnderlyingObject?.Version.AsVersion();
            return version == null ? string.Empty : ToVersionString(version);
        }

        /// <summary>
        /// To the version string.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        private string ToVersionString(Version version)
        {
            // Why 2017/11/11? Just developer's joke.
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, (int)(DateTime.UtcNow - new DateTime(2017, 11, 11, 8, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        }

        /// <summary>
        /// Gets the next nu get version.
        /// </summary>
        /// <param name="existedHighestVersion">The existed highest version.</param>
        /// <returns></returns>
        public string GetNextNuGetVersion(string existedHighestVersion)
        {
            Version expectedVersion, highestVersion;

            if (!Version.TryParse((this.NuGetParameter?.Version).SafeToString(this.UnderlyingObject?.Version), out expectedVersion))
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