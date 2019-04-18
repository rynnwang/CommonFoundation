namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITimeFrame
    {
        /// <summary>
        /// Gets or sets the size of the frame.
        /// </summary>
        /// <value>
        /// The size of the frame.
        /// </value>
        uint FrameSize { get; set; }

        /// <summary>
        /// Gets or sets the frame scope.
        /// </summary>
        /// <value>
        /// The frame scope.
        /// </value>
        TimeScope FrameScope { get; set; }
    }
}