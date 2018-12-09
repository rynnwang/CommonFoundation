using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using Beyova;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Extension class for collection type object.
    /// </summary>
    public static partial class CollectionExtension
    {
        /// <summary>
        /// Ases the array with same value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static T[] AsArrayWithSameValue<T>(this T value, int count)
        {
            var array = new T[count];
            array.SetValueByRange(value);

            return array;
        }

        /// <summary>
        /// Sets the value by range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="value">The value.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        public static void SetValueByRange<T>(this T[] array, T value, int startIndex = 0, int? count = null)
        {
            if (array != null)
            {
                var c = 0;
                if (!count.HasValue || count.Value < 0)
                {
                    count = array.Length;
                }
                for (var i = (startIndex < 0 ? 0 : startIndex); i < array.Length && c < count; i++, c++)
                {
                    array[i] = value;
                }
            }
        }

        /// <summary>
        /// Filters the specified objects.
        /// </summary>
        /// <typeparam name="TOriginal">The type of the original.</typeparam>
        /// <typeparam name="TTarget">The type of the target.</typeparam>
        /// <param name="objects">The objects.</param>
        /// <returns></returns>
        public static ICollection<TTarget> AsCollection<TOriginal, TTarget>(this IEnumerable<TOriginal> objects)
            where TOriginal : class
            where TTarget : class
        {
            Collection<TTarget> result = new Collection<TTarget>();

            if (objects.HasItem())
            {
                foreach (var one in objects)
                {
                    result.AddIfNotNull(one as TTarget);
                }
            }

            return result;
        }

        #region Add If Not xxx

        /// <summary>
        /// Adds if not exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        public static void AddIfNotExists<T>(this ICollection<T> collection, T item, Func<T, T, bool> comparer = null)
        {
            if (collection != null && item != null)
            {
                var found = (from one in collection where (comparer != null ? (comparer(one, item)) : (item.Equals(one))) select one).Any();

                if (!found)
                {
                    collection.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if succeed to add, <c>false</c> otherwise.</returns>
        public static bool AddIfNotNullOrEmpty(this ICollection<string> collection, string item)
        {
            if (collection != null && !string.IsNullOrWhiteSpace(item))
            {
                collection.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if succeed to add, <c>false</c> otherwise.</returns>
        public static bool AddIfNotNull<T>(this ICollection<T> collection, Nullable<T> item) where T : struct
        {
            if (collection != null && item.HasValue)
            {
                collection.Add(item.Value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <returns><c>true</c> if succeed to add, <c>false</c> otherwise.</returns>
        public static bool AddIfNotNull<T>(this ICollection<T> collection, T item)
        {
            if (collection != null && item != null)
            {
                collection.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        public static void AddIfNotNull<T>(this HashSet<T> collection, Nullable<T> item) where T : struct
        {
            if (collection != null && item.HasValue)
            {
                collection.Add(item.Value);
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        public static void AddIfNotNull<T>(this HashSet<T> collection, T item)
        {
            if (collection != null && item != null)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNotNullOrEmpty(this NameValueCollection collection, string key, string value)
        {
            if (collection != null && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                collection.Add(key, value);
            }
        }

        /// <summary>
        /// Adds if not exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        public static void AddIfNotExists<T>(this HashSet<T> collection, T item, Func<T, T, bool> comparer = null)
        {
            if (collection != null && item != null)
            {
                var found = (from one in collection where (comparer != null ? (comparer(one, item)) : (item.Equals(one))) select one).Any();

                if (!found)
                {
                    collection.Add(item);
                }
            }
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary != null && key != null && value != null)
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Adds if not null or empty. Key and value would be added only when neither of them is null or empty.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public static void AddIfNotNullOrEmpty(this IDictionary<string, string> dictionary, string key, string value)
        {
            if (dictionary != null && !string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(value))
            {
                dictionary.Add(key, value);
            }
        }

        #endregion Add If Not xxx

        #region IEnumerable, ICollection, IList, IDictionary, HashSet

        /// <summary>
        /// Safes the contains.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool SafeContains<T>(this IEnumerable<T> collection, T value)
        {
            return collection.HasItem() ? collection.Contains(value) : false;
        }

        /// <summary>
        /// Gets the mapping value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mappingTable">The mapping table.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T GetMappingValue<T>(this MappingTable<T> mappingTable, string key, T defaultValue = default(T))
        {
            return mappingTable == null ? defaultValue : mappingTable.InternalGetMappingValue(key, defaultValue);
        }

        /// <summary>
        /// Gets the mapped value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mappingTable">The mapping table.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetMappedValue<T>(this MappingTable<T> mappingTable, T value, string defaultValue = null)
        {
            return mappingTable == null ? defaultValue : mappingTable.InternalGetMappedValue(value, defaultValue);
        }

        /// <summary>
        /// Gets the mapping value.
        /// </summary>
        /// <param name="mappingTable">The mapping table.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetMappingValue(this MappingTable mappingTable, string key)
        {
            return GetMappingValue(mappingTable, key, key);
        }

        /// <summary>
        /// Gets the mapped value.
        /// </summary>
        /// <param name="mappingTable">The mapping table.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetMappedValue(this MappingTable mappingTable, string key)
        {
            return GetMappedValue(mappingTable, key, key);
        }

        /// <summary>
        /// To the j array.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="eachObjectGenerateFunc">The each object generate function.</param>
        /// <returns>JArray.</returns>
        public static JArray ToJArray<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<TKey, TValue, JToken> eachObjectGenerateFunc)
        {
            JArray result = new JArray();
            if (dictionary.HasItem() && eachObjectGenerateFunc != null)
            {
                foreach (var one in dictionary)
                {
                    result.AddIfNotNull(eachObjectGenerateFunc(one.Key, one.Value));
                }
            }

            return result;
        }

        /// <summary>
        /// To the j array.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyPropertyName">Name of the key property.</param>
        /// <param name="valuePropertyName">Name of the value property.</param>
        /// <returns>JArray.</returns>
        public static JArray ToJArray<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string keyPropertyName, string valuePropertyName)
        {
            if (dictionary.HasItem() && !string.IsNullOrWhiteSpace(keyPropertyName) && !string.IsNullOrWhiteSpace(valuePropertyName))
            {
                if (keyPropertyName == valuePropertyName)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("keyPropertyName/valuePropertyName", new { keyPropertyName, valuePropertyName });
                }

                return ToJArray(dictionary, (k, v) =>
                {
                    var o = new JObject();
                    o.Add(keyPropertyName, JToken.FromObject(k));
                    o.Add(valuePropertyName, JToken.FromObject(v));

                    return o;
                });
            }

            return new JArray();
        }

        /// <summary>
        /// Merges to.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        public static void MergeTo<T>(this IEnumerable<T> source, IList<T> destination, bool overrideIfExists = false, IEqualityComparer<T> equalityComparer = null)
        {
            if (source.HasItem() && destination.HasItem())
            {
                if (equalityComparer == null)
                {
                    equalityComparer = EqualityComparer<T>.Default;
                }

                foreach (var one in source)
                {
                    var hasMatch = false;

                    for (var i = 0; i < destination.Count; i++)
                    {
                        if (equalityComparer.Equals(destination[i], one))
                        {
                            hasMatch = true;
                            if (overrideIfExists)
                            {
                                destination[i] = one;
                            }

                            break;
                        }
                    }

                    if (!hasMatch)
                    {
                        destination.Add(one);
                    }
                }
            }
        }

        /// <summary>
        /// Matches any. If any item of <c>source</c> matches any item of <c>hitSubjects</c>, return true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="hitSubjects">The hit subjects.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns><c>true</c> if exists any match between <c>source</c> and <c>hitSubjects</c>, <c>false</c> otherwise.</returns>
        public static bool MatchAny<T>(this IEnumerable<T> source, IEnumerable<T> hitSubjects, IEqualityComparer<T> equalityComparer = null)
        {
            if (source.HasItem() && hitSubjects.HasItem())
            {
                if (equalityComparer == null)
                {
                    equalityComparer = EqualityComparer<T>.Default;
                }

                foreach (var one in source)
                {
                    foreach (var item in hitSubjects)
                    {
                        if (equalityComparer.Equals(one, item))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Matches all.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="hitSubjects">The hit subjects.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns><c>true</c> if match all from <c>source</c> and <c>hitSubjects</c> (including <c>hitSubjects</c> has no item), <c>false</c> otherwise.</returns>
        public static bool MatchAll<T>(this IEnumerable<T> source, IEnumerable<T> hitSubjects, IEqualityComparer<T> equalityComparer = null)
        {
            if (source.HasItem())
            {
                if (!hitSubjects.HasItem())
                {
                    return true;
                }

                if (equalityComparer == null)
                {
                    equalityComparer = EqualityComparer<T>.Default;
                }

                foreach (var one in source)
                {
                    if (!hitSubjects.Contains(one, equalityComparer))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Subs the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>List&lt;T&gt;.</returns>
        /// <exception cref="InvalidObjectException">
        /// startIndex
        /// or
        /// count
        /// </exception>
        public static List<T> SubCollection<T>(this IList<T> collection, int startIndex, int? count = null)
        {
            if (collection != null)
            {
                if (startIndex < 0 || startIndex >= collection.Count)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("startIndex", data: new { startIndex, collectionCount = collection.Count });
                }

                if (count.HasValue && (count.Value < 0 || (count.Value + startIndex - 1) > collection.Count))
                {
                    throw ExceptionFactory.CreateInvalidObjectException("count", data: new { count, collectionCount = collection.Count });
                }

                var result = new List<T>();
                for (var i = startIndex; i < (count ?? collection.Count); i++)
                {
                    result.Add(collection[i]);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Subs the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static T[] SubArray<T>(this T[] array, int startIndex, int? count = null)
        {
            if (array != null)
            {
                if (startIndex < 0 || startIndex >= array.Length)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("startIndex", data: new { startIndex, arrayLength = array.Length });
                }

                var maxAllowedCount = array.Length - startIndex;

                if (count.HasValue && (count.Value < 0 || count.Value > maxAllowedCount))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(count), data: new { count, arrayLength = array.Length });
                }

                if (maxAllowedCount > 0)
                {
                    var result = new T[count ?? maxAllowedCount];
                    Array.Copy(array, startIndex, result, 0, result.Length);
                    return result;
                }
            }

            return default(T[]);
        }

        /// <summary>
        /// Values the equals.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="otherArray">The other array.</param>
        /// <returns></returns>
        public static bool ValueEquals<T>(this T[] array, T[] otherArray)
        {
            if (array == null || otherArray == null || array.Length != otherArray.Length)
            {
                return false;
            }

            for (var i = 0; i < array.Length; i++)
            {
                if (!array[i].SafeEquals(otherArray[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Subs the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static T[] SubArray<T>(this T[] array, long startIndex, long? count = null)
        {
            if (array != null)
            {
                if (startIndex < 0 || startIndex >= array.Length)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("startIndex", data: new { startIndex, collectionCount = array.LongLength });
                }

                var maxAllowedCount = array.Length - startIndex;

                if (count.HasValue && (count.Value < 0 || count.Value > maxAllowedCount))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(count), data: new { count, arrayLength = array.LongLength });
                }

                if (maxAllowedCount > 0)
                {
                    var result = new T[count ?? maxAllowedCount];
                    Array.Copy(array, startIndex, result, 0, result.Length);
                    return result;
                }
            }

            return default(T[]);
        }

        /// <summary>
        /// Reads the specified step count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="stepCount">The step count.</param>
        /// <param name="currentIndex">Index of the current.</param>
        /// <returns></returns>
        public static T[] Read<T>(this T[] array, int stepCount, ref int currentIndex)
        {
            if (array != null && stepCount > 0 && currentIndex >= 0)
            {
                var result = array.SubArray(currentIndex, stepCount);
                currentIndex += result.Length;
                return result;
            }

            return null;
        }

        /// <summary>
        /// Reads the specified step count.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="stepCount">The step count.</param>
        /// <param name="currentIndex">Index of the current.</param>
        /// <returns></returns>
        public static T[] Read<T>(this T[] array, long stepCount, ref long currentIndex)
        {
            if (array != null && stepCount > 1 && currentIndex >= 0)
            {
                var result = array.SubArray(currentIndex, stepCount);
                currentIndex += result.Length;
                return result;
            }

            return null;
        }

        /// <summary>
        /// Removes from.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="startIndex">The start index.</param>
        /// <exception cref="InvalidObjectException">startIndex</exception>
        public static void RemoveFrom<T>(this List<T> list, int startIndex)
        {
            if (list != null)
            {
                if (startIndex < 0 || startIndex >= list.Count)
                {
                    throw ExceptionFactory.CreateInvalidObjectException("startIndex", data: new { startIndex, collectionCount = list.Count });
                }

                list.RemoveRange(startIndex, list.Count - startIndex);
            }
        }

        /// <summary>
        /// Gets the by last index of. (index should >= 1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="lastIndex">The last index. (should >= 1)</param>
        /// <returns>T.</returns>
        public static T GetByLastIndexOf<T>(this IList<T> collection, int lastIndex)
        {
            return (collection != null && lastIndex > 0 && collection.Count >= lastIndex) ? collection[collection.Count - lastIndex] : default(T);
        }

        /// <summary>
        /// Gets the by index of. (index should >= 1)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="index">The index.</param>
        /// <returns>T.</returns>
        public static T GetByIndexOf<T>(this IList<T> collection, int index)
        {
            return (collection != null && index > 0 && collection.Count >= index) ? collection[index - 1] : default(T);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int IndexOf<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            if (collection.HasItem() && predicate != null)
            {
                int index = 0;
                foreach (var one in collection)
                {
                    if (predicate(one))
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="stringToFind">The string to find.</param>
        /// <param name="charComparer">The character comparer.</param>
        /// <param name="charactors">The charactors.</param>
        /// <returns></returns>
        public static int IndexOf(this string stringToFind, CharComparer charComparer, params char[] charactors)
        {
            if (stringToFind != null && charactors.HasItem())
            {
                int index = 0;
                foreach (var one in stringToFind)
                {
                    if (charactors.Contains(one, charComparer))
                    {
                        return index;
                    }

                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <param name="stringToFind">The string to find.</param>
        /// <param name="charactors">The charactors.</param>
        /// <returns></returns>
        public static int IndexOf(this string stringToFind, params char[] charactors)
        {
            return IndexOf(stringToFind, CharComparer.OrdinalIgnoreCase, charactors);
        }

        /// <summary>
        /// Indexes the of.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TFactor">The type of the factor.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="factorValue">The factor value.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public static int IndexOf<TEntity, TFactor>(this IEnumerable<TEntity> collection, TFactor factorValue, Func<TEntity, TFactor, bool> predicate)
        {
            if (collection.HasItem() && predicate != null)
            {
                int index = 0;
                foreach (var one in collection)
                {
                    if (predicate(one, factorValue))
                    {
                        return index;
                    }
                    index++;
                }
            }

            return -1;
        }

        /// <summary>
        /// Sorts the specified comparable selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparableSelector">The comparable selector.</param>
        /// <param name="isDescending">if set to <c>true</c> [is descending].</param>
        public static void Sort<T, TComparableType>(this List<T> list, Func<T, TComparableType> comparableSelector, bool isDescending = false)
            where TComparableType : IComparable
        {
            var comparer = new LambdaComparableComparer<T, TComparableType>(comparableSelector, isDescending);
            list?.Sort(comparer);
        }

        /// <summary></summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparableType"></typeparam>
        /// <param name="list"></param>
        /// <param name="comparableSelector"></param>
        /// <param name="isDescending"></param>
        public static void Sort<T, TComparableType>(this List<T> list, Func<T, TComparableType?> comparableSelector, bool isDescending = false)
            where TComparableType : struct, IComparable
        {
            var comparer = new NullableLambdaComparableComparer<T, TComparableType>(comparableSelector, isDescending);
            list?.Sort(comparer);
        }

        /// <summary>
        /// Sorts the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparableType">The type of the t comparable type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparableSelector">The comparable selector.</param>
        /// <param name="comparison">The comparison.</param>
        public static void Sort<T, TComparableType>(this List<T> list, Func<T, TComparableType> comparableSelector, Func<TComparableType, TComparableType, int> comparison)
        {
            if (list != null)
            {
                var comparer = new LambdaComparer<T, TComparableType>(comparableSelector, comparison);
                list.Sort(comparer);
            }
        }

        /// <summary>
        /// Sorts the specified comparable selector.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparableType">The type of the comparable type.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="comparableSelector">The comparable selector.</param>
        /// <param name="comparison">The comparison.</param>
        public static void Sort<T, TComparableType>(this List<T> list, Func<T, TComparableType?> comparableSelector, Func<TComparableType, TComparableType, int> comparison)
            where TComparableType : struct
        {
            if (list != null)
            {
                var comparer = new NullableLambdaComparer<T, TComparableType>(comparableSelector, comparison);
                list.Sort(comparer);
            }
        }

        /// <summary>
        /// Sorts as.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TSortFactor">The type of the sort factor.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="sortFactors">The sort factors.</param>
        /// <param name="getSortFactor">The get sort factor.</param>
        public static void SortAs<T, TSortFactor>(this List<T> list, List<TSortFactor> sortFactors, Func<T, TSortFactor> getSortFactor)
        {
            if (sortFactors.HasItem() && getSortFactor != null)
            {
                Sort(list, x => { return sortFactors.IndexOf(getSortFactor(x)); }, (a, b) => { return a.CompareTo(b); });
            }
        }

        /// <summary>
        /// Try to the replace with specific item. Replaced index would be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the t comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="itemToReplace">The item to replace.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>System.Nullable&lt;System.Int32&gt; for replaced index.</returns>
        public static int? TryReplace<T, TComparerIdentifier>(this IList<T> collection, T itemToReplace, TComparerIdentifier comparerIdentifier, Func<T, TComparerIdentifier, bool> comparer)
        {
            if (collection != null && comparer != null && itemToReplace != null)
            {
                for (var i = 0; i < collection.Count;)
                {
                    if (comparer(collection[i], comparerIdentifier))
                    {
                        var tmp = collection[i];
                        collection.RemoveAt(i);
                        collection.Insert(i, itemToReplace);
                        return i;
                    }

                    i++;
                }
            }

            return null;
        }

        /// <summary>
        /// Try to the replace all with specific item. Replaced count would be returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the t comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="itemToReplace">The item to replace.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>System.Int32.</returns>
        public static int TryReplaceAll<T, TComparerIdentifier>(this IList<T> collection, T itemToReplace, TComparerIdentifier comparerIdentifier, Func<T, TComparerIdentifier, bool> comparer)
        {
            int count = 0;
            if (collection != null && comparer != null && itemToReplace != null)
            {
                for (var i = 0; i < collection.Count; i++)
                {
                    if (comparer(collection[i], comparerIdentifier))
                    {
                        collection.RemoveAt(i);
                        collection.Insert(i, itemToReplace);
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>TValue.</returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            if (dictionary != null && key != null)
            {
                return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
            }

            return defaultValue;
        }

        /// <summary>
        /// To the hash set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items">The items.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>HashSet&lt;T&gt;.</returns>
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer = null)
        {
            items = items ?? new Collection<T>();

            return comparer == null ?
                new HashSet<T>(items)
                : new HashSet<T>(items, comparer);
        }

        /// <summary>
        /// To the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="items">The items.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> items, IEqualityComparer<TKey> comparer = null)
        {
            if (items != null)
            {
                var result = comparer == null ? new Dictionary<TKey, TValue>() : new Dictionary<TKey, TValue>(comparer);

                foreach (var one in items)
                {
                    result.Merge(one.Key, one.Value);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// SQLs the json to list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlJson">The SQL json.</param>
        /// <returns>System.Collections.Generic.List&lt;T&gt;.</returns>
        public static List<T> SqlJsonToList<T>(this object sqlJson)
        {
            return sqlJson?.ObjectToJToken()?.ToObject<List<T>>();
        }

        /// <summary>
        /// Converts SQL the json to simple list.
        /// Example: Input [{item:x},{item:y}] + "item", then get [x,y]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlJson">The SQL json.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>System.Collections.Generic.List&lt;T&gt;.</returns>
        public static List<T> SqlJsonToSimpleList<T>(this object sqlJson, string propertyName)
        {
            List<T> result = new List<T>();

            if (!string.IsNullOrWhiteSpace(propertyName) && sqlJson != null)
            {
                var items = sqlJson?.ObjectToJToken();
                if (items.HasItem())
                {
                    foreach (JObject one in items)
                    {
                        if (one != null)
                        {
                            result.AddIfNotNull(one.SafeGetValue<T>(propertyName));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// SQLs the json to dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="sqlJson">The SQL json.</param>
        /// <param name="keyPropertyName">Name of the key property.</param>
        /// <param name="valuePropertyName">Name of the value property.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <param name="useMerge">if set to <c>true</c> [use merge].</param>
        /// <returns>Dictionary&lt;TKey, TValue&gt;.</returns>
        public static Dictionary<TKey, TValue> SqlJsonToDictionary<TKey, TValue>(this object sqlJson, string keyPropertyName, string valuePropertyName, IEqualityComparer<TKey> equalityComparer = null, bool useMerge = false)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(equalityComparer ?? EqualityComparer<TKey>.Default);

            if (!string.IsNullOrWhiteSpace(keyPropertyName) && !string.IsNullOrWhiteSpace(valuePropertyName) && sqlJson != null)
            {
                var items = sqlJson.ObjectToJToken();
                if (items.HasItem())
                {
                    foreach (JObject one in items)
                    {
                        if (one != null)
                        {
                            if (useMerge)
                            {
                                result.Merge(one.SafeGetValue<TKey>(keyPropertyName), one.SafeGetValue<TValue>(valuePropertyName));
                            }
                            else
                            {
                                result.Add(one.SafeGetValue<TKey>(keyPropertyName), one.SafeGetValue<TValue>(valuePropertyName));
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Joins the within format. In format value, {0} would be used for index, {1} would be used as value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="format">The format. {0} would be used for index, {1} would be used as value.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns>
        /// System.String.
        /// </returns>
        public static string JoinWithinFormat<T>(this IEnumerable<T> instance, string format, string seperator = null)
        {
            return (instance.HasItem() && !string.IsNullOrWhiteSpace(format)) ? InternalJoin(instance, (item, index) => { return string.Format(format, index, item); }, seperator) : string.Empty; ;
        }

        /// <summary>
        /// Joins the specified item predict.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="itemPredict">The item predict.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        public static string Join<T>(this IEnumerable<T> instance, Func<T, string> itemPredict = null, string seperator = null)
        {
            if (itemPredict == null)
            {
                itemPredict = DefaultJoinLambda;
            }

            return InternalJoin(instance, (item, index) => { return itemPredict(item); }, seperator);
        }

        private static string DefaultJoinLambda<T>(T item)
        {
            return item?.ToString();
        }

        /// <summary>
        /// Internals the join.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="itemPredict">The item predict.</param>
        /// <param name="seperator">The seperator.</param>
        /// <returns></returns>
        private static string InternalJoin<T>(this IEnumerable<T> instance, Func<T, int, string> itemPredict, string seperator = null)
        {
            if (instance.HasItem() && itemPredict != null)
            {
                if (instance.Count() == 1)
                {
                    return itemPredict(instance.First(), 0);
                }

                var builder = new StringBuilder((instance?.Count() ?? 0) * 64);

                seperator = string.IsNullOrEmpty(seperator) ? StringConstants.Comma : seperator;

                if (instance != null)
                {
                    int index = 0;
                    foreach (var one in instance)
                    {
                        builder.Append(itemPredict(one, index));
                        builder.Append(seperator);
                        index++;
                    }
                }

                builder.RemoveLastIfMatch(seperator);

                return builder.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Determines whether the specified instance has item.
        /// <remarks><c>Instance</c> can be null. Return true only when instance is not null and has item.</remarks>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified instance has item; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasItem(this IEnumerable source)
        {
            return source?.GetEnumerator()?.MoveNext() ?? false;
        }

        /// <summary>
        /// Determines whether the specified instance has item.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="objToMatch">The object to match.</param>
        /// <returns>System.Boolean.</returns>
        public static bool HasItem<T>(this IList<T> instance, T objToMatch)
        {
            return instance != null && instance.IndexOf(objToMatch) > -1;
        }

        #endregion IEnumerable, ICollection, IList, IDictionary, HashSet

        /// <summary>
        /// This method would try to convert each item of collection to from <c>TInput</c> to <c>TOutput</c> by keyword <c>as</c>. If by any reason to get null as result, it would not be included in result collection.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static ICollection<TOutput> AsCovariance<TInput, TOutput>(this IEnumerable<TInput> collection)
            where TInput : class
            where TOutput : class, TInput
        {
            return AsNotNullAll(collection, x => x as TOutput);
        }

        /// <summary>
        /// As all which is not null.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        public static List<TOutput> AsNotNullAll<TInput, TOutput>(this IEnumerable<TInput> collection, Func<TInput, TOutput> converter)
        {
            if (collection != null && converter != null)
            {
                List<TOutput> result = new List<TOutput>(collection.Count());

                foreach (var one in collection)
                {
                    result.AddIfNotNull(converter(one));
                }

                return result;
            }

            return new List<TOutput>();
        }

        #region AsDictionary

        /// <summary>
        /// Ases the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="key">The key.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>
        /// Dictionary&lt;TKey, TValue&gt;.
        /// </returns>
        public static Dictionary<TKey, TValue> AsDictionary<TKey, TValue>(this TValue anyObject, TKey key, IEqualityComparer<TKey> equalityComparer = null)
        {
            return anyObject == null ? new Dictionary<TKey, TValue>() : new Dictionary<TKey, TValue>(equalityComparer ?? EqualityComparer<TKey>.Default) { { key, anyObject } };
        }

        /// <summary>
        /// As dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="initializer">The initializer.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AsKeyDictionary<TKey, TValue>(this IEnumerable<TKey> collection, Func<TKey, TValue> initializer = null, IEqualityComparer<TKey> equalityComparer = null)
        {
            return AsDictionary<TKey, TKey, TValue>(collection, FuncExtension.GetSelf, initializer, equalityComparer);
        }

        /// <summary>
        /// Ases the dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="initializer">The initializer.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AsKeyDictionary<TKey, TValue>(this IEnumerable<TKey> collection, Func<TValue> initializer = null, IEqualityComparer<TKey> equalityComparer = null)
        {
            return AsDictionary<TKey, TKey, TValue>(collection, FuncExtension.GetSelf, initializer.ExtendAsInputless<TKey, TValue>(), equalityComparer);
        }

        /// <summary>
        /// Ases the value dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="initializer">The initializer.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AsValueDictionary<TKey, TValue>(this IEnumerable<TValue> collection, Func<TValue, TKey> initializer = null, IEqualityComparer<TKey> equalityComparer = null)
        {
            return AsDictionary<TValue, TKey, TValue>(collection, initializer, FuncExtension.GetSelf, equalityComparer);
        }

        /// <summary>
        /// Ases the dictionary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="keyInitializer">The key initializer.</param>
        /// <param name="valueInitializer">The value initializer.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> AsDictionary<T, TKey, TValue>(this IEnumerable<T> collection, Func<T, TKey> keyInitializer, Func<T, TValue> valueInitializer, IEqualityComparer<TKey> equalityComparer = null)
        {
            if (collection != null)
            {
                Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>(collection.Count(), equalityComparer ?? EqualityComparer<TKey>.Default);

                foreach (var one in collection)
                {
                    result.Add(keyInitializer == null ? default(TKey) : keyInitializer.Invoke(one), valueInitializer == null ? default(TValue) : valueInitializer.Invoke(one));
                }

                return result;
            }

            return null;
        }

        #endregion AsDictionary

        /// <summary>
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="keyValueString">The key value string.</param>
        public static void SetValuesByKeyValueString(this NameValueCollection dictionary, string keyValueString)
        {
            if (dictionary == null || string.IsNullOrWhiteSpace(keyValueString))
            {
                return;
            }

            var webDictionary = keyValueString.ParseToNameValueCollection();
            foreach (string key in dictionary.Keys)
            {
                dictionary.Add(key, webDictionary[key]);
            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified any string].
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">The ignore case.</param>
        /// <returns>System.Boolean.</returns>
        public static bool Contains(this string anyString, char value, bool ignoreCase = false)
        {
            return anyString.ToCharArray().Contains(value, ignoreCase ? CharComparer.OrdinalIgnoreCase : CharComparer.Ordinal);
        }

        /// <summary>
        /// Safes the contains.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static bool SafeContains(this string anyString, char value, bool ignoreCase = false)
        {
            return anyString.HasItem() ? anyString.ToCharArray().Contains(value, ignoreCase ? CharComparer.OrdinalIgnoreCase : CharComparer.Ordinal) : false;
        }

        /// <summary>
        /// Determines whether [contains] [the specified string array].
        /// </summary>
        /// <param name="stringCollection">The string array.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified string array]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this IEnumerable<string> stringCollection, string value, bool ignoreCase)
        {
            return stringCollection.Contains(value, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
        }

        /// <summary>
        /// Safes the contains.
        /// </summary>
        /// <param name="stringCollection">The string collection.</param>
        /// <param name="value">The value.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        public static bool SafeContains(this IEnumerable<string> stringCollection, string value, bool ignoreCase)
        {
            return stringCollection.HasItem() ? stringCollection.Contains(value, ignoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal) : false;
        }

        /// <summary>
        /// Determines whether the specified predicate has item.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///   <c>true</c> if the specified predicate has item; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasItem<TEntity>(this IEnumerable<TEntity> collection, Func<TEntity, bool> predicate)
        {
            return collection != null && predicate != null && collection.Any(predicate);
        }

        /// <summary>
        /// Determines whether [contains] [the specified collection]. Rules: exists any item where equalityComparer.Equals(selector(x), factor) == true
        /// </summary>
        /// <typeparam name="TEntity">The type of the t entity.</typeparam>
        /// <typeparam name="TFactor">The type of the t factor.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>System.Boolean.</returns>
        public static bool HasItem<TEntity, TFactor>(this IEnumerable<TEntity> collection, TFactor factor, Func<TEntity, TFactor> selector, IEqualityComparer<TFactor> equalityComparer = null)
        {
            return (collection.HasItem() && selector != null)
                && collection.Any(x =>
             {
                 return (equalityComparer ?? EqualityComparer<TFactor>.Default).Equals(selector(x), factor);
             });
        }

        /// <summary>
        /// Determines whether [contains] [the specified collection].
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TFactor">The type of the factor.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified collection]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains<TEntity, TFactor>(this IEnumerable<TEntity> collection, TFactor factor, Func<TEntity, TFactor> selector, Func<TFactor, TFactor, bool> equalityComparer)
        {
            return (collection.HasItem() && selector != null)
                && collection.Any(x =>
                {
                    return (equalityComparer ?? EqualityComparer<TFactor>.Default.Equals)(factor, selector(x));
                });
        }

        /// <summary>
        /// Determines whether [contains] [the specified dictionary].
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <typeparam name="TFactor">The type of the t factor.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="factor">The factor.</param>
        /// <param name="selector">The selector.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns>System.Boolean.</returns>
        public static bool Contains<TKey, TValue, TFactor>(this IDictionary<TKey, TValue> dictionary, TFactor factor, Func<TValue, TFactor> selector, IEqualityComparer<TFactor> equalityComparer = null)
        {
            return (dictionary.HasItem() && selector != null)
                && dictionary.Any(x =>
                {
                    return (equalityComparer ?? EqualityComparer<TFactor>.Default).Equals(selector(x.Value), factor);
                });
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="cookieCollection">The cookie collection.</param>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string TryGetValue(this CookieCollection cookieCollection, string key)
        {
            string result = null;

            if (cookieCollection != null && !string.IsNullOrWhiteSpace(key))
            {
                var cookie = cookieCollection[key];

                if (cookie != null)
                {
                    result = cookie.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Safe to get first or default. If instance is null, return default(T);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>T.</returns>
        public static T SafeFirstOrDefault<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? default(T) : enumerable.FirstOrDefault();
        }

        /// <summary>
        /// Safes the first or default value.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <returns>TValue.</returns>
        public static TValue SafeFirstOrDefaultValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return (dictionary != null && dictionary.HasItem()) ? dictionary.First().Value : default(TValue);
        }

        /// <summary>
        /// Firsts the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>T.</returns>
        public static T FirstOrDefault<T>(this Array array)
        {
            return array.Cast<T>().FirstOrDefault();
        }

        /// <summary>
        /// Safes the first or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>T.</returns>
        public static T SafeFirstOrDefault<T>(this Array array)
        {
            return array == null ? default(T) : array.FirstOrDefault<T>();
        }

        /// <summary>
        /// Safes the first or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <returns>T.</returns>
        public static T SafeFirstOrDefault<T>(this T[] array)
        {
            return array == null ? default(T) : array.FirstOrDefault<T>();
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> AsList<T>(this T anyObject)
        {
            return anyObject != null ? new List<T>() { anyObject } : new List<T>();
        }

        /// <summary>
        /// Ases the collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>Collection&lt;T&gt;.</returns>
        public static Collection<T> AsCollection<T>(this T anyObject)
        {
            return anyObject != null ? new Collection<T> { anyObject } : new Collection<T>();
        }

        /// <summary>
        /// Ases the hash set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>System.Collections.Generic.HashSet&lt;T&gt;.</returns>
        public static HashSet<T> AsHashSet<T>(this T anyObject)
        {
            return anyObject != null ? new HashSet<T> { anyObject } : new HashSet<T>();
        }

        /// <summary>
        /// Ases the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns>T[].</returns>
        public static T[] AsArray<T>(this T anyObject)
        {
            return anyObject != null ? new T[] { anyObject } : new T[] { };
        }



        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection != null && items.HasItem())
            {
                foreach (var one in items)
                {
                    collection.Add(one);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="items">The items.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> items, bool overrideIfExists = false)
        {
            if (dictionary != null && items != null)
            {
                foreach (var one in items)
                {
                    dictionary.Merge(one.Key, one.Value, overrideIfExists);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange(this NameValueCollection nameValueCollection, NameValueCollection items)
        {
            if (nameValueCollection != null && items != null)
            {
                foreach (var key in items.AllKeys)
                {
                    nameValueCollection.Set(key, items.Get(key));
                }
            }
        }

        /// <summary>
        /// Merges the specified name value collection.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        public static void Merge(this NameValueCollection nameValueCollection, string key, string value, bool overrideIfExists = false)
        {
            if (nameValueCollection != null && !string.IsNullOrWhiteSpace(key))
            {
                if (!nameValueCollection.AllKeys.Contains(key))
                {
                    nameValueCollection.Add(key, value);
                }
                else if (overrideIfExists)
                {
                    nameValueCollection.Set(key, value);
                }
            }
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="items">The items.</param>
        public static void AddRange(this NameValueCollection nameValueCollection, IDictionary<string, string> items)
        {
            if (nameValueCollection != null && items != null)
            {
                foreach (var one in items)
                {
                    nameValueCollection.Add(one.Key, one.Value);
                }
            }
        }

        /// <summary>
        /// Unions the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="allowDuplicated">if set to <c>true</c> [allow duplicated].</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static ICollection<T> Union<T>(this IEnumerable<T> item1, IEnumerable<T> item2, bool allowDuplicated = false, IEqualityComparer<T> equalityComparer = null)
        {
            var container = allowDuplicated ? new List<T>(item1?.Count() ?? 0 + item2?.Count() ?? 0) : new HashSet<T>(equalityComparer) as ICollection<T>;

            container.AddRange(item1);
            container.AddRange(item2);

            return container;
        }

        /// <summary>
        /// Unions the specified items.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The item.</param>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public static T[] Union<T>(this T[] item, params T[] items)
        {
            T[] result = new T[(item?.Length ?? 0) + (items?.Length ?? 0)];

            if (item.HasItem())
            {
                Array.Copy(item, 0, result, 0, item.Length);
            }

            Array.Copy(items, 0, result, item?.Length ?? 0, items.Length);
            return result;
        }

        /// <summary>
        /// Finds the first match and remove it from list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the t comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns>T.</returns>
        public static T FindAndRemove<T, TComparerIdentifier>(this List<T> collection, TComparerIdentifier comparerIdentifier, Func<T, TComparerIdentifier, bool> comparer)
        {
            // Can NOT use IList<T>, because Array is IList<T> too, but it does not support remove at.
            if (collection != null && comparer != null)
            {
                for (var i = 0; i < collection.Count; i++)
                {
                    if (comparer(collection[i], comparerIdentifier))
                    {
                        var tmp = collection[i];
                        collection.RemoveAt(i);
                        return tmp;
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Finds the and remove.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="predict">The predict.</param>
        /// <returns></returns>
        public static T FindAndRemove<T>(this List<T> collection, Func<T, bool> predict)
        {
            // Can NOT use IList<T>, because Array is IList<T> too, but it does not support remove at.
            if (collection != null && predict != null)
            {
                for (var i = 0; i < collection.Count; i++)
                {
                    if (predict(collection[i]))
                    {
                        var tmp = collection[i];
                        collection.RemoveAt(i);
                        return tmp;
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Finds the and remove.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public static KeyValuePair<TKey, TValue>? FindAndRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> selector)
        {
            KeyValuePair<TKey, TValue>? matchedItem = null;

            if (dictionary != null && selector != null)
            {
                foreach (var one in dictionary)
                {
                    if (selector(one))
                    {
                        matchedItem = one;
                        break;
                    }
                }

                if (matchedItem.HasValue)
                {
                    dictionary.Remove(matchedItem.Value);
                }
            }

            return matchedItem;
        }

        /// <summary>
        /// Removes the specified selector.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="selector">The selector.</param>
        public static void Remove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> selector)
        {
            FindAndRemove(dictionary, selector);
        }

        /// <summary>
        /// Finds the and remove.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static T FindAndRemove<T, TComparerIdentifier>(this List<T> collection, Func<T, TComparerIdentifier> converter, TComparerIdentifier comparerIdentifier, IEqualityComparer<TComparerIdentifier> comparer)
        {
            // Can NOT use IList<T>, because Array is IList<T> too, but it does not support remove at.
            if (collection != null && comparer != null && converter != null)
            {
                for (var i = 0; i < collection.Count; i++)
                {
                    if (comparer.Equals(converter(collection[i]), comparerIdentifier))
                    {
                        var tmp = collection[i];
                        collection.RemoveAt(i);
                        return tmp;
                    }
                }
            }

            return default(T);
        }

        /// <summary>
        /// Safes the get.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static T SafeGet<T>(this T[] array, int index)
        {
            return (array.HasItem() && index > -1 && array.Length > index) ? array[index] : default(T);
        }

        /// <summary>
        /// Finds the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="matchedObject">The matched object.</param>
        /// <returns></returns>
        public static bool Find<T, TComparerIdentifier>(this IEnumerable<T> collection, Func<T, TComparerIdentifier> converter, TComparerIdentifier comparerIdentifier, IEqualityComparer<TComparerIdentifier> comparer, out T matchedObject)
        {
            if (collection != null && comparer != null && converter != null)
            {
                foreach (var one in collection)
                {
                    if (comparer.Equals(converter(one), comparerIdentifier))
                    {
                        matchedObject = one;
                        return true;
                    }
                }
            }

            matchedObject = default(T);
            return false;
        }

        /// <summary>
        /// Finds the specified collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TComparerIdentifier">The type of the comparer identifier.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="comparerIdentifier">The comparer identifier.</param>
        /// <param name="comparer">The comparer.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static T Find<T, TComparerIdentifier>(this IEnumerable<T> collection, Func<T, TComparerIdentifier> converter, TComparerIdentifier comparerIdentifier, IEqualityComparer<TComparerIdentifier> comparer, T defaultValue = default(T))
        {
            T result;
            return Find(collection, converter, comparerIdentifier, comparer, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Safes the peek.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public static T SafePeek<T>(this Stack<T> stack)
        {
            return (stack != null && stack.Count > 0) ? stack.Peek() : default(T);
        }

        /// <summary>
        /// Safes the pop.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stack">The stack.</param>
        /// <returns></returns>
        public static T SafePop<T>(this Stack<T> stack)
        {
            return (stack != null && stack.Count > 0) ? stack.Pop() : default(T);
        }

        #region Dictionary Extensions

        /// <summary>
        /// Safe to get value. Only when instance is not null and key is contained, return value. Otherwise return default(T).
        /// </summary>
        /// <typeparam name="TKey">T of the key.</typeparam>
        /// <typeparam name="TValue">T of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static TValue SafeGetValue<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue defaultValue = default(TValue))
        {
            return (instance != null && key != null && instance.ContainsKey(key)) ? instance[key] : defaultValue;
        }

        /// <summary>
        /// Gets the or create.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="valueForCreate">The value for create.</param>
        /// <returns>TValue.</returns>
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue valueForCreate = default(TValue))
        {
            if (instance != null && key != null)
            {
                TValue result;
                if (!instance.TryGetValue(key, out result))
                {
                    instance.Add(key, valueForCreate);
                    result = valueForCreate;
                }

                return result;
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keyValueCollection">The key value collection.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        public static void Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, IEnumerable<KeyValuePair<TKey, TValue>> keyValueCollection, bool overrideIfExists = false)
        {
            if (instance != null && keyValueCollection != null)
            {
                if (keyValueCollection.Any())
                {
                    foreach (var one in keyValueCollection)
                    {
                        var key = one.Key;
                        var value = one.Value;

                        if (key != null)
                        {
                            instance.Merge(key, value, overrideIfExists);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads the specified instance.
        /// </summary>
        /// <typeparam name="TKey">T of the key.</typeparam>
        /// <typeparam name="TValue">T of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keyValueCollection">The key value collection.</param>
        public static void Load<TKey, TValue>(this Dictionary<TKey, TValue> instance, ICollection<KeyValuePair<TKey, TValue>> keyValueCollection)
        {
            if (instance != null)
            {
                instance.Clear();
                instance.Merge(keyValueCollection);
            }
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        /// <returns><c>true</c> if value is inserted, <c>false</c> otherwise.</returns>
        public static bool Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value, bool overrideIfExists = true)
        {
            if (instance != null && key != null)
            {
                if (instance.ContainsKey(key))
                {
                    if (overrideIfExists)
                    {
                        instance[key] = value;
                    }
                }
                else
                {
                    instance.Add(key, value);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Merges the specified key.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="determineOverride">The determine override.</param>
        /// <returns><c>true</c> if it is added, <c>false</c> otherwise.</returns>
        public static bool Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, TValue value, Func<TKey, TValue, TValue, bool> determineOverride)
        {
            if (instance != null && key != null)
            {
                if (determineOverride == null)
                {
                    determineOverride = (k, v1, v2) => { return v1 == null; };
                }

                if (instance.ContainsKey(key))
                {
                    if (determineOverride(key, instance[key], value))
                    {
                        instance[key] = value;
                    }
                }
                else
                {
                    instance.Add(key, value);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Merges the specified instance.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="mergeObject">The merge object.</param>
        /// <param name="overrideIfExists">if set to <c>true</c> [override if exists].</param>
        /// <returns>System.Int32.</returns>
        public static int Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, IDictionary<TKey, TValue> mergeObject, bool overrideIfExists = true)
        {
            int total = 0;
            if (instance != null && mergeObject != null)
            {
                foreach (var key in mergeObject.Keys)
                {
                    if (instance.Merge(key, mergeObject[key], overrideIfExists))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// Merges the specified merge object.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="mergeObject">The merge object.</param>
        /// <param name="determineOverride">The determine override.</param>
        /// <returns>System.Int32.</returns>
        public static int Merge<TKey, TValue>(this IDictionary<TKey, TValue> instance, IDictionary<TKey, TValue> mergeObject, Func<TKey, TValue, TValue, bool> determineOverride)
        {
            int total = 0;
            if (instance != null && mergeObject != null)
            {
                if (determineOverride == null)
                {
                    determineOverride = (k, v1, v2) => { return v1 == null; };
                }

                foreach (var key in mergeObject.Keys)
                {
                    if (instance.Merge(key, mergeObject[key], determineOverride))
                    {
                        total++;
                    }
                }
            }

            return total;
        }

        /// <summary>
        /// Safes the try get value.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if succeed to get value, <c>false</c> otherwise.</returns>
        public static bool SafeTryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key, out TValue value)
        {
            value = default(TValue);
            return key != null && instance != null && instance.TryGetValue(key, out value);
        }

        /// <summary>
        /// Safes the try get key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool SafeTryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> instance, TValue value, IEqualityComparer<TValue> equalityComparer, out TKey key)
        {
            key = default(TKey);

            if (value != null && instance != null)
            {
                equalityComparer = equalityComparer ?? EqualityComparer<TValue>.Default;

                foreach (var one in instance)
                {
                    if (equalityComparer.Equals(one.Value, value))
                    {
                        key = one.Key;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Safes the try get key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="defaultKey">The default key.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static TKey SafeTryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> instance, TValue value, TKey defaultKey, IEqualityComparer<TValue> equalityComparer = null)
        {
            TKey key;
            return SafeTryGetKey(instance, value, equalityComparer, out key) ? key : defaultKey;
        }

        /// <summary>
        /// Safes the try get key.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static bool SafeTryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> instance, TValue value, out TKey key)
        {
            return SafeTryGetKey(instance, value, null, out key);
        }

        /// <summary>
        /// To the key value pair string.
        /// </summary>
        /// <typeparam name="TKey">The type of the t key.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="separatorChar">The separator character.</param>
        /// <param name="encodeKeyValue">if set to <c>true</c> [encode key value].</param>
        /// <returns>System.String.</returns>
        public static string ToKeyValuePairString<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, char separatorChar = '&', bool encodeKeyValue = false)
        {
            string format = "{0}={1}" + separatorChar;

            var builder = new StringBuilder(64);

            if (dictionary != null)
            {
                foreach (var one in dictionary)
                {
                    builder.AppendFormat(format, encodeKeyValue ?
                        one.Key.ToString().ToUrlPathEncodedText() : one.Key.ToString(),
                        encodeKeyValue ? one.Value.ToString().ToUrlPathEncodedText() : one.Value.ToString());
                }
            }

            return builder.ToString().TrimEnd(separatorChar);
        }

        #endregion Dictionary Extensions

        /// <summary>
        /// Holds flat items into specified container. NOTE: <c>container</c> would add <c>source</c> in this method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container">The container. It can be List or HashSet, based on case.</param>
        /// <param name="source">The source.</param>
        /// <param name="getChildren">The get children.</param>
        public static void HoldFlatItems<T>(this ICollection<T> container, T source, Func<T, IEnumerable<T>> getChildren)
        {
            if (container != null && source != null && getChildren != null)
            {
                container.Add(source);

                var children = getChildren(source);
                if (children.HasItem())
                {
                    foreach (var item in children)
                    {
                        HoldFlatItems(container, item, getChildren);
                    }
                }
            }
        }
    }
}