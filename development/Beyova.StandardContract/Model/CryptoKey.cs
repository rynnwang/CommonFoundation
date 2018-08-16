using System;
using Newtonsoft.Json;

namespace Beyova
{
    /// <summary>
    /// Class CryptoKey.
    /// </summary>
    [JsonConverter(typeof(CryptoKeyConverter))]
    public sealed class CryptoKey : IImplicitiveStringValueObject
    {

        /// <summary>
        /// Gets the byte value.
        /// </summary>
        /// <value>
        /// The byte value.
        /// </value>
        public byte[] ByteValue { get; private set; }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue { get { return ByteValue.EncodeBase64(); } }

        #region Constructor

        /// <summary>
        /// Prevents a default instance of the <see cref="CryptoKey"/> class from being created.
        /// </summary>
        private CryptoKey() { }

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
            this.ByteValue = value.DecodeBase64ToByteArray();
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
            return value?.ByteValue;
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
            return value?.StringValue;
        }

        #endregion implicit methods

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
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.ByteValue.ValueEquals((obj as CryptoKey)?.ByteValue);
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
    }
}