using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// Class SessionInfo.
    /// </summary>
    [Obsolete("New systems should use SessionCredential")]
    public class SessionInfo : IExpirable
    {
        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>The user key.</value>
        public Guid? UserKey { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>The token.</value>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the expired stamp.
        /// </summary>
        /// <value>The expired stamp.</value>
        public DateTime? ExpiredStamp { get; set; }

        /// <summary>
        /// Gets or sets the created stamp.
        /// </summary>
        /// <value>The created stamp.</value>
        public DateTime? CreatedStamp { get; set; }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>The platform.</value>
        public PlatformType Platform { get; set; }

        /// <summary>
        /// Gets or sets the type of the device.
        /// </summary>
        /// <value>The type of the device.</value>
        public DeviceType DeviceType { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>The device identifier.</value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the device.
        /// </summary>
        /// <value>The name of the device.</value>
        public string DeviceName { get; set; }

        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>The realm.</value>
        public string Realm { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfo"/> class.
        /// </summary>
        public SessionInfo() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionInfo" /> class.
        /// </summary>
        /// <param name="sessionInfo">The session information.</param>
        protected SessionInfo(SessionInfo sessionInfo)
        {
            if (sessionInfo != null)
            {
                this.UserKey = sessionInfo.UserKey;
                this.IpAddress = sessionInfo.IpAddress;
                this.Platform = sessionInfo.Platform;
                this.Realm = sessionInfo.Realm;
                this.DeviceId = sessionInfo.DeviceId;
                this.DeviceName = sessionInfo.DeviceName;
                this.DeviceType = sessionInfo.DeviceType;
                this.CreatedStamp = sessionInfo.CreatedStamp;
                this.ExpiredStamp = sessionInfo.ExpiredStamp;
                this.Token = sessionInfo.Token;
                this.UserAgent = sessionInfo.UserAgent;
            }
        }
    }
}