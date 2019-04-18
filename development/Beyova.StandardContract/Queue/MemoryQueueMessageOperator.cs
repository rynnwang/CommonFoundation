using System;
using System.Collections.Generic;
using System.Linq;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MemoryQueueMessageOperator<T> : IQueueMessageOperator<T>
    {
        /// <summary>
        /// The queue
        /// </summary>
        private List<QueueMessageItem<T>> _items = new List<QueueMessageItem<T>>();

        /// <summary>
        /// The locker
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryQueueMessageOperator{T}"/> class.
        /// </summary>
        public MemoryQueueMessageOperator()
        {
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (locker)
            {
                _items.Clear();
            }
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Enqueue(T item)
        {
            lock (locker)
            {
                _items.Add(new QueueMessageItem<T>
                {
                    Message = item,
                    CreatedStamp = DateTime.UtcNow,
                    Id = Guid.NewGuid().ToString("N")
                });
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            lock (locker)
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// Internals the get items.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <param name="invisibilityTimeout">The invisibility timeout.</param>
        /// <returns></returns>
        private List<QueueMessageItem<T>> InternalGetItems(int messageCount, int? invisibilityTimeout)
        {
            if (messageCount < 0)
            {
                messageCount = 1;
            }

            if (invisibilityTimeout < 500)
            {
                invisibilityTimeout = 60;
            }

            try
            {
                List<QueueMessageItem<T>> result = new List<QueueMessageItem<T>>();
                lock (locker)
                {
                    foreach (var item in _items)
                    {
                        if (result.Count >= messageCount)
                        {
                            break;
                        }

                        if (!item.RevisibleStamp.HasValue || item.RevisibleStamp.Value < DateTime.UtcNow)
                        {
                            result.Add(item);
                            item.RevisibleStamp = invisibilityTimeout.HasValue ? new DateTime?(DateTime.UtcNow.AddSeconds(invisibilityTimeout.Value)) : null;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new
                {
                    messageCount,
                    invisibilityTimeout
                });
            }
        }

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        public QueueMessageItem<T> Peek()
        {
            return PeekMessages(1).FirstOrDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryQueueMessageOperator{T}"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        private MemoryQueueMessageOperator(Queue<T> queue)
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
        /// Initializes a new instance of the <see cref="MemoryQueueMessageOperator{T}"/> class.
        /// </summary>
        /// <param name="queue">The queue.</param>
        public MemoryQueueMessageOperator(Queue<QueueMessageItem<T>> queue)
        {
            if (queue != null)
            {
                _items = new List<QueueMessageItem<T>>(queue);
            }
        }

        /// <summary>
        /// Wraps from.
        /// </summary>
        /// <param name="queue">The queue.</param>
        /// <returns></returns>
        public static MemoryQueueMessageOperator<T> WrapFrom(Queue<T> queue)
        {
            return queue == null ? null : new MemoryQueueMessageOperator<T>(queue);
        }

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="receipt">The receipt.</param>
        public void DeleteMessage(string id, string receipt)
        {
            try
            {
                id.CheckEmptyString(nameof(id));

                lock (locker)
                {
                    var deletedItem = _items.FindAndRemove(x => x.Id.Equals(id));
                }
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { id });
            }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <param name="invisibilityTimeout">The invisibility timeout.</param>
        /// <returns></returns>
        public QueueMessageItem<T> GetMessage(int? invisibilityTimeout)
        {
            return GetMessages(1, invisibilityTimeout).FirstOrDefault();
        }

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <param name="invisibilityTimeout">The invisibility timeout.</param>
        /// <returns></returns>
        public List<QueueMessageItem<T>> GetMessages(int messageCount, int? invisibilityTimeout)
        {
            return InternalGetItems(messageCount, invisibilityTimeout ?? 2 * 60);
        }

        /// <summary>
        /// Peeks the messages.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <returns></returns>
        public List<QueueMessageItem<T>> PeekMessages(int messageCount)
        {
            return InternalGetItems(messageCount, null);
        }
    }
}