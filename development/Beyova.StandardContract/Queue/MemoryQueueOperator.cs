using System.Collections.Generic;
using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MemoryQueueOperator<T> : IQueueOperator<T>
    {
        /// <summary>
        /// The queue
        /// </summary>
        private Queue<QueueItem<T>> _queue = new Queue<QueueItem<T>>();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _queue.Clear();
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        public QueueItem<T> Dequeue()
        {
            return _queue.Dequeue();
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            this._queue.Enqueue(new QueueItem<T>
            {
                Message = item,
                CreatedStamp = DateTime.UtcNow,
                Id = Guid.NewGuid().ToHexString()
            });
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return this._queue.Count;
        }

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        public QueueItem<T> Peek()
        {
            return this._queue.Peek();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryQueueOperator{T}"/> class.
        /// </summary>
        public MemoryQueueOperator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryQueueOperator{T}"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        private MemoryQueueOperator(Queue<T> queue)
        {
            if (queue != null)
            {
                while (queue.Count > 0)
                {
                    Enqueue(queue.Dequeue());
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryQueueOperator{T}"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        public MemoryQueueOperator(Queue<QueueItem<T>> queue)
        {
            if (queue != null)
            {
                this._queue = new Queue<QueueItem<T>>(queue);
            }
        }

        /// <summary>
        /// Wraps from.
        /// </summary>
        /// <param name="queue">The queue.</param>
        /// <returns></returns>
        public static MemoryQueueOperator<T> WrapFrom(Queue<T> queue)
        {
            return queue == null ? null : new MemoryQueueOperator<T>(queue);
        }
    }
}