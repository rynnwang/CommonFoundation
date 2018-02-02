using System;
using System.Text;
using System.Text.RegularExpressions;
using Beyova.ExceptionSystem;
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
        /// The value
        /// </summary>
        private byte[] _value;

        /// <summary>
        /// Gets the byte value.
        /// </summary>
        /// <value>
        /// The byte value.
        /// </value>
        public byte[] ByteValue { get { return _value; } }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>
        /// The string value.
        /// </value>
        public string StringValue { get { return _value.EncodeBase64(); } }

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
            this._value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoKey"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public CryptoKey(string value)
        {
            this._value = value.DecodeBase64ToByteArray();
        }

        #endregion

        #region implicit methods

        /// <summary>
        /// Performs an implicit conversion from <see cref="byte[]"/> to <see cref="CryptoKey"/>.
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
        /// Performs an implicit conversion from <see cref="CryptoKey" /> to <see cref="byte[]" />.
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

        #endregion

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
        /// Checks the null or empty.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        public void CheckNullOrEmpty(string variableName = null)
        {
            _value.CheckNullOrEmptyCollection(variableName.SafeToString(nameof(CryptoKey)));
        }
    }
}