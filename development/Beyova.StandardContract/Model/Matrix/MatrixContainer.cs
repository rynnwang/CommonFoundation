using System;
using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Class MatrixList.
    /// </summary>
    /// <typeparam name="TKey">The type of the t key.</typeparam>
    /// <typeparam name="TContainer">The type of the container.</typeparam>
    /// <typeparam name="TValue">The type of the t value.</typeparam>
    public abstract class MatrixContainer<TKey, TContainer, TValue> : Dictionary<TKey, TContainer>
        where TContainer : class, ICollection<TValue>
    {
        /// <summary>
        /// The value comparer
        /// </summary>
        protected IEqualityComparer<TValue> _valueComparer = null;

        /// <summary>
        /// The value capacity
        /// </summary>
        protected int _valueCapacity;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixContainer{TKey, TContainer, TValue}"/> class.
        /// </summary>
        protected MatrixContainer() : this(null, null, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixContainer{TKey, TContainer, TValue}" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        protected MatrixContainer(int capacity)
            : this(null, null, capacity, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixContainer{TKey, TContainer, TValue}"/> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        protected MatrixContainer(IEqualityComparer<TKey> keyComparer)
        : this(keyComparer, null, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixContainer{TKey, TContainer, TValue}" /> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="valueComparer">The value comparer.</param>
        protected MatrixContainer(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
            : this(keyComparer, valueComparer, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixContainer{TKey, TContainer, TValue}"/> class.
        /// </summary>
        /// <param name="keyComparer">The key comparer.</param>
        /// <param name="valueComparer">The value comparer.</param>
        /// <param name="keyCapacity">The key capacity.</param>
        /// <param name="valueCapacity">The value capacity.</param>
        protected MatrixContainer(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer, int keyCapacity, int valueCapacity)
            : base(keyCapacity, keyComparer)
        {
            _valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;
            _valueCapacity = valueCapacity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixContainer{TKey, TContainer, TValue}"/> class.
        /// </summary>
        /// <param name="matrixContainer">The matrix container.</param>
        protected MatrixContainer(MatrixContainer<TKey, TContainer, TValue> matrixContainer) : base()
        {
            InitializeMatrixContainer(matrixContainer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixList{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="matrixContainer">The matrix container.</param>
        /// <param name="comparer">The comparer.</param>
        protected MatrixContainer(MatrixContainer<TKey, TContainer, TValue> matrixContainer, IEqualityComparer<TKey> comparer) : base(comparer)
        {
            InitializeMatrixContainer(matrixContainer);
        }

        /// <summary>
        /// Initializes the matrix container.
        /// </summary>
        /// <param name="matrixContainer">The matrix container.</param>
        protected void InitializeMatrixContainer(MatrixContainer<TKey, TContainer, TValue> matrixContainer)
        {
            if (matrixContainer != null)
            {
                _valueCapacity = matrixContainer._valueCapacity;
                _valueComparer = matrixContainer._valueComparer;

                if (matrixContainer.HasItem())
                {
                    foreach (var one in matrixContainer)
                    {
                        Add(one.Key, NewContainer(_valueComparer, _valueCapacity, one.Value));
                    }
                }
            }
        }

        /// <summary>
        /// News the container.
        /// </summary>
        /// <param name="valueComparer">The value comparer.</param>
        /// <param name="valueCapacity">The value capacity.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        protected abstract TContainer NewContainer(IEqualityComparer<TValue> valueComparer, int valueCapacity, IEnumerable<TValue> values);

        /// <summary>
        /// Gets the or create.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// List&lt;TValue&gt;.
        /// </returns>
        public virtual TContainer GetOrCreate(TKey key)
        {
            return GetCollectionByKey(key, true);
        }

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual TContainer TryGet(TKey key)
        {
            return GetCollectionByKey(key, false);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void Add(TKey key, TValue value)
        {
            try
            {
                if (!IsKeyValid(key))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(key));
                }

                var list = GetCollectionByKey(key, true);
                list.Add(value);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key, value });
            }
        }

        /// <summary>
        /// Initials the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="container">The container.</param>
        public virtual void Initial(TKey key, TContainer container)
        {
            //NOTES: In case like MatrixList<TKey, Object>. Method Add would hard to tell which method is right to call (Add(key, value) of Dictionary or Add(key, List<Value>))
            try
            {
                if (!IsKeyValid(key))
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(key));
                }

                if (!ContainsKey(key))
                {
                    Add(key, container);
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { key, container });
            }
        }

        /// <summary>
        /// Gets the collection by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createIfNotExist">if set to <c>true</c> [create if not exist].</param>
        /// <returns>List&lt;T&gt;.</returns>
        protected virtual TContainer GetCollectionByKey(TKey key, bool createIfNotExist)
        {
            ValidateKey(key);

            TContainer result = ContainsKey(key) ? this[key] : null;

            if (result == null && createIfNotExist)
            {
                Add(key, NewContainer(_valueComparer, _valueCapacity, null));
                result = this[key];
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is key valid] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>true</c> if [is key valid] [the specified key]; otherwise, <c>false</c>.</returns>
        protected virtual bool IsKeyValid(TKey key)
        {
            return key != null;
        }

        /// <summary>
        /// Validates the key.
        /// </summary>
        /// <param name="key">The key.</param>
        protected void ValidateKey(TKey key)
        {
            if (!IsKeyValid(key))
            {
                throw ExceptionFactory.CreateInvalidObjectException(nameof(key), data: key);
            }
        }
    }
}