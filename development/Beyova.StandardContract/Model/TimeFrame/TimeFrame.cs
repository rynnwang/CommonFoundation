using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeFrame : ITimeFrame
    {
        /// <summary>
        /// Gets or sets the size of the frame.
        /// </summary>
        /// <value>
        /// The size of the frame.
        /// </value>
        [JsonProperty(PropertyName = "frameSize")]
        public uint FrameSize { get; set; }

        /// <summary>
        /// Gets or sets the frame scope.
        /// </summary>
        /// <value>
        /// The frame scope.
        /// </value>
        [JsonProperty(PropertyName = "frameScope")]
        public TimeScope FrameScope { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Every {0} {1}(s)", FrameSize, FrameScope);
        }
    }
}