using System;

namespace Beyova
{
    /// <summary>
    /// Class MathExtension.
    /// </summary>
    public static class MathExtension
    {
        #region Max

        /// <summary>
        /// Internals the maximum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns></returns>
        public static T Max<T>(this T item1, T item2) where T : struct, IComparable
        {
            T maxResult;
            Max<T>(item1, item2, out maxResult);
            return maxResult;
        }

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TComparible">The type of the comparible.</typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="getComparer">The get comparer.</param>
        /// <param name="maxResult">The maximum result.</param>
        /// <returns><c>true</c> if item1 is Max, <c>false</c> otherwise.</returns>
        public static bool Max<TEntity, TComparible>(this TEntity item1, TEntity item2, Func<TEntity, TComparible?> getComparer, out TEntity maxResult)
            where TComparible : struct, IComparable
        {
            if (getComparer == null)
            {
                maxResult = default(TEntity);
                return false;
            }

            var comparibleItem1 = item1 == null ? null : getComparer(item1);
            var comparibleItem2 = item2 == null ? null : getComparer(item2);
            if (!comparibleItem1.HasValue)
            {
                maxResult = item2;
                return false;
            }
            else if (!comparibleItem2.HasValue)
            {
                maxResult = item1;
                return false;
            }
            else
            {
                var result = comparibleItem1.Value.CompareTo(comparibleItem2.Value) > 0;
                maxResult = result ? item1 : item2;

                return result;
            }
        }

        /// <summary>
        /// Maximums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="maxResult">The maximum result.</param>
        /// <returns><c>true</c> if item1 is Max, <c>false</c> otherwise.</returns>
        public static bool Max<T>(this T item1, T item2, out T maxResult) where T : struct, IComparable
        {
            bool result = item1.CompareTo(item2) > 0;
            maxResult = result ? item1 : item2;

            return result;
        }

        /// <summary>
        /// Maximums the specified item1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>System.Nullable&lt;T&gt;.</returns>
        public static T? Max<T>(this T? item1, T? item2) where T : struct, IComparable
        {
            if (!item1.HasValue)
            {
                return item2;
            }
            else if (!item2.HasValue)
            {
                return item1;
            }
            else
            {
                return Max(item1.Value, item2.Value);
            }
        }

        #endregion Max

        #region Min

        /// <summary>
        /// Internals the maximum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns></returns>
        public static T Min<T>(this T item1, T item2) where T : struct, IComparable
        {
            T minResult;
            Min<T>(item1, item2, out minResult);
            return minResult;
        }

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TComparible">The type of the comparible.</typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="getComparer">The get comparer.</param>
        /// <param name="minResult">The minimum result.</param>
        /// <returns>
        ///   <c>true</c> if item1 is Min, <c>false</c> otherwise.
        /// </returns>
        public static bool Min<TEntity, TComparible>(this TEntity item1, TEntity item2, Func<TEntity, TComparible?> getComparer, out TEntity minResult)
            where TComparible : struct, IComparable
        {
            if (getComparer == null)
            {
                minResult = default(TEntity);
                return false;
            }

            var comparibleItem1 = getComparer?.Invoke(item1);
            var comparibleItem2 = getComparer?.Invoke(item2);
            if (!comparibleItem1.HasValue)
            {
                minResult = item2;
                return false;
            }
            else if (!comparibleItem2.HasValue)
            {
                minResult = item1;
                return false;
            }
            else
            {
                var result = comparibleItem1.Value.CompareTo(comparibleItem2.Value) < 0;
                minResult = result ? item1 : item2;

                return result;
            }
        }

        /// <summary>
        /// Minimums the specified item2.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="minResult">The minimum result.</param>
        /// <returns>
        ///   <c>true</c> if item1 is Min, <c>false</c> otherwise.
        /// </returns>
        public static bool Min<T>(this T item1, T item2, out T minResult) where T : struct, IComparable
        {
            bool result = item1.CompareTo(item2) < 0;
            minResult = result ? item1 : item2;

            return result;
        }

        /// <summary>
        /// Minimums the specified item1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1">The item1.</param>
        /// <param name="item2">The item2.</param>
        /// <returns>System.Nullable&lt;T&gt;.</returns>
        public static T? Min<T>(this T? item1, T? item2) where T : struct, IComparable
        {
            if (!item1.HasValue)
            {
                return item2;
            }
            else if (!item2.HasValue)
            {
                return item1;
            }
            else
            {
                return Min(item1.Value, item2.Value);
            }
        }

        #endregion Min

        /// <summary>
        /// Bytes the wise sum with.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="byteItems">The byte items.</param>
        /// <returns></returns>
        public static byte[] ByteWiseSumWith(this byte[] item, params byte[][] byteItems)
        {
            try
            {
                item.CheckNullOrEmptyCollection(nameof(item));

                if (byteItems == null || byteItems.Length == 0)
                {
                    return item;
                }

                int index = 0;
                foreach (var one in byteItems)
                {
                    if (one.Length != item.Length)
                    {
                        throw ExceptionFactory.CreateInvalidObjectException(nameof(byteItems), data: new { index }, reason: "LengthDismatch");
                    }

                    index++;
                }

                var result = new byte[item.Length];

                for (var i = 0; i < item.Length; i++)
                {
                    var sum = Convert.ToInt32(item[i]);

                    foreach (var one in byteItems)
                    {
                        sum += Convert.ToInt32(one[i]);
                    }

                    result[i] = Convert.ToByte(sum);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { item, byteItems });
            }
        }

        /// <summary>
        /// Sigmoids the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static float Sigmoid(double value)
        {
            //https://stackoverflow.com/questions/412019/math-optimization-in-c-sharp
            return 1.0f / (1.0f + (float)Math.Exp(-value));
        }
    }
}