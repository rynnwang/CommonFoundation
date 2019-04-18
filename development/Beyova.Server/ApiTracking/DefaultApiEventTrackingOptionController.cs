using Beyova.Api;

namespace Beyova.Diagnostic
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="IApiEventTrackingOptionController" />
    internal class DefaultApiEventTrackingOptionController : IApiEventTrackingOptionController
    {
        /// <summary>
        /// The tracking option
        /// </summary>
        private ApiEventTrackingOption _trackingOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultApiEventTrackingOptionController"/> class.
        /// </summary>
        /// <param name="trackingOption">The tracking option.</param>
        internal DefaultApiEventTrackingOptionController(ApiEventTrackingOption trackingOption)
        {
            _trackingOption = trackingOption;
        }

        /// <summary>
        /// Gets the option.
        /// </summary>
        /// <param name="routeIdentifier">The route identifier.</param>
        /// <param name="omitParameters">if set to <c>true</c> [omit parameters].</param>
        /// <returns></returns>
        public ApiEventTrackingOption GetOption(ApiRouteIdentifier routeIdentifier, bool omitParameters = false)
        {
            return _trackingOption;
        }
    }
}