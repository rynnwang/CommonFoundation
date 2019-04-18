using Beyova.Api.RestApi;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static partial class HttpApiExtension
    {
        /// <summary>
        /// To the full raw URL.
        /// </summary>
        /// <param name="runtimeContext">The runtime context.</param>
        /// <returns>System.String.</returns>
        public static string ToFullRawUrl(this RuntimeContext runtimeContext)
        {
            return runtimeContext == null ? string.Empty : (string.Format("/{0}/{1}/{2}/{3}/", runtimeContext.ApiMethod, runtimeContext.Version, runtimeContext.ResourceName, runtimeContext.ActionName).TrimEnd('/') + "/");
        }
    }
}