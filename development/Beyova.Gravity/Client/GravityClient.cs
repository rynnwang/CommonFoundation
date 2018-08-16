using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityClient.
    /// </summary>
    internal class GravityClient : GravityAgent, IGravityServiceProtocol
    {
        /// <summary>
        /// The token
        /// </summary>
        protected string _token = null;

        /// <summary>
        /// The expired stamp
        /// </summary>
        protected DateTime? _expiredStamp = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityClient" /> class.
        /// </summary>
        /// <param name="entry">The entry.</param>
        internal GravityClient(GravityEntryObject entry) : base(entry)
        {
            this.Entry = entry;
        }

        public RegistrationToken Register(MemberRegistration memberRegistration)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Heartbeats the specified heartbeat.
        /// </summary>
        /// <param name="heartbeat">The heartbeat.</param>
        public HeartbeatEcho Heartbeat(Heartbeat heartbeat)
        {
            if (heartbeat != null)
            {
                return Invoke<Heartbeat, HeartbeatEcho>("heartbeat", heartbeat);
            }

            return null;
        }

        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>RemoteConfigurationObject.</returns>
        public RemoteConfigurationObject RetrieveConfiguration(string name = null)
        {
            return Invoke<string, RemoteConfigurationObject>("configuration", name);
        }

        /// <summary>
        /// Invokes the specified feature.
        /// </summary>
        /// <typeparam name="TIn">The type of the t in.</typeparam>
        /// <typeparam name="TOut">The type of the t out.</typeparam>
        /// <param name="action">The feature.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>TOut.</returns>
        protected TOut Invoke<TIn, TOut>(string action, TIn parameter)
        {
            return Invoke<TIn, TOut>(string.Empty, action, parameter);
        }

        public Guid? SubmitInstructionResult(GravityInstructionResult result)
        {
            if (result != null)
            {
                return Invoke<GravityInstructionResult, Guid?>("command", result);
            }

            return null;
        }
    }
}