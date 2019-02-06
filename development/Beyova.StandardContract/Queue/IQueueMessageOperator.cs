using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface IQueueMessageOperator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueueMessageOperator<T>
    {
        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        void Enqueue(T item);

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns></returns>
        QueueMessageItem<T> GetMessage(int? invisibilityTimeout);

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        List<QueueMessageItem<T>> GetMessages(int messageCount, int? invisibilityTimeout);

        /// <summary>
        /// Deletes the message.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="receipt">The receipt.</param>
        void DeleteMessage(string id, string receipt);

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        QueueMessageItem<T> Peek();

        /// <summary>
        /// Peeks the messages.
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <returns></returns>
        List<QueueMessageItem<T>> PeekMessages(int messageCount);
    }
}