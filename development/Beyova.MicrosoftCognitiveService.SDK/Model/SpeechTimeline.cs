using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.MicrosoftCognitiveService
{
    /// <summary>
    /// Class SpeechTimeline.
    /// </summary>
    public class SpeechTimeline
    {
        /// <summary>
        /// Gets or sets the index of the text.
        /// </summary>
        /// <value>The index of the text.</value>
        public int? TextIndex { get; set; }

        /// <summary>
        /// Gets or sets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public int? TextLength { get; set; }

        /// <summary>
        /// Gets or sets the index of the audio.
        /// </summary>
        /// <value>The index of the audio.</value>
        public double? AudioIndex { get; set; }

        /// <summary>
        /// Gets or sets the duration of the audio.
        /// </summary>
        /// <value>The duration of the audio.</value>
        public double? AudioDuration { get; set; }
    }
}