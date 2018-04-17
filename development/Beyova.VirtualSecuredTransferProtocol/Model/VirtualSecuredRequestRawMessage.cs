using System;

namespace Beyova.VirtualSecuredTransferProtocol
{
    /// <summary>
    /// Class VirtualSecuredRequestRawMessage stands for the RSA-decrpted raw message object for request.
    /// Byte[] composition: [Schema Version]{1}[UTC Stamp]{4}[Encrypted Security Key Length Indication]{2+2}[Encrypted Security Key]{M+N}[Encrypted Body]{L}.
    /// Total Length = 1 + 4 + 2 + M + N
    /// </summary>
    public class VirtualSecuredRequestRawMessage : VirtualSecuredRawMessageBase
    {
        /// <summary>
        /// Gets or sets the symmetric primary key.
        /// </summary>
        /// <value>
        /// The symmetric primary key.
        /// </value>
        public CryptoKey SymmetricPrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the symmetric secondary key.
        /// </summary>
        /// <value>
        /// The symmetric secondary key.
        /// </value>
        public CryptoKey SymmetricSecondaryKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualSecuredRequestRawMessage"/> class.
        /// </summary>
        public VirtualSecuredRequestRawMessage() : base()
        {
        }
    }
}