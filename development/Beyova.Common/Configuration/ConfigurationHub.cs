using System.Collections.Generic;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    internal static class ConfigurationHub
    {
        /// <summary>
        /// The configuration readers
        /// </summary>
        private static List<IConfigurationReader> _configurationReaders = new List<IConfigurationReader>();

        /// <summary>
        /// Initializes the <see cref="ConfigurationHub"/> class.
        /// </summary>
        static ConfigurationHub()
        {
            //_configurationReaders.AddIfNotNull(GravityShell.Current?.ConfigurationReader);
        }

        /// <summary>
        /// Registers the configuration reader.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public static void RegisterConfigurationReader(IConfigurationReader reader)
        {
            reader?.Reload();
            _configurationReaders.AddIfNotNull(reader);
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// T.
        /// </returns>
        public static T GetConfiguration<T>(string key, T defaultValue = default(T))
        {
            T result;
            foreach (IConfigurationReader one in _configurationReaders)
            {
                if (one.TryGetConfiguration(key, out result))
                {
                    return result;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>System.String.</returns>
        public static object GetConfiguration(string key, object defaultValue = null)
        {
            object result;
            foreach (IConfigurationReader one in _configurationReaders)
            {
                if (one.TryGetConfiguration(key, out result))
                {
                    return result;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Gets the configuration setting count.
        /// </summary>
        /// <value>The configuration setting count.</value>
        public static int ConfigurationSettingCount
        {
            get
            {
                int sum = 0;
                foreach (IConfigurationReader one in _configurationReaders)
                {
                    sum += one.Count;
                }

                return sum;
            }
        }

        /// <summary>
        /// Gets the configuration values.
        /// </summary>
        /// <value>
        /// The configuration values.
        /// </value>
        public static Dictionary<string, object> ConfigurationValues
        {
            get
            {
                Dictionary<string, object> result = new Dictionary<string, object>(ConfigurationSettingCount);
                foreach (IConfigurationReader one in _configurationReaders)
                {
                    result.Merge(one.GetValues(), false);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the configuration items.
        /// </summary>
        /// <value>
        /// The configuration items.
        /// </value>
        public static Dictionary<string, RuntimeConfigurationItem> ConfigurationItems
        {
            get
            {
                Dictionary<string, RuntimeConfigurationItem> result = new Dictionary<string, RuntimeConfigurationItem>(ConfigurationSettingCount);
                foreach (IConfigurationReader one in _configurationReaders)
                {
                    result.Merge(one.GetItems(), false);
                }

                return result;
            }
        }

        /// <summary>
        /// Reloads this instance.
        /// </summary>
        public static void Reload()
        {
            foreach (var one in _configurationReaders)
            {
                one.Reload();
            }
        }
    }
}