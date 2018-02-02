using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beyova.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public enum GrammarTokenType
    {
        /// <summary>
        /// The undefined
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The normal word. Like: bike
        /// </summary>
        NormalWord,
        /// <summary>
        /// The short term. Like: "Dr."
        /// </summary>
        ShortTerm,
        /// <summary>
        /// The double term. Lile "I'm"
        /// </summary>
        DoubleTerm,
        /// <summary>
        /// The quote term: Like "xxx"
        /// </summary>
        QuoteTerm,
        /// <summary>
        /// The sentence
        /// </summary>
        Sentence,
        /// <summary>
        /// The sentence seprator
        /// </summary>
        SentenceSeprator,
        /// <summary>
        /// The sub sentence seprator
        /// </summary>
        SubSentenceSeprator,

    }
}
