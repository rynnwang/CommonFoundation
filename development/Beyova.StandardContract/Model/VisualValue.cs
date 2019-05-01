using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public class VisualValue
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [JsonProperty("image")]
        public BinaryStorageMetaBase Image { get; set; }

        /// <summary>
        /// Gets or sets the audio.
        /// </summary>
        /// <value>
        /// The audio.
        /// </value>
        [JsonProperty("audio")]
        public BinaryStorageMetaBase Audio { get; set; }

        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>
        /// The video.
        /// </value>
        [JsonProperty("video")]
        public BinaryStorageMetaBase Video { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        [JsonProperty("text")]
        public TextValue Text { get; set; }
    }
}