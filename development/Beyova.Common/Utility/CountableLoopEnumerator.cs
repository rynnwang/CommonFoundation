using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CountableLoopEnumerator<T> : LoopEnumerator<T>
    {
        /// <summary>
        /// The counter
        /// </summary>
        protected List<int> _counter;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableLoopEnumerator{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public CountableLoopEnumerator(int capacity) : base(capacity)
        {
            _counter = new List<int>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableLoopEnumerator{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public CountableLoopEnumerator(IEnumerable<T> collection) : base(collection)
        {
            var array = 0.AsArrayWithSameValue(collection.Count());
            _counter = new List<int>(array);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CountableLoopEnumerator{T}"/> class.
        /// </summary>
        public CountableLoopEnumerator() : base()
        {
            _counter = new List<int>();
        }

        #endregion      

        /// <summary>
        /// Gets the current count.
        /// </summary>
        /// <value>
        /// The current count.
        /// </value>
        public int CurrentCount
        {
            get { return _counter[_currentIndex]; }
        }

        /// <summary>
        /// Gets the counts.
        /// </summary>
        /// <value>
        /// The counts.
        /// </value>
        public Dictionary<T, int> Counts
        {
            get
            {
                var count = _list.Count;
                var result = new Dictionary<T, int>(count);

                for (var i = 0; i < count; i++)
                {
                    result.Add(_list[i], _counter[i]);
                }

                return result;
            }
        }

        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected override T OnGet(int index, T obj)
        {
            _counter[index]++;
            return base.OnGet(index, obj);
        }

        /// <summary>
        /// Called when [remove].
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void OnRemove(int index)
        {
            _counter[index] = 0;
            base.OnRemove(index);
        }

        /// <summary>
        /// Called when [clear].
        /// </summary>
        protected override void OnClear()
        {
            _counter.Clear();
            base.OnClear();
        }
    }
}
