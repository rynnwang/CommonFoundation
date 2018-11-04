using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Beyova.ExceptionSystem;

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

            if (list != null)
            {
                foreach (var one in list)
                {
                    result.Add(keyFunc == null ? string.Empty : keyFunc(one), one);
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
            var result = new MatrixList<T>(keyCaseSensitive);
            result.Add(key.SafeToString(), list ?? new List<T>());

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

        #endregion
    }
}