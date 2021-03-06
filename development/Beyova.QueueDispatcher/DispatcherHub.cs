﻿using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Beyova.QueueDispatch
{
    /// <summary>
    ///
    /// </summary>
    public static class DispatcherHub
    {
        /// <summary>
        /// The dispatchers
        /// </summary>
        private static HashSet<IQueueDispatcher> dispatchers = new HashSet<IQueueDispatcher>();

        /// <summary>
        /// Registers the specified dispatcher.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        internal static void Register(IQueueDispatcher dispatcher)
        {
            dispatchers.AddIfNotNull(dispatcher);
        }

        /// <summary>
        /// Gets the dispatchers.
        /// </summary>
        /// <value>
        /// The dispatchers.
        /// </value>
        public static Dictionary<string, JToken> Dispatchers
        {
            get
            {
                var result = new Dictionary<string, JToken>();

                foreach (var item in dispatchers)
                {
                    result.Add(item.GetType().ToString(), JToken.FromObject(item.Options));
                }

                return result;
            }
        }
    }
}