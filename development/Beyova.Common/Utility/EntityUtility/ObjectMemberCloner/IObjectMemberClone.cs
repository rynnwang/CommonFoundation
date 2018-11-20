namespace Beyova
{
    /// <summary>
    /// Interface IObjectMemberClone
    /// </summary>
    public interface IObjectMemberClone<T>
    {
        /// <summary>
        /// Shadows the clone.
        /// </summary>
        void ShadowClone(T source, T destination);
    }
}