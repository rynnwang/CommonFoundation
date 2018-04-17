using System.Globalization;

namespace Beyova
{
    /// <summary>
    /// Interface II18NResourceOperatable
    /// </summary>
    public interface II18NResourceOperatable
    {
        /// <summary>
        /// Gets the resource string.
        /// </summary>
        /// <param name="resourceKey">The resource key.</param>
        /// <param name="typeRequired">The type required.</param>
        /// <param name="cultureInfo">The culture information.</param>
        /// <param name="languageCompatibility">if set to <c>true</c> [language compatibility].</param>
        /// <returns>
        /// System.String.
        /// </returns>
        string GetResourceString(string resourceKey, GlobalCultureResourceType? typeRequired = null, CultureInfo cultureInfo = null, bool languageCompatibility = true);

        /// <summary>
        /// Determines whether [has culture resource] [the specified culture information].
        /// </summary>
        /// <param name="cultureInfo">The culture information.</param>
        /// <returns><c>true</c> if [has culture resource] [the specified culture information]; otherwise, <c>false</c>.</returns>
        bool HasCultureResource(CultureInfo cultureInfo);
    }
}