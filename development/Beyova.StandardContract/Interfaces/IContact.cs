namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContact
    {
        /// <summary>
        /// Gets or sets the name of the contact.
        /// </summary>
        /// <value>
        /// The name of the contact.
        /// </value>
        string ContactName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the call number.
        /// </summary>
        /// <value>
        /// The call number.
        /// </value>
        string CallNumber { get; set; }
    }
}