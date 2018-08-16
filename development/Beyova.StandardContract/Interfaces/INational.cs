namespace Beyova
{
    /// <summary>
    /// Interface <see cref="INational" /> is based on entity. For user based, use <see cref="IGlobalObjectName" />
    /// </summary>
    public interface INational
    {
        /// <summary>
        /// Gets or sets the nation code.
        /// </summary>
        /// <value>
        /// The nation code.
        /// </value>
        string NationCode { get; set; }
    }
}