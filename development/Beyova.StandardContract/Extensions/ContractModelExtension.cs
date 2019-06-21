using Beyova.Diagnostic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// Class ContractModelExtension.
    /// </summary>
    public static partial class ContractModelExtension
    {
        #region Tree

        /// <summary>
        /// Builds the tree hierarchy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">The nodes.</param>
        /// <param name="root">The root.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>T.</returns>
        public static T BuildTreeHierarchy<T>(this List<T> nodes, Guid root, bool throwException = false) where T : ITreeNode<T>
        {
            try
            {
                nodes.CheckNullObject(nameof(nodes));

                var topNode = nodes.FindAndRemove(root, (item, key) =>
                {
                    return item.Key == key;
                });

                if (topNode != null)
                {
                    var groups = nodes.GroupBy(x => x.ParentNodeKey);
                    var nodeDictionary = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());

                    HashSet<Guid> handledKeys = new HashSet<Guid>();
                    AddChildren(topNode, nodeDictionary, handledKeys, throwException);

                    return topNode;
                }

                return default(T);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { nodes, root, type = typeof(T).FullName });
            }
        }

        /// <summary>
        /// Adds the children.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">The node.</param>
        /// <param name="source">The source.</param>
        /// <param name="handledKeys">The handled keys.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <exception cref="DataConflictException"></exception>
        private static void AddChildren<T>(T node, IDictionary<Guid, List<T>> source, HashSet<Guid> handledKeys, bool throwException = false) where T : ITreeNode<T>
        {
            if (handledKeys != null && node.Key.HasValue && source.ContainsKey(node.Key.Value))
            {
                if (handledKeys.Contains(node.Key.Value))
                {
                    if (throwException)
                    {
                        throw new DataConflictException(typeof(T).FullName, node.Key.ToString());
                    }

                    return;
                }

                node.Children = source[node.Key.Value];
                handledKeys.Add(node.Key.Value);

                for (int i = 0; i < node.Children.Count; i++)
                {
                    AddChildren(node.Children[i], source, handledKeys, throwException);
                }
            }
            else
            {
                node.Children = new List<T>();
            }
        }

        /// <summary>
        /// Descendant the specified root.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="root">The root.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        public static IEnumerable<T> Descendant<T>(this T root) where T : ITreeNode<T>
        {
            var nodes = new Stack<T>(new[] { root });
            while (nodes.Any())
            {
                T node = nodes.Pop();
                yield return node;
                foreach (var n in node.Children)
                {
                    nodes.Push(n);
                }
            }
        }

        #endregion Tree

        #region MatrixList

        /// <summary>
        /// Groups as matrix list.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static MatrixList<TKey, TSource> GroupAsMatrixList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).ToMatrixList();
        }

        /// <summary>
        /// Groups as matrix list.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static MatrixList<TKey, TSource> GroupAsMatrixList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.GroupBy(keySelector, comparer).ToMatrixList(comparer);
        }

        /// <summary>
        /// Groups as matrix list.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static MatrixList<TKey, TSource> GroupAsMatrixList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey?> keySelector, IEqualityComparer<TKey> comparer)
            where TKey : struct
        {
            return source.Where(x => keySelector(x).HasValue).GroupBy(x => keySelector(x).Value, comparer).ToMatrixList(comparer);
        }

        /// <summary>
        /// Groups as matrix list.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns></returns>
        public static MatrixList<TKey, TSource> GroupAsMatrixList<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey?> keySelector)
            where TKey : struct
        {
            return source.Where(x => keySelector(x).HasValue).GroupBy(x => keySelector(x).Value).ToMatrixList();
        }

        /// <summary>
        /// To the matrix list.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public static MatrixList<TKey, TSource> ToMatrixList<TKey, TSource>(this IEnumerable<IGrouping<TKey, TSource>> source, IEqualityComparer<TKey> comparer = null)
        {
            MatrixList<TKey, TSource> result = new MatrixList<TKey, TSource>(comparer);
            if (source.HasItem())
            {
                foreach (var item in source)
                {
                    result.Add(item.Key, (item as IEnumerable<TSource>).ToList() ?? new List<TSource>());
                }
            }

            return result;
        }

        /// <summary>
        /// To the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="matrixList">The matrix list.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>List&lt;T&gt;.</returns>
        public static List<T> ToList<T>(this MatrixList<T> matrixList, Func<string, T, bool> filter = null)
        {
            List<T> result = new List<T>();

            if (matrixList != null)
            {
                foreach (var one in matrixList)
                {
                    foreach (var item in one.Value)
                    {
                        if (filter == null || filter(one.Key, item))
                        {
                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// To the matrix.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="keyFunc">The key function.</param>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        /// <returns>MatrixList&lt;T&gt;.</returns>
        public static MatrixList<T> ToMatrix<T>(this List<T> list, Func<T, string> keyFunc, bool keyCaseSensitive = true)
        {
            var result = new MatrixList<T>(keyCaseSensitive);

            if (list != null && keyFunc != null)
            {
                foreach (var one in list)
                {
                    result.Add(keyFunc(one), one);
                }
            }

            return result;
        }

        /// <summary>
        /// To the matrix.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="list">The list.</param>
        /// <param name="keyFunc">The key function.</param>
        /// <param name="valueFunc">The value function.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns></returns>
        public static MatrixList<TKey, TValue> ToMatrix<TEntity, TKey, TValue>(this List<TEntity> list, Func<TEntity, TKey> keyFunc, Func<TEntity, TValue> valueFunc, IEqualityComparer<TKey> equalityComparer = null)
        {
            var result = new MatrixList<TKey, TValue>(equalityComparer);

            if (list != null && keyFunc != null && valueFunc != null)
            {
                foreach (var one in list)
                {
                    result.Add(keyFunc(one), valueFunc(one));
                }
            }

            return result;
        }

        /// <summary>
        /// Ases the matrix list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="key">The key.</param>
        /// <param name="keyCaseSensitive">if set to <c>true</c> [key case sensitive].</param>
        /// <returns>MatrixList&lt;T&gt;.</returns>
        public static MatrixList<T> AsMatrixList<T>(this List<T> list, string key, bool keyCaseSensitive = true)
        {
            var result = new MatrixList<T>(keyCaseSensitive)
            {
                { key.SafeToString(), list ?? new List<T>() }
            };

            return result;
        }

        #endregion MatrixList

        #region Range<T>

        /// <summary>
        /// Hits any range.
        /// </summary>
        /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
        /// <param name="ranges">The ranges.</param>
        /// <param name="hitTry">The hit try.</param>
        /// <returns></returns>
        public static Range<TCoordinate> HitAnyRange<TCoordinate>(this IEnumerable<Range<TCoordinate>> ranges, TCoordinate hitTry)
            where TCoordinate : struct, IComparable
        {
            if (ranges != null)
            {
                foreach (var one in ranges)
                {
                    if ((!one.From.HasValue || hitTry.CompareTo(one.From.Value) >= 0) && (!one.To.HasValue || hitTry.CompareTo(one.To.Value) < 0))
                    {
                        return one;
                    }
                }
            }

            return null;
        }

        #endregion Range<T>

        #region ICodeIdentifier

        /// <summary>
        /// Determines whether [has valid code].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        ///   <c>true</c> if [has valid code] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasValidCode(this ICodeIdentifier item)
        {
            return !string.IsNullOrWhiteSpace(item?.Code);
        }

        /// <summary>
        /// Codes the equals.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool CodeEquals(this ICodeIdentifier x, ICodeIdentifier y)
        {
            return HasValidCode(x) && HasValidCode(y) && StringComparer.OrdinalIgnoreCase.Equals(x.Code, y.Code);
        }

        #endregion ICodeIdentifier

        #region IAccessClientIdentifier

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyTo(this IAccessClientIdentifier source, IAccessClientIdentifier destination)
        {
            if (source != null && destination != null)
            {
                destination.DeviceId = source.DeviceId;
                destination.DeviceName = source.DeviceName;
                destination.IpV4Address = source.IpV4Address;
                destination.IpV6Address = source.IpV6Address;
                destination.UserAgent = source.UserAgent;
            }
        }

        #endregion IAccessClientIdentifier

        #region KVMetaExtensible

        /// <summary>
        /// Computes the specified k.
        /// </summary>
        /// <param name="kvMetaObject">The kv meta object.</param>
        /// <param name="k">The k.</param>
        /// <param name="operator">The operator.</param>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static bool Compute(this IKVMetaExtensible kvMetaObject, string k, string @operator, JValue v)
        {
            return Compute(kvMetaObject?.KVMeta, k, @operator, v);
        }

        /// <summary>
        /// Computes the specified key.
        /// </summary>
        /// <param name="jsonObject">The json object.</param>
        /// <param name="k">The k.</param>
        /// <param name="operator">The operator.</param>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static bool Compute(this JObject jsonObject, string k, string @operator, JValue v)
        {
            if (jsonObject != null && !string.IsNullOrWhiteSpace(k))
            {
                var property = jsonObject.GetProperty(k);

                if (property != null && property.Type == v.Type)
                {
                    return JsonValueOperatesOn(property.Value<JValue>(), @operator, v);
                }
            }

            return false;
        }

        /// <summary>
        /// Jsons the value operates on.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="operator">The operator.</param>
        /// <param name="right">The right.</param>
        /// <returns></returns>
        public static bool JsonValueOperatesOn(JValue left, string @operator, JValue right)
        {
            if (left != null && right != null && left.Type == right.Type && !string.IsNullOrWhiteSpace(@operator))
            {
                switch (@operator)
                {
                    case "==":
                        return left == right;

                    case "!=":
                        return left != right;

                    case ">":
                        return left.CompareTo(right) > 0;

                    case "<":
                        return left.CompareTo(right) < 0;

                    case "<=":
                        return left.CompareTo(right) >= 0;

                    case ">=":
                        return left.CompareTo(right) <= 0;

                    default:
                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Computes the specified key.
        /// </summary>
        /// <param name="kvMetaObject">The kv meta object.</param>
        /// <param name="k">The k.</param>
        /// <param name="operator">The operator.</param>
        /// <param name="v">The v.</param>
        /// <returns></returns>
        public static bool Compute(this IDictionary<string, JValue> kvMetaObject, string k, string @operator, JValue v)
        {
            JValue metaValue = null;
            return (kvMetaObject.HasItem()
                && !string.IsNullOrWhiteSpace(k)
                && !string.IsNullOrWhiteSpace(@operator)
                && v != null && kvMetaObject.TryGetValue(k, out metaValue)) ?
                    JsonValueOperatesOn(metaValue, @operator, v)
                    : false;
        }

        #endregion KVMetaExtensible

        /// <summary>
        /// To the tag collection.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seperators">The seperators.</param>
        /// <returns></returns>
        public static TagCollection ToTagCollection(this string value, params char[] seperators)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                TagCollection result = new TagCollection();
                foreach (var item in value.Split(seperators.HasItem() ? seperators : new char[] { ',', '.', '，', '；', '/', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    result.Add(item);
                }

                return result;
            }

            return null;
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyTo(this IEventStampRange source, IEventDateRange destination)
        {
            if (source != null && destination != null)
            {
                destination.StartDate = source.StartStamp.ToDate();
                destination.EndDate = source.EndStamp.ToDate();
            }
        }

        /// <summary>
        /// To the stamp point.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <returns></returns>
        public static StampPoint<T> ToStampPoint<T>(this T anyObject)
            where T : ISimpleBaseObject
        {
            if (anyObject != null && anyObject.CreatedStamp.HasValue)
            {
                return new StampPoint<T>(anyObject.CreatedStamp.Value, anyObject);
            }

            return null;
        }

        /// <summary>
        /// Sorts the by sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sequenceList">The sequence list.</param>
        public static void SortBySequence<T>(this List<T> sequenceList)
          where T : ISequence
        {
            if (sequenceList.HasItem())
            {
                sequenceList.Sort(x => x.Sequence);
            }
        }

        /// <summary>
        /// Creates the commit request.
        /// </summary>
        /// <param name="storageIdentifier">The storage identifier.</param>
        /// <returns></returns>
        public static BinaryStorageCommitRequest CreateCommitRequest(this BinaryStorageIdentifier storageIdentifier)
        {
            return storageIdentifier == null ? null : new BinaryStorageCommitRequest(storageIdentifier);
        }

        /// <summary>
        /// Gets the by code.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyCollection">Any collection.</param>
        /// <param name="code">The code.</param>
        /// <param name="stringComparison">The string comparison.</param>
        /// <returns></returns>
        public static T GetByCode<T>(this IEnumerable<T> anyCollection, string code, StringComparison stringComparison = StringComparison.OrdinalIgnoreCase)
           where T : ICodeIdentifier
        {
            return (anyCollection.HasItem() && !string.IsNullOrWhiteSpace(code)) ? anyCollection.FirstOrDefault(x => code.MeaningfulEquals(x.Code, stringComparison)) : default(T);
        }

        /// <summary>
        /// Determines whether the specified now is expired.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="now">The now.</param>
        /// <returns>
        ///   <c>true</c> if the specified now is expired; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsExpired<T>(this T anyObject, DateTime? now = null)
            where T : IExpirable
        {
            return anyObject != null && anyObject.ExpiredStamp.HasValue && anyObject.ExpiredStamp < (now ?? DateTime.UtcNow);
        }
    }
}