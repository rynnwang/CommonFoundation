namespace Beyova
{
    /// <summary>
    /// Interface ITextNameSearchable. Note, for I18N supported cases, this interface should apply on language specific.
    /// </summary>
    public interface ITextNameSearchable
    {
        /// <summary>
        /// Gets or sets the full term.
        /// </summary>
        /// <value>
        /// The full term.
        /// </value>
        string FullTerm { get; set; }

        /// <summary>
        /// Gets or sets the short term. (Native language short term)
        /// </summary>
        /// <value>
        /// The short term.
        /// </value>
        string ShortTerm { get; set; }
    }
}