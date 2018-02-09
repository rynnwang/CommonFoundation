using Beyova.Api;

namespace Beyova.ApiTracking
{
    /// <summary>
    ///
    /// </summary>
    public interface IApiEventTrackingOptionController
    {
        /// <summary>
        /// Gets the option.
        /// </summary>
        /// <param name="routeIdentifier">The route identifier.</param>
        /// <param name="omitParameters">if set to <c>true</c> [omit parameters].</param>
        /// <returns></returns>
        ApiEventTrackingOption GetOption(ApiRouteIdentifier routeIdentifier, bool omitParameters = false);
    }
}