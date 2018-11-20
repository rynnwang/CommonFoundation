using System;
using System.Collections.Generic;

namespace Beyova.Quiz
{
    /// <summary>
    ///
    /// </summary>
    public class UserAnswer : UserAnswer<Guid?>
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TUserKey">The type of the user key.</typeparam>
    /// <typeparam name="TRealm">The type of the realm.</typeparam>
    public abstract class UserAnswer<TUserKey, TRealm> : UserAnswer<TUserKey>
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
    /// <typeparam name="TUserKey">The type of the user key.</typeparam>
    public abstract class UserAnswer<TUserKey>
    {
        /// <summary>
        /// Gets or sets the user key.
        /// </summary>
        /// <value>
        /// The user key.
        /// </value>
        public TUserKey UserKey { get; set; }

        /// <summary>
        /// Gets or sets the question key.
        /// </summary>
        /// <value>
        /// The question key.
        /// </value>
        public Guid? QuestionKey { get; set; }

        /// <summary>
        /// Gets or sets the selected option ids.
        /// </summary>
        /// <value>
        /// The selected option ids.
        /// </value>
        public List<string> SelectedOptionIds { get; set; }

        /// <summary>
        /// Gets or sets the total weight.
        /// </summary>
        /// <value>
        /// The total weight.
        /// </value>
        public double TotalWeight { get; set; }
    }
}