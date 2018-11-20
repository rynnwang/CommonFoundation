using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    /// Interface IConfigurationReader
    /// </summary>
    public interface IConfigurationReader
    {
        /// <summary>
        /// Gets or sets the source assembly.
        /// </summary>
        /// <value>
        /// The source assembly.
        /// </value>
        string SourceAssembly { get; }

        /// <summary>
        /// Gets or sets the type of the reader.
        /// </summary>
        /// <value>
        /// The type of the reader.
        /// </value>
        string ReaderType { get; }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        int Count { get; }

        /// <summary>
        /// Gets the primary SQL connection.
        /// </summary>
        /// <value>
        /// The primary SQL connection.
        /// </value>
        string PrimarySqlConnection { get; }

        /// <summary>
        /// Try to get configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        bool TryGetConfiguration<T>(string key, out T result);

        /// <summary>
        /// Try to get configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        bool TryGetConfiguration(string key, out object result);

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>T.</returns>
        T GetConfiguration<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        object GetConfiguration(string key, object defaultValue = null);

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <returns>IEnumerable&lt;KeyValuePair&lt;System.String, System.Object&gt;&gt;.</returns>
        IEnumerable<KeyValuePair<string, object>> GetValues();

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns></returns>
        Dictionary<string, RuntimeConfigurationItem> GetItems();

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        void Reload();
    }
}