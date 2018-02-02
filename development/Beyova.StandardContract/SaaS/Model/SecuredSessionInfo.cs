using System;

namespace Beyova.SaasPlatform
{
    /// <summary>
    /// 
    /// </summary>
    public class SecuredSessionInfo : SessionInfo, IExpirable
    {
        /// <summary>
        /// Gets or sets the server private key.
        /// </summary>
        /// <value>
        /// The server private key.
        /// </value>
        public string ServerPrivateKey { get; set; }

        /// <summary>
        /// Gets or sets the server public key.
        /// </summary>
        /// <value>
        /// The server public key.
        /// </value>
        public string ServerPublicKey { get; set; }

        /// <summary>
        /// Gets or sets the client public key.
        /// </summary>
        /// <value>
        /// The client public key.
        /// </value>
        public string ClientPublicKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredSessionInfo"/> class.
        /// </summary>
        public SecuredSessionInfo() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredSessionInfo"/> class.
        /// </summary>
        /// <param name="sessionInfo">The session information.</param>
        protected SecuredSessionInfo(SessionInfo sessionInfo) : base(sessionInfo) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecuredSessionInfo"/> class.
        /// </summary>
        /// <param name="sessionInfo">The session information.</param>
        protected SecuredSessionInfo(SecuredSessionInfo sessionInfo) : base(sessionInfo)
        {
            this.ServerPrivateKey = sessionInfo.ServerPrivateKey;
            this.ServerPublicKey = sessionInfo.ServerPublicKey;
        }
    }
}