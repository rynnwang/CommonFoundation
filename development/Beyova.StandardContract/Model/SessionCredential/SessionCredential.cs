//using System;

//namespace Beyova
//{
//    /// <summary>
//    /// </summary>
//    public class SessionCredential : IExpirable, IAccessClientIdentifier, IRealmable
//    {
//        /// <summary>
//        /// Gets or sets the user key.
//        /// </summary>
//        /// <value>The user key.</value>
//        public Guid? UserKey { get; set; }

//        /// <summary>
//        /// Gets or sets the user agent.
//        /// </summary>
//        /// <value>The user agent.</value>
//        public string UserAgent { get; set; }

//        /// <summary>
//        /// Gets or sets the token.
//        /// </summary>
//        /// <value>The token.</value>
//        public CryptoKey Token { get; set; }

//        /// <summary>
//        /// Gets or sets the expired stamp.
//        /// </summary>
//        /// <value>The expired stamp.</value>
//        public DateTime? ExpiredStamp { get; set; }

//        /// <summary>
//        /// Gets or sets the created stamp.
//        /// </summary>
//        /// <value>The created stamp.</value>
//        public DateTime? CreatedStamp { get; set; }

//        /// <summary>
//        /// Gets or sets the device identifier.
//        /// </summary>
//        /// <value>The device identifier.</value>
//        public string DeviceId { get; set; }

//        /// <summary>
//        /// Gets or sets the name of the device.
//        /// </summary>
//        /// <value>The name of the device.</value>
//        public string DeviceName { get; set; }

//        /// <summary>
//        /// Gets or sets the realm.
//        /// </summary>
//        /// <value>The realm.</value>
//        public string Realm { get; set; }

//        /// <summary>
//        /// Gets or sets the ip v4 address.
//        /// </summary>
//        /// <value>
//        /// The ip v4 address.
//        /// </value>
//        public string IpV4Address { get; set; }

//        /// <summary>
//        /// Gets or sets the ip v6 address.
//        /// </summary>
//        /// <value>
//        /// The ip v6 address.
//        /// </value>
//        public string IpV6Address { get; set; }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="SessionCredential"/> class.
//        /// </summary>
//        public SessionCredential() { }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="SessionCredential" /> class.
//        /// </summary>
//        /// <param name="sessionInfo">The session information.</param>
//        protected SessionCredential(SessionCredential sessionInfo)
//        {
//            if (sessionInfo != null)
//            {
//                this.UserKey = sessionInfo.UserKey;
//                this.IpV4Address = sessionInfo.IpV4Address;
//                this.IpV6Address = sessionInfo.IpV6Address;
//                this.Realm = sessionInfo.Realm;
//                this.DeviceId = sessionInfo.DeviceId;
//                this.DeviceName = sessionInfo.DeviceName;
//                this.CreatedStamp = sessionInfo.CreatedStamp;
//                this.ExpiredStamp = sessionInfo.ExpiredStamp;
//                this.Token = sessionInfo.Token;
//                this.UserAgent = sessionInfo.UserAgent;
//            }
//        }
//    }
//}