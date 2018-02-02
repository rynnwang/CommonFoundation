using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Security
{
    /// <summary>
    /// 
    /// </summary>
    public class SecondFactorAuthenticator
    {
        //Reference: https://www.codeproject.com/articles/403355/implementing-two-factor-authentication-in-asp-net

        #region Core functionality

        /// <summary>
        /// Generates the password.
        /// </summary>
        /// <param name="secret">The secret.</param>
        /// <param name="iterationNumber">The iteration number.</param>
        /// <param name="digits">The digits. Default value: 6. (Google authenticator uses 6 digits)</param>
        /// <returns></returns>
        private static string GeneratePassword(string secret, long iterationNumber, int digits = 6)
        {
            byte[] counter = BitConverter.GetBytes(iterationNumber);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(counter);
            }

            byte[] key = Encoding.ASCII.GetBytes(secret);

            HMACSHA1 hmac = new HMACSHA1(key, true);

            byte[] hash = hmac.ComputeHash(counter);

            int offset = hash[hash.Length - 1] & 0xf;

            int binary =
                ((hash[offset] & 0x7f) << 24)
                | ((hash[offset + 1] & 0xff) << 16)
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);

            int password = binary % (int)Math.Pow(10, digits); // 6 digits

            return password.ToString(new string('0', digits));
        }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="secret">The secret.</param>
        /// <param name="password">The password.</param>
        /// <param name="checkAdjacentIntervals">The check adjacent intervals.</param>
        /// <returns>
        ///   <c>true</c> if the specified secret is valid; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValid(string secret, string password, int checkAdjacentIntervals = 1)
        {
            if (password == GetPassword(secret))
                return true;

            for (int i = 1; i <= checkAdjacentIntervals; i++)
            {
                if (password == GetPassword(secret, GetCurrentCounter() + i))
                    return true;

                if (password == GetPassword(secret, GetCurrentCounter() - i))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Generates the secret key.
        /// </summary>
        /// <returns></returns>
        public static string GenerateSecretKey()
        {
            using (RandomNumberGenerator rng = RNGCryptoServiceProvider.Create())
            {
                rng.GetBytes(buffer);
            }

            // Generates a 10 character string of A-Z, a-z, 0-9
            // Don't need to worry about any = padding from the
            // Base64 encoding, since our input buffer is divisible by 3
            var secret = Convert.ToBase64String(buffer).Substring(0, 10).Replace('/', '0').Replace('+', '1');
            return new Base32Encoder().Encode(Encoding.ASCII.GetBytes(secret));
        }

        #endregion
    }
}
