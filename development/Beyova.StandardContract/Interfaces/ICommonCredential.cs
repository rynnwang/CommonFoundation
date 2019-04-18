namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public interface ICommonCredential
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        string Account { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        string Token { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        string Domain { get; set; }
    }
}