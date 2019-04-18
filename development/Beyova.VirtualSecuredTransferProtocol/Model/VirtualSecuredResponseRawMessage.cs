using System;

namespace Beyova.VirtualSecuredTransferProtocol
{
    /// <summary>
    /// Class VirtualSecuredResponseRawMessage stands for the RSA-decrpted raw message object for response.
    /// Byte[] composition: [Schema Version]{1}[UTC Stamp]{4}[Encrypted Body]{N}.
    /// Total Length = 1 + 4 + N
    /// </summary>
    public class VirtualSecuredResponseRawMessage : VirtualSecuredRawMessageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualSecuredResponseRawMessage"/> class.
        /// </summary>
        public VirtualSecuredResponseRawMessage()
        {
            Stamp = DateTime.UtcNow;
        }
    }
}