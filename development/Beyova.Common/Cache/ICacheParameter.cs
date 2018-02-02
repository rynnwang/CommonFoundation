namespace Beyova.Cache
{
    /// <summary>
    /// Interface ICacheParameter
    /// </summary>
    public interface ICacheParameter
    {
        /// <summary>
        /// The expiration in second
        /// </summary>
        long? ExpirationInSecond { get; }
    }
}