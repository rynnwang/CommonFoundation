namespace Beyova
{
    /// <summary>
    /// Interface IQueueOperator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueueOperator<T>
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
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        QueueItem<T> Dequeue();

        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        QueueItem<T> Peek();
    }
}