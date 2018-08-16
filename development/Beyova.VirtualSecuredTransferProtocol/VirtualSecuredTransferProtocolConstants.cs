using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Beyova.ExceptionSystem;

namespace Beyova.VirtualSecuredTransferProtocol
{
    /// <summary>
    /// class VirtualSecuredTransferProtocolConstants
    /// </summary>
    public static class VirtualSecuredTransferProtocolConstants
    {
        /// <summary>
        /// The header key client identifier. GMID = Gravity member ID
        /// </summary>
        public const string headerKey_ClientId = "X-BA-GMID";

    }
}
