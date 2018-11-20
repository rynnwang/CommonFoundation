using System;
using System.Collections.Generic;

namespace Beyova.Quiz
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TRealm">The type of the realm.</typeparam>
    public abstract class Question<TRealm> : Question
    {
        /// <summary>
        /// Gets or sets the realm.
        /// </summary>
        /// <value>
        /// The realm.
        /// </value>
        public TRealm Realm { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class Question : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>
        /// The body.
        /// </value>
        public VisualValue Body { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<Option> Options { get; set; }

        /// <summary>
        /// Gets or sets the correct option ids.
        /// </summary>
        /// <value>
        /// The correct option ids.
        /// </value>
        public List<string> CorrectOptionIds { get; set; }
    }
}