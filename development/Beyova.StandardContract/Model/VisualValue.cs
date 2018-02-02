using System;

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
        public BinaryStorageMetaBase Image { get; set; }

        /// <summary>
        /// Gets or sets the audio.
        /// </summary>
        /// <value>
        /// The audio.
        /// </value>
        public BinaryStorageMetaBase Audio { get; set; }

        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>
        /// The video.
        /// </value>
        public BinaryStorageMetaBase Video { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public TextValue Text { get; set; }
    }
}