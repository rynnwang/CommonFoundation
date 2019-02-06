using System.Collections.Generic;
using System.Linq;

namespace Beyova
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseQueueMessageOperator<T> : IQueueMessageOperator<T>
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        public abstract int GetCount();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public abstract void Enqueue(T item);

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns></returns>
        public virtual QueueMessageItem<T> GetMessage(int? invisibilityTimeout)
        {
            return GetMessages(1, invisibilityTimeout).FirstOrDefault();
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        public abstract List<QueueMessageItem<T>> GetMessages(int messageCount, int? invisibilityTimeout);

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="receipt">The receipt.</param>
        public abstract void DeleteMessage(string id, string receipt);

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        public virtual QueueMessageItem<T> Peek()
        {
            return PeekMessages(1).FirstOrDefault();
        }

        /// <summary>
        /// Peeks the messages.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <returns></returns>
        public abstract List<QueueMessageItem<T>> PeekMessages(int messageCount);
    }
}