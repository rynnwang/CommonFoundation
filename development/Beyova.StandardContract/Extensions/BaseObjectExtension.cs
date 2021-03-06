﻿using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class BaseObjectExtension.
    /// </summary>
    public static class BaseObjectExtension
    {
        /// <summary>
        /// Determines whether the specified any object is removal.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.Boolean.</returns>
        private static bool IsRemoval(SimpleBaseObject anyObject)
        {
            return (anyObject?.State & ObjectState.Deleted) > 0;
        }

        /// <summary>
        /// To the entity synchronization response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseObjects">The base objects.</param>
        /// <param name="upsertsOnly">if set to <c>true</c> [upserts only].</param>
        /// <returns>EntitySynchronizationResponse&lt;T&gt;.</returns>
        public static EntitySynchronizationResponse<T> ToEntitySynchronizationResponse<T>(this IEnumerable<SimpleBaseObject<T>> baseObjects, bool upsertsOnly = false)
        {
            var result = new EntitySynchronizationResponse<T>();

            if (baseObjects.HasItem())
            {
                SimpleBaseObject<T> maxObject = null;
                Guid? lastKey = null;

                foreach (var one in baseObjects)
                {
                    if (IsRemoval(one))
                    {
                        if (!upsertsOnly)
                        {
                            result.Removals.Add(one.Key.ToString());
                        }
                    }
                    else
                    {
                        result.Upserts.Add(one.Object);
                    }

                    if (maxObject.Max(one, x => x.LastUpdatedStamp, out maxObject))
                    {
                        // Only when maxObject.LastUpdatedStamp == one.LastUpdatedStamp, maxObject would not be updated, but key needs to update.
                        // In other case, Key would always follow maxObject's key.
                        lastKey = maxObject.LastUpdatedStamp == one.LastUpdatedStamp ? one.Key : maxObject.Key;
                    }
                }

                result.LastStamp = maxObject.LastUpdatedStamp;
                result.LastKey = lastKey;
            }

            return result;
        }

        /// <summary>
        /// To the entity synchronization response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="baseObjects">The base objects.</param>
        /// <param name="upsertsOnly">The upserts only.</param>
        /// <returns>Beyova.EntitySynchronizationResponse&lt;T&gt;.</returns>
        public static EntitySynchronizationResponse<T> ToEntitySynchronizationResponse<T>(this IEnumerable<T> baseObjects, bool upsertsOnly = false)
                    where T : SimpleBaseObject
        {
            var result = new EntitySynchronizationResponse<T>();

            if (baseObjects.HasItem())
            {
                T maxObject = null;
                Guid? lastKey = null;

                foreach (var one in baseObjects)
                {
                    if (IsRemoval(one))
                    {
                        if (!upsertsOnly)
                        {
                            result.Removals.Add(one.Key.ToString());
                        }
                    }
                    else
                    {
                        result.Upserts.Add(one);
                    }

                    if (maxObject.Max(one, x => x.LastUpdatedStamp as DateTime?, out maxObject))
                    {
                        // Only when maxObject.LastUpdatedStamp == one.LastUpdatedStamp, maxObject would not be updated, but key needs to update.
                        // In other case, Key would always follow maxObject's key.
                        lastKey = maxObject.LastUpdatedStamp == one.LastUpdatedStamp ? one.Key : maxObject.Key;
                    }
                }

                result.LastStamp = maxObject.LastUpdatedStamp;
                result.LastKey = lastKey;
            }

            return result;
        }
    }
}