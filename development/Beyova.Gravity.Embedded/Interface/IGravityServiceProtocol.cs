using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Interface IGravityServiceProtocol
    /// </summary>
    public interface IGravityServiceProtocol
    {
        /// <summary>
        /// Registers the specified member registration.
        /// </summary>
        /// <param name="memberRegistration">The member registration.</param>
        /// <returns></returns>
        [GravityApiOperation(GravityBuiltInResources.Member, "Register")]
        RegistrationToken Register(MemberRegistration memberRegistration);

        /// <summary>
        /// Heartbeats the specified heartbeat.
        /// </summary>
        /// <param name="heartbeat">The heartbeat.</param>
        /// <returns>HeartbeatEcho.</returns>
        [GravityApiOperation(GravityBuiltInResources.Member, "Heartbeat")]
        HeartbeatEcho Heartbeat(Heartbeat heartbeat);

        /// <summary>
        /// Retrieves the configuration.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>RemoteConfigurationObject.</returns>
        [GravityApiOperation(GravityBuiltInResources.CentralManagement, "RetrieveConfiguration")]
        RemoteConfigurationObject RetrieveConfiguration(string name = null);

        /// <summary>
        /// Submits the instruction result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        [GravityApiOperation(GravityBuiltInResources.Instruction, "Submit")]
        Guid? SubmitInstructionResult(GravityInstructionResult result);
    }
}