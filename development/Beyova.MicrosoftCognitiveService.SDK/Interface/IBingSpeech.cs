using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.MicrosoftCognitiveService
{
    public interface IBingSpeech
    {
        byte[] TextToSpeech(SSMLObject ssmlObject);

        List<SpeechTimeline> GetSpeechTimeLine(byte[] ssmlObject);

        SpeechRecognizationResult RecognizeSpeech(byte[] speechBytes, string cultureCode);
    }
}
