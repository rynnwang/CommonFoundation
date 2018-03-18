using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class BaseJsonConfigurationReader.
    /// </summary>
    /// <seealso cref="Beyova.IConfigurationReader" />
    public abstract class BaseJsonConfigurationReader : IConfigurationReader
    {
        #region Constants

        /// <summary>
        /// The configuration key primary SQL connection
        /// </summary>
        private const string ConfigurationKey_PrimarySqlConnection = "PrimarySqlConnection";

        #endregion Constants

        /// <summary>
        /// The items
        /// </summary>
        protected Dictionary<string, RuntimeConfigurationItem> _items = null;

        /// <summary>
        /// The throw exception
        /// </summary>
        protected bool _throwException;

        /// <summary>
        /// Gets or sets the source assembly.
        /// </summary>
        /// <value>
        /// The source assembly.
        /// </value>
        public string SourceAssembly { get; protected set; }

        /// <summary>
        /// Gets or sets the type of the reader.
        /// </summary>
        /// <value>
        /// The type of the reader.
        /// </value>
        public string ReaderType { get; protected set; }

        /// <summary>
        /// Gets or sets the core component version.
        /// </summary>
        /// <value>
        /// The core component version.
        /// </value>
        public Version CoreComponentVersion { get; protected set; }

        /// <summary>
        /// Gets the settings count.
        /// </summary>
        /// <value>The settings count.</value>
        public int Count
        {
            get
            {
                return _items == null ? 0 : _items.Count;
            }
        }

        /// <summary>
        /// Gets the hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash
        {
            get
            {
                if (_items.Keys.HasItem())
                {
                    List<Byte[]> hashValues = new List<byte[]>(_items.Count * 2);
                    Parallel.ForEach(_items, x =>
                    {
                        hashValues.Add(Encoding.UTF8.GetBytes(x.Key).ToMD5Bytes());
                        hashValues.Add(Encoding.UTF8.GetBytes(x.Value.Value.ToString()).ToMD5Bytes());
                    });

                    var resultBytes = (new byte[16]).ByteWiseSumWith(hashValues.ToArray());
                    return resultBytes.ToHex();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the SQL connection.
        /// </summary>
        /// <value>The SQL connection.</value>
        public string SqlConnection
        {
            get { return GetConfiguration<string>(ConfigurationKey_PrimarySqlConnection); }
        }

        /// <summary>
        /// Gets the primary SQL connection.
        /// </summary>
        /// <value>
        /// The primary SQL connection.
        /// </value>
        public string PrimarySqlConnection
        {
            get { return GetConfiguration<string>(ConfigurationKey_PrimarySqlConnection); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJsonConfigurationReader" /> class.
        /// </summary>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <param name="readerType">Type of the reader.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        protected BaseJsonConfigurationReader(string sourceAssembly, string coreComponentVersion, string readerType, bool throwException = false)
        {
            this.SourceAssembly = sourceAssembly;
            this.ReaderType = readerType ?? this.GetType().GetFullName();
            this.CoreComponentVersion = coreComponentVersion.AsVersion();
            this._throwException = throwException;

            // Need to call intialize or reload manually, because additonal constructor parameter loading order issue.
            //_items = Initialize(throwException);
        }

        #region Public method

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        public T GetConfiguration<T>(string key, T defaultValue = default(T))
        {
            T result;
            return TryGetConfiguration(key, out result) ? result : defaultValue;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public object GetConfiguration(string key, object defaultValue = null)
        {
            object result;
            return TryGetConfiguration(key, out result) ? result : defaultValue;
        }

        #endregion Public method

        #region Initialization

        /// <summary>
        /// Initializes the specified throw exception.
        /// </summary>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns>
        /// Dictionary&lt;System.String, ConfigurationItem&gt;.
        /// </returns>
        protected abstract Dictionary<string, RuntimeConfigurationItem> Initialize(bool throwException = false);

        /// <summary>
        /// Fills the object collection.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <param name="configurationRawItem">The configuration raw item.</param>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="readerType">Type of the reader.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        protected static void FillObjectCollection(IDictionary<string, RuntimeConfigurationItem> container, Version coreComponentVersion, ConfigurationRawItem configurationRawItem, string sourceAssembly, string readerType, bool throwException = false)
        {
            try
            {
                container.CheckNullObject(nameof(container));
                configurationRawItem.CheckNullObject(nameof(configurationRawItem));
                configurationRawItem.Key.CheckEmptyString(nameof(configurationRawItem.Key));

                var runtimeConfigurationItem = RuntimeConfigurationItem.FromRaw(sourceAssembly, readerType, coreComponentVersion, configurationRawItem);
                runtimeConfigurationItem.CheckNullObject(nameof(runtimeConfigurationItem));
                container.Merge(runtimeConfigurationItem.Key, runtimeConfigurationItem);
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    throw ex.Handle(data: new { coreComponentVersion, configurationRawItem, sourceAssembly, readerType });
                }
            }
        }

        #endregion Initialization

        /// <summary>
        /// Refreshes the settings.
        /// </summary>
        public virtual void Reload()
        {
            _items = Initialize(this._throwException);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerable<KeyValuePair<string, object>> GetValues()
        {
            var result = new Dictionary<string, object>();

            _items.Where(result, (k, v) => { return v?.IsActive ?? false; }, x => x.Value);
            return result;
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, RuntimeConfigurationItem> GetItems()
        {
            return this._items;
        }

        /// <summary>
        /// Tries the get configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryGetConfiguration<T>(string key, out T result)
        {
            RuntimeConfigurationItem configuration = null;

            if (_items.SafeTryGetValue(key, out configuration) && configuration.IsActive)
            {
                result = (T)(configuration.Value);
                return true;
            }

            result = default(T);
            return false;
        }

        /// <summary>
        /// Tries the get configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public bool TryGetConfiguration(string key, out object result)
        {
            RuntimeConfigurationItem configuration = null;

            if (_items.SafeTryGetValue(key, out configuration) && configuration.IsActive)
            {
                result = configuration.Value;
                return true;
            }

            result = default(object);
            return false;
        }
    }
}