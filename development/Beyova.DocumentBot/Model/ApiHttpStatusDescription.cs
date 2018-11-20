using System.Net;

namespace Beyova.DocumentBot
{
    /// <summary>
    ///
    /// </summary>
    internal class ApiHttpStatusDescription
    {
        /// <summary>
        /// Gets or sets the HTTP status.
        /// </summary>
        /// <value>
        /// The HTTP status.
        /// </value>
        public HttpStatusCode HttpStatus { get; set; }

        /// <summary>
        /// Gets or sets the minor code.
        /// </summary>
        /// <value>
        /// The minor code.
        /// </value>
        public string MinorCode { get; set; }

        /// <summary>
        /// Gets or sets the circumstance.
        /// </summary>
        /// <value>
        /// The circumstance.
        /// </value>
        public string Circumstance { get; set; }

        /// <summary>
        /// Gets or sets the suggestion.
        /// </summary>
        /// <value>
        /// The suggestion.
        /// </value>
        public string Suggestion { get; set; }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; }
    }
}