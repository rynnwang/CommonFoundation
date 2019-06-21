//using System;
//using System.Collections.Generic;

//namespace Beyova
//{
//    /// <summary>
//    ///
//    /// </summary>
//    public static class Union
//    {
//        /// <summary>
//        /// Finds the specified predicate.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <typeparam name="TResult">The type of the result.</typeparam>
//        /// <param name="predicate">The predicate.</param>
//        /// <param name="collection">The collection.</param>
//        /// <returns></returns>
//        public static TResult Find<TEntity, TResult>(Func<TEntity, TResult> predicate, params IEnumerable<TEntity>[] collection)
//            where TResult : class
//        {
//            return Find(predicate, default(TResult), collection);
//        }

//        /// <summary>
//        /// Finds the specified predicate.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <typeparam name="TResult">The type of the result.</typeparam>
//        /// <param name="predicate">The predicate.</param>
//        /// <param name="convert">The convert.</param>
//        /// <param name="collection">The collection.</param>
//        /// <returns></returns>
//        public static TResult Find<TEntity, TResult>(Func<TEntity, bool> predicate, Func<TEntity, TResult> convert, params IEnumerable<TEntity>[] collection)
//         where TResult : class
//        {
//            return Find(predicate, convert, default(TResult), collection);
//        }

//        /// <summary>
//        /// Finds the specified predicate.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <typeparam name="TResult">The type of the result.</typeparam>
//        /// <param name="predicate">The predicate.</param>
//        /// <param name="defaultResult">The default result.</param>
//        /// <param name="collection">The collection.</param>
//        /// <returns></returns>
//        public static TResult Find<TEntity, TResult>(Func<TEntity, TResult> predicate, TResult defaultResult, params IEnumerable<TEntity>[] collection)
//        where TResult : class
//        {
//            TResult result = null;

//            if (predicate != null)
//            {
//                foreach (var item in collection)
//                {
//                    foreach (var one in item)
//                    {
//                        result = predicate(one);

//                        if (result != null)
//                        {
//                            return result;
//                        }
//                    }
//                }
//            }

//            return defaultResult;
//        }

//        /// <summary>
//        /// Finds the specified predicate.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <typeparam name="TResult">The type of the result.</typeparam>
//        /// <param name="predicate">The predicate.</param>
//        /// <param name="convert">The convert.</param>
//        /// <param name="defaultResult">The default result.</param>
//        /// <param name="collection">The collection.</param>
//        /// <returns></returns>
//        public static TResult Find<TEntity, TResult>(Func<TEntity, bool> predicate, Func<TEntity, TResult> convert, TResult defaultResult, params IEnumerable<TEntity>[] collection)
//        {
//            TResult result = defaultResult;

//            if (predicate != null && convert != null)
//            {
//                foreach (var item in collection)
//                {
//                    foreach (var one in item)
//                    {
//                        if (predicate(one))
//                        {
//                            result = convert(one);
//                            break;
//                        }
//                    }
//                }
//            }

//            return result;
//        }

//        /// <summary>
//        /// Finds the specified predicate.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <param name="predicate">The predicate.</param>
//        /// <param name="defaultResult">The default result.</param>
//        /// <param name="collection">The collection.</param>
//        /// <returns></returns>
//        public static TEntity Find<TEntity>(Func<TEntity, bool> predicate, TEntity defaultResult, params IEnumerable<TEntity>[] collection)
//        {
//            return Find(predicate, FuncExtension.GetSelf, defaultResult, collection);
//        }

//        /// <summary>
//        /// Finds the specified predicate.
//        /// </summary>
//        /// <typeparam name="TEntity">The type of the entity.</typeparam>
//        /// <param name="predicate">The predicate.</param>
//        /// <param name="collection">The collection.</param>
//        /// <returns></returns>
//        public static TEntity Find<TEntity>(Func<TEntity, bool> predicate, params IEnumerable<TEntity>[] collection)
//        {
//            return Find(predicate, FuncExtension.GetSelf, default(TEntity), collection);
//        }
//    }
//}