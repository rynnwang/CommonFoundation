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
        /// Parameter <c>resourceBaseName</c> should contains namespace as full name.
        /// </summary>
        /// <param name="cultureResourceDirectory">The culture resource directory.</param>
        /// <param name="resourceBaseName">Name of the resource base.</param>
        /// <param name="defaultCultureCode">The default culture code.</param>
        /// <param name="tryLanguageCompatibility">if set to <c>true</c> [try language compatibility].</param>
        public BeyovaCultureResourceAttribute(string cultureResourceDirectory, string resourceBaseName, string defaultCultureCode, bool tryLanguageCompatibility = true)
        {
            this.UnderlyingObject = new BeyovaCultureResourceInfo(cultureResourceDirectory, resourceBaseName, defaultCultureCode, tryLanguageCompatibility);
        }
    }
}