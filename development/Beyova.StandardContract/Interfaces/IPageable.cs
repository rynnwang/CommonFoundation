namespace Beyova
{
    /// <summary>
    /// Interface IPageable
    /// </summary>
    public interface IPageable
    {
        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; set; }

        /// <summary>
        /// Gets or sets the start index.
        /// </summary>
        /// <value>
        /// The start index.
        /// </value>
        int StartIndex { get; set; }
    }
}