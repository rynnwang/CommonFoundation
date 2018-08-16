using System;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class UpdateConfigurationCommandInvoker.
    /// </summary>
    public class UpdateConfigurationCommandInvoker : IGravityInstructionInvoker
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get { return GravityBuiltInInstructionTypes.Configuration; } }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        public JToken Invoke(string action, JToken parameters)
        {
            try
            {
                action.CheckEmptyString(nameof(action));

                JToken result = null;

                switch (action.ToLowerInvariant())
                {
                    case "update":
                        result = UpdateConfiguration().ToJson(false);
                        break;
                    case "upload":
                        result = ConfigurationHub.ConfigurationItems.ToJson(false);
                        break;
                    case "backup":
                        //TODO
                    default:
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }

        /// <summary>
        /// Updates the configuration.
        /// </summary>
        /// <returns></returns>
        private string UpdateConfiguration()
        {
            try
            {
                var configurationReader = GravityShell.Current.ConfigurationReader;
                configurationReader.CheckNullObject(nameof(configurationReader));

                configurationReader.Reload();
                return configurationReader.Hash;
            }
            catch (Exception ex)
            {
                throw ex.Handle();
            }
        }
    }
}