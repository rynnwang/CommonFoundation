using System;

namespace Beyova.Diagnostic
{
    /// <summary>
    /// Interface IApiEventLogBase.
    /// </summary>
    public interface IApiEventLogBase : IApiLogBase
    {
        /// <summary>
        /// Gets or sets the exception key.
        /// </summary>
        /// <value>The exception key.</value>
        Guid? ExceptionKey { get; set; }

        /// <summary>
        /// Gets or sets the culture code.
        /// </summary>
        /// <value>The culture code.</value>
        string CultureCode { get; set; }

        /// <summary>
        /// Gets or sets the client identifier.
        /// Commonly, it can be device id, PC name, etc.
        /// </summary>
        /// <value>The client identifier.</value>
        string ClientIdentifier { get; set; }

        /// <summary>
        /// Gets or sets the ip address.
        /// </summary>
        /// <value>The ip address.</value>
        string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the trace identifier.
        /// </summary>
        /// <value>The trace identifier.</value>
        string TraceId { get; set; }

        /// <summary>
        /// Gets or sets the operator credential.
        /// </summary>
        /// <value>The operator credential.</value>
        BaseCredential OperatorCredential { get; set; }

        /// <summary>
        /// Gets or sets the raw URL.
        /// </summary>
        /// <value>
        /// The raw URL.
        /// </value>
        string RawUrl { get; set; }
    }
}