using System;

namespace Beyova
{
    /// <summary>
    /// Class BeyovaCultureResourceAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class BeyovaCultureResourceAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the underlying object.
        /// </summary>
        /// <value>
        /// The underlying object.
        /// </value>
        public BeyovaCultureResourceInfo UnderlyingObject { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeyovaComponentAttribute" /> class.
        /// Parameter <c>resourceBaseName</c> is suggested to contains namespace as full name.
        /// </summary>
        /// <param name="cultureResourceDirectory">The culture resource directory. It is based on value of <see cref="EnvironmentCore.ApplicationBaseDirectory"/>.</param>
        /// <param name="resourceBaseName">Name of the resource base. it is suggested to contains namespace as full name</param>
        /// <param name="defaultCultureCode">The default culture code.</param>
        /// <param name="tryLanguageCompatibility">if set to <c>true</c> [try language compatibility].</param>
        public BeyovaCultureResourceAttribute(string cultureResourceDirectory, string resourceBaseName, string defaultCultureCode, bool tryLanguageCompatibility = true)
        {
            this.UnderlyingObject = new BeyovaCultureResourceInfo(cultureResourceDirectory, resourceBaseName, defaultCultureCode, tryLanguageCompatibility);
        }
    }
}