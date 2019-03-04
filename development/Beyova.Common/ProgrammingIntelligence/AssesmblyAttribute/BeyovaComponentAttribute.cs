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
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        public BeyovaComponentAttribute(string id, string version)
            : this(id, version, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="version">The version.</param>
        /// <param name="apiTrackingType">Type of the API tracking.</param>
        public BeyovaComponentAttribute(string id, string version, Type apiTrackingType)
        {
            this.UnderlyingObject = new BeyovaComponentInfo(id, version, apiTrackingType);
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
        /// To the version string.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        private string ToVersionString(Version version)
        {
            // Why 2017/11/11? Just developer's joke.
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, (int)(DateTime.UtcNow - new DateTime(2017, 11, 11, 8, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        }
    }
}