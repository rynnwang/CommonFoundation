using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.MicrosoftCognitiveService
{
    /// <summary>
    /// Class SpeechRecognizationResult.
    /// </summary>
    public class SpeechRecognizationResult
    {
        //TODO: 
        //Follow JSON schema in https://www.microsoft.com/cognitive-services/en-us/Speech-api/documentation/API-Reference-REST/BingVoiceRecognition
        //Example:
        //{
        //    "version": "3.0",
        //    "header": {
        //        "status": "success",
        //        "scenario": "websearch",
        //        "name": "Mc Dermant Autos",
        //        "lexical": "mac dermant autos",
        //        "properties": {
        //            "requestid": "ABDDD97E-171F-4B75-A491-A977027B0BC3"
        //        },
        //        "results": [{
        //            // Formatted result
        //            "name": "Mc Dermant Autos",
        //            // The text of what was spoken
        //            "lexical": "mac dermant autos",
        //            // Words that make up the result; a word can include a space if there
        //            // isn’t supposed to be a pause between words when speaking them
        //            "tokens": [{
        //                // The text in the grammar that matched what was spoken for this token
        //                "token": "mc dermant",
        //                // The text of what was spoken for this token
        //                "lexical": "mac dermant",
        //                // The IPA pronunciation of this token (I made up M AC DIR MANT; 
        //                // refer to a real IPA spec for the text of an actual pronunciation)
        //                "pronunciation": "M AC DIR MANT",
        //            },
        //              {
        //                  "token": "autos",
        //                  "lexical": "autos",
        //                  "pronunciation": "OW TOS",
        //              }],
        //            "properties": {
        //                "HIGHCONF": "1"
        //            },
        //        }],
        //    }
        //}

    }
}
