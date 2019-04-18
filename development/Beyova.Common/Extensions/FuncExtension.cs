using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static class FuncExtension
    {
        /// <summary>
        /// Gets the self.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T GetSelf<T>(T obj)
        {
            return obj;
        }

        /// <summary>
        /// Converts as nullable input.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static Func<TInput?, TOutput> ConvertAsNullableInput<TInput, TOutput>(this Func<TInput, TOutput> func)
            where TInput : struct
        {
            if (func == null)
            {
                return null;
            }

            Func<TInput?, TOutput> result = (x) =>
             {
                 return x.HasValue ? func(x.Value) : default(TOutput);
             };

            return result;
        }

        /// <summary>
        /// Converts as inputless.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static Func<TInput, TOutput> ExtendAsInputless<TInput, TOutput>(this Func<TOutput> func)
        {
            if (func == null)
            {
                return null;
            }

            Func<TInput, TOutput> result = (x) =>
            {
                return func();
            };

            return result;
        }

        /// <summary>
        /// Extends as outputless.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public static Action<TInput> ConvertToAction<TInput, TOutput>(this Func<TInput, TOutput> func)
        {
            if (func == null)
            {
                return null;
            }

            Action<TInput> result = (x) =>
            {
                func(x);
            };

            return result;
        }
    }
}