using System.Collections.Generic;

namespace Beyova.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class GrammarToken
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public GrammarTokenType Type { get; set; }

        /// <summary>
        /// Gets or sets the raw term.
        /// </summary>
        /// <value>
        /// The raw term.
        /// </value>
        public string RawTerm { get; set; }

        /// <summary>
        /// Gets or sets the sub token.
        /// </summary>
        /// <value>
        /// The sub token.
        /// </value>
        public List<GrammarToken> SubToken{ get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarToken"/> class.
        /// </summary>
        public GrammarToken() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarToken"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public GrammarToken(GrammarTokenType type) : this()
        {
            this.Type = type;
            this.SubToken = new List<GrammarToken>();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return RawTerm;
        }
    }
}
