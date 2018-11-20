using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class CryptoKey.
    /// </summary>
    [JsonConverter(typeof(CryptoKeyConverter))]
    public struct CryptoKey
    {
        /// <summary>
        /// Gets the byte value.
        /// </summary>
        /// <value>
        /// The byte value.
        /// </value>
        public byte[] ByteValue { get; set; }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue { get { return ByteValue.EncodeBase64(); } }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoKey"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public CryptoKey(byte[] value)
        {
            this.ByteValue = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoKey"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public CryptoKey(string value)
        {
            this.ByteValue = string.IsNullOrWhiteSpace(value) ? null : value.DecodeBase64ToByteArray();
        }

        #endregion Constructor

        #region implicit methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="byte" /> array to <see cref="CryptoKey"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator CryptoKey(byte[] value)
        {
            return new CryptoKey(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="CryptoKey"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator CryptoKey(string value)
        {
            return new CryptoKey(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CryptoKey" /> to <see cref="byte" /> array.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator byte[] (CryptoKey value)
        {
            return value.ByteValue;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CryptoKey"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CryptoKey value)
        {
            return value.StringValue;
        }

        #endregion implicit methods

        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmpty()
        {
            return this.ByteValue == null || this.ByteValue.Length == 0;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return StringValue;
        }

        /// <summary>
        /// To the hexadecimal.
        /// </summary>
        /// <returns></returns>
        public string ToHex()
        {
            return ByteValue.ToHex();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.ByteValue.ValueEquals(((CryptoKey)obj).ByteValue);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return ByteValue?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Randoms the specified byte length.
        /// </summary>
        /// <param name="byteLength">Length of the byte.</param>
        /// <returns></returns>
        public static CryptoKey Random(int byteLength)
        {
            try
            {
                if (byteLength < 1)
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(byteLength));
                }

                Random rnd = new Random();
                Byte[] b = new Byte[byteLength];
                rnd.NextBytes(b);

                return new CryptoKey(b);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { byteLength });
            }
        }

        /// <summary>
        /// Gets the empty.
        /// </summary>
        /// <value>
        /// The empty.
        /// </value>
        public static CryptoKey Empty { get; private set; } = new CryptoKey(null as byte[]);
    }
}