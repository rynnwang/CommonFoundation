using System;
using System.Collections;
using System.Collections.Generic;
using Beyova.Diagnostic;

namespace Beyova
{
    /// <summary>
    /// LoopEnumerator{T} is used for endless repeatable loop, like 1-&gt;2-&gt;3-&gt;1-&gt;2-&gt;...
    /// It is especially used when a few resources needs to be used in turns.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LoopEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// The current index
        /// </summary>
        protected int _currentIndex = -1;

        /// <summary>
        /// The list
        /// </summary>
        protected List<T> _list;

        /// <summary>
        /// Gets the internal current.
        /// </summary>
        /// <value>
        /// The internal current.
        /// </value>
        protected bool InternalGetCurrent(out T output)
        {
            output = default(T);

            if (IsValidInex(_currentIndex))
            {
                output = _list[_currentIndex];
                output = OnGet(_currentIndex, output);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the internal current.
        /// </summary>
        /// <value>
        /// The internal current.
        /// </value>
        protected T InternalCurrent
        {
            get
            {
                T output;
                InternalGetCurrent(out output);
                return output;
            }
        }

        #region IEnumerator

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        public T Current
        {
            get
            {
                return InternalCurrent;
            }
        }

        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        object IEnumerator.Current
        {
            get { return InternalCurrent; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            //Nothing to do.
        }

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
        /// </returns>
        public bool MoveNext()
        {
            if (_list.Count < 0)
            {
                return false;
            }

            _currentIndex++;
            if (_currentIndex >= _list.Count)
            {
                _currentIndex = 0;
            }

            return true;
        }

        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
            _currentIndex = _list.Count - 1;
        }

        #endregion IEnumerator

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopEnumerator{T}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public LoopEnumerator(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopEnumerator{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public LoopEnumerator(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoopEnumerator{T}"/> class.
        /// </summary>
        public LoopEnumerator()
        {
            _list = new List<T>();
        }

        #endregion Constructor

        #region Public methods

        /// <summary>
        /// Does the in loop.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="functionality">The functionality.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        /// <exception cref="Beyova.Diagnostic.ResourceNotFoundException"></exception>
        public TOutput DoInLoop<TOutput>(Func<T, TOutput> functionality, out Exception exception)
        {
            functionality.CheckNullObject(nameof(functionality));
            exception = null;
            T input;

            if (InternalGetCurrent(out input))
            {
                try
                {
                    var result = functionality(input);
                    MoveNext();
                    return result;
                }
                catch (Exception ex)
                {
                    exception = ex.Handle();
                    return default(TOutput);
                }
            }
            else
            {
                throw new ResourceNotFoundException(typeof(T).GetFullName(), _currentIndex.ToString());
            }
        }

        /// <summary>
        /// Does the in loop.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="functionality">The functionality.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        /// <exception cref="Beyova.Diagnostic.ResourceNotFoundException"></exception>
        public TOutput DoInLoop<TOutput>(Func<T, object, TOutput> functionality, object parameter, out Exception exception)
        {
            functionality.CheckNullObject(nameof(functionality));
            exception = null;
            T input;

            if (InternalGetCurrent(out input))
            {
                try
                {
                    var result = functionality(input, parameter);
                    MoveNext();
                    return result;
                }
                catch (Exception ex)
                {
                    exception = ex.Handle(new { parameter });
                    return default(TOutput);
                }
            }
            else
            {
                throw new ResourceNotFoundException(typeof(T).GetFullName(), _currentIndex.ToString());
            }
        }

        /// <summary>
        /// Adds the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Add(T obj)
        {
            var result = OnGet(_list.Count, obj);
            _list.Add(result);
        }

        /// <summary>
        /// Adds if not null.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void AddIfNotNull(T obj)
        {
            var result = OnGet(_list.Count, obj);
            if (result != null)
            {
                _list.Add(result);
            }
        }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <value>
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (IsValidInex(index))
                {
                    var result = _list[index];
                    result = OnGet(index, result);
                    return result;
                }

                return default(T);
            }
            set
            {
                if (IsValidInex(index))
                {
                    var result = OnSet(index, value);
                    _list[index] = result;
                }
            }
        }

        /// <summary>
        /// Removes at.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public T RemoveAt(int index)
        {
            if (IsValidInex(index))
            {
                T result = _list[index];
                _list.RemoveAt(index);
                OnRemove(index);
                return result;
            }

            return default(T);
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _list.Clear();
            OnClear();
        }

        #endregion Public methods

        /// <summary>
        /// Determines whether [is valid inex] [the specified index].
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        ///   <c>true</c> if [is valid inex] [the specified index]; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsValidInex(int index)
        {
            return index > -1 && index < _list.Count;
        }

        /// <summary>
        /// Called when [get].
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected virtual T OnGet(int index, T obj)
        {
            return obj;
        }

        /// <summary>
        /// Called when [set].
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected virtual T OnSet(int index, T obj)
        {
            return obj;
        }

        /// <summary>
        /// Called when [remove].
        /// </summary>
        /// <param name="index">The index.</param>
        protected virtual void OnRemove(int index)
        {
        }

        /// <summary>
        /// Called when [clear].
        /// </summary>
        protected virtual void OnClear()
        {
        }
    }
}