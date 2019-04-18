using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class GenericEqualityComparer.
    /// </summary>
    /// <typeparam name="TOriginal">The type of the t original.</typeparam>
    /// <typeparam name="TComparer">The type of the t comparer.</typeparam>
    public class GenericComparer<TOriginal, TComparer> : IComparer<TOriginal>
    {
        /// <summary>
        /// The _convert function
        /// </summary>
        protected Func<TOriginal, TComparer> _convertFunction;

        /// <summary>
        /// The _comparer
        /// </summary>
        protected IComparer<TComparer> _comparer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericEqualityComparer{TOriginal, TComparer}" /> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <param name="comparer">The comparer.</param>
        public GenericComparer(Func<TOriginal, TComparer> converter, IComparer<TComparer> comparer = null)
        {
            _convertFunction = converter ?? (x => { return default(TComparer); });
            _comparer = comparer ?? Comparer<TComparer>.Default;
        }

        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public int Compare(TOriginal x, TOriginal y)
        {
            return _comparer.Compare(_convertFunction(x), _convertFunction(y));
        }
    }
}