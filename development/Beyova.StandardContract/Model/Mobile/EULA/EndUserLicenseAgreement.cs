namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Beyova.SimpleBaseObject" />
    public class EndUserLicenseAgreement : SimpleBaseObject
    {
        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public TextValue Body { get; set; }
    }
}