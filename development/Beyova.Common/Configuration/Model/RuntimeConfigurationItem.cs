using Newtonsoft.Json.Linq;
using System;

namespace Beyova
{
    /// <summary>
    /// Class RuntimeConfigurationItem
    /// </summary>
    public sealed class RuntimeConfigurationItem : ConfigurationItemBase<object>
    {
        /// <summary>
        /// Gets or sets the assembly.
        /// </summary>
        /// <value>
        /// The assembly.
        /// </value>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the type of the reader.
        /// </summary>
        /// <value>
        /// The type of the reader.
        /// </value>
        public string ReaderType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        internal bool IsActive { get; private set; }

        /// <summary>
        /// Gets or sets the type of the runtime.
        /// </summary>
        /// <value>
        /// The type of the runtime.
        /// </value>
        internal Type RuntimeType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeConfigurationItem"/> class.
        /// </summary>
        /// <param name="configurationItem">The configuration item.</param>
        private RuntimeConfigurationItem(ConfigurationRawItem configurationItem) : base(configurationItem)
        {
        }

        /// <summary>
        /// Froms the raw.
        /// </summary>
        /// <param name="sourceAssembly">The source assembly.</param>
        /// <param name="readerType">Type of the reader.</param>
        /// <param name="coreComponentVersion">The core component version.</param>
        /// <param name="configurationItem">The configuration item.</param>
        /// <returns></returns>
        public static RuntimeConfigurationItem FromRaw(string sourceAssembly, string readerType, Version coreComponentVersion, ConfigurationRawItem configurationItem)
        {
            try
            {
                configurationItem.CheckNullObject(nameof(configurationItem));
                configurationItem.Type.CheckEmptyString(nameof(configurationItem.Type));

                var result = new RuntimeConfigurationItem(configurationItem)
                {
                    Assembly = sourceAssembly,
                    ReaderType = readerType,
                    RuntimeType = ReflectionExtension.SmartGetType(configurationItem.Type, false) ?? ReflectionExtension.SmartGetType(configurationItem.Type, true)
                };

                result.IsActive = JudgeActive(coreComponentVersion, result.MinComponentVersionRequired, result.MaxComponentVersionLimited);
                result.Value = RawStringToObject(configurationItem.Value, result.RuntimeType);

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Raws the string to object.
        /// </summary>
        /// <param name="rawStringValue">The raw string value.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private static object RawStringToObject(string rawStringValue, Type type)
        {
            try
            {
                type.CheckNullObject(nameof(type));

                if (!string.IsNullOrWhiteSpace(rawStringValue))
                {
                    if (type == typeof(string))
                    {
                        return rawStringValue;
                    }
                    else if (type == typeof(Boolean) || type == typeof(Boolean?))
                    {
                        return rawStringValue.ToLowerInvariant().ParseToJToken().ToObject(type);
                    }
                    else if (type == typeof(Guid) || type == typeof(Guid?) || typeof(IStringConvertable).IsAssignableFrom(type))
                    {
                        return JToken.FromObject(rawStringValue).ToObject(type);
                    }
                    else
                    {
                        return rawStringValue.ParseToJToken().ToObject(type);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { rawStringValue, type = type?.GetFullName() });
            }
        }

        /// <summary>
        /// Judges the active.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="minRequired">The minimum required.</param>
        /// <param name="maxLimited">The maximum limited.</param>
        /// <returns>
        /// System.Boolean.
        /// </returns>
        private static bool JudgeActive(Version version, string minRequired, string maxLimited)
        {
            Version min = minRequired.AsVersion(), max = maxLimited.AsVersion();

            if (version == null)
            {
                return min == null && max == null;
            }
            else
            {
                if ((min != null && min > version) || (max != null && max < version))
                {
                    return false;
                }
            }

            return true;
        }
    }
}