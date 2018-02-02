using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Beyova.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class SentenceAnalyzer
    {
        /// <summary>
        /// The word separators
        /// </summary>
        static char[] wordSeparators = new char[] { ',', ';', ' ' };

        /// <summary>
        /// The quote signal
        /// </summary>
        const char quoteSignal = '"';

        /// <summary>
        /// The short term regex
        /// </summary>
        static Regex shortTermRegex = new Regex(@"[A-Z]([a-zA-Z0-9]*)\.", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The sentence end signal
        /// </summary>
        protected char[] _sentenceEndSignal;

        /// <summary>
        /// The sub sentence end signal
        /// </summary>
        protected char[] _subSentenceEndSignal;

        /// <summary>
        /// Initializes a new instance of the <see cref="SentenceAnalyzer" /> class.
        /// </summary>
        /// <param name="sentenceEndSignal">The sentence end signal.</param>
        /// <param name="subSentenceEndSignal">The sub sentence end signal.</param>
        public SentenceAnalyzer(char[] sentenceEndSignal = null, char[] subSentenceEndSignal = null)
        {
            _sentenceEndSignal = sentenceEndSignal.HasItem() ? sentenceEndSignal : new char[] { '.', '?', '!' };
            _subSentenceEndSignal = subSentenceEndSignal.HasItem() ? subSentenceEndSignal : new char[] { ',', ';', ':' };
        }

        /// <summary>
        /// Converts to tokens.
        /// </summary>
        /// <param name="paragragh">The paragragh.</param>
        /// <returns></returns>
        public GrammarToken[] ConvertToTokens(string paragragh)
        {
            var text = FixDoubleByteSignals(paragragh);
            text = FixWrapWord(text);

            return GetSentenceTokens(text);
        }

        #region Pre-do

        /// <summary>
        /// Fixes the double byte signals.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string FixDoubleByteSignals(string input)
        {
            return string.IsNullOrWhiteSpace(input) ? input : input.ReplaceByMap(
                new char[] { '。', '，', '“', '《', '》', '（', '）', '’', '’' },
                new char[] { '.', ',', '"', '"', '"', '(', ')', '\'', '\'' }
            );
        }

        /// <summary>
        /// The wrap word regex
        /// </summary>
        protected static Regex wrapWordRegex = new Regex(@"[a-zA-Z]\-[\s\n\r\t]+[a-zA-Z]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Fixes the wrap word.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string FixWrapWord(string input)
        {
            StringBuilder builder = new StringBuilder(input.Length);

            int lastStartIndex = 0;
            foreach (Match one in wrapWordRegex.Matches(input))
            {
                if (one.Success)
                {
                    builder.Append(input.Substring(lastStartIndex, one.Index - lastStartIndex + 1));
                    lastStartIndex = one.Index + one.Length - 1;
                }
            }

            builder.Append(input.Substring(lastStartIndex, input.Length - lastStartIndex));

            return builder.ToString();
        }

        #endregion

        /// <summary>
        /// The space chars out of sentence
        /// </summary>
        protected static char[] spaceCharsOutOfSentence = new char[] { '\n', '\r', '\t', ' ' };

        /// <summary>
        /// Gets the sentence token.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        protected GrammarToken[] GetSentenceTokens(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                List<GrammarToken> tokens = new List<GrammarToken>();

                for (var i = 0; i < text.Length;)
                {
                    if (text[i].IsInValues(spaceCharsOutOfSentence))
                    {
                        i++;
                    }
                    else
                    {
                        tokens.AddIfNotNull(GetSentenceToken(text, ref i));
                    }
                }

                return tokens.ToArray();
            }

            return null;
        }

        /// <summary>
        /// Gets the sentence token.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="index">The index.</param>
        /// <param name="isInQuote">if set to <c>true</c> [is in quote].</param>
        /// <returns></returns>
        protected GrammarToken GetSentenceToken(string text, ref int index, bool isInQuote = false)
        {
            if (!string.IsNullOrWhiteSpace(text) && index < text.Length)
            {
                var lastStartAlphabetIndex = index;
                var token = new GrammarToken(GrammarTokenType.Sentence);

                int startIndex = index;
                for (; index < text.Length; index++)
                {
                    if (text[index] == quoteSignal)
                    {
                        if (isInQuote)
                        {
                            // quit loop and get token.
                            break;
                        }
                        else
                        {
                            token.SubToken.Add(new GrammarToken(GrammarTokenType.Sentence) { RawTerm = text.Substring(startIndex, index - startIndex) });
                            index++;
                            token.SubToken.Add(GetSentenceToken(text, ref index, true));
                        }
                    }
                    else if (text[index].IsInValues(wordSeparators))
                    {
                        lastStartAlphabetIndex = index + 1;
                    }
                    else if (text[index].IsInValues(this._sentenceEndSignal))
                    {
                        //validate it is real end signal or not.
                        if (text[index] == '.')
                        {
                            if (shortTermRegex.IsMatch(text.Substring(lastStartAlphabetIndex, index - lastStartAlphabetIndex + 1)))
                            {
                                lastStartAlphabetIndex = index + 1;
                                continue;
                            }

                            // quit loop and get token.
                            break;
                        }
                    }
                }

                index++;
                token.RawTerm = text.Substring(startIndex, index - startIndex);

                AnalyzeSentenceToken(token);
                return token;
            }

            return null;
        }

        /// <summary>
        /// Analyzes the sentence token.
        /// </summary>
        /// <param name="token">The token.</param>
        protected static void AnalyzeSentenceToken(GrammarToken token)
        {
            if (token != null && token.Type == GrammarTokenType.Sentence)
            {

            }
        }


    }
}
