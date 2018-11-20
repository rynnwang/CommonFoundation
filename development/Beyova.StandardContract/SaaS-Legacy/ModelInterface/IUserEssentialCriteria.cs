namespace Beyova
{
    /// <summary>
    /// Interface IUserInfoEssentialCriteria
    /// </summary>
    public interface IUserEssentialCriteria : IIdentifier, ICredential
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        string Email { get; set; }

        /// <summary>
        /// Gets or sets the nation code.
        /// </summary>
        /// <value>
        /// The nation code.
        /// </value>
        string NationCode { get; set; }

        /// <summary>
        /// Gets or sets the cellphone.
        /// </summary>
        /// <value>
        /// The cellphone.
        /// </value>
        string Cellphone { get; set; }
    }
}