using System;

namespace Beyova.ApiTracking
{
    /// <summary>
    /// Class ApiTrackingOptionAttribute
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiTrackingOptionAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of the omit.
        /// </summary>
        /// <value>
        /// The type of the omit.
        /// </value>
        public ApiTrackingType OmitType { get; protected set; }

        /// <summary>
        /// Gets or sets the event tracking option controller.
        /// </summary>
        /// <value>
        /// The event tracking option controller.
        /// </value>
        public IApiEventTrackingOptionController EventTrackingOptionController { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingOptionAttribute"/> class.
        /// </summary>
        /// <param name="omitType">Type of the omit.</param>
        internal ApiTrackingOptionAttribute(ApiTrackingType omitType)
        {
            this.OmitType = omitType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingOptionAttribute"/> class.
        /// </summary>
        /// <param name="eventTrackingOptionController">The event tracking option controller.</param>
        internal ApiTrackingOptionAttribute(IApiEventTrackingOptionController eventTrackingOptionController)
        {
            this.EventTrackingOptionController = eventTrackingOptionController;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiTrackingOptionAttribute"/> class.
        /// </summary>
        /// <param name="trackingOption">The tracking option.</param>
        public ApiTrackingOptionAttribute(ApiEventTrackingOption trackingOption)
        {
            this.EventTrackingOptionController = new DefaultApiEventTrackingOptionController(trackingOption);
        }

        /// <summary>
        /// Omits the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        internal bool Omit(ApiTrackingType type)
        {
            return OmitType.HasFlag(type);
        }
    }
}