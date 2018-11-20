namespace Beyova
{
    /// <summary>
    /// Interface IDataSecurityProvider
    /// </summary>
    public interface IDataSecurityProvider
    {
        /// <summary>
        /// Encrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        byte[] Encrypt(byte[] value);

        /// <summary>
        /// Decrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        byte[] Decrypt(byte[] value);
    }
}