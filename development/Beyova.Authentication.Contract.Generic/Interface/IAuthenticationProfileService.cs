using System;
using System.Collections.Generic;
using Beyova.Api;
using Beyova.Api.RestApi;
using Beyova.SaasPlatform;

namespace Beyova.FunctionService.Generic
{
    /// <summary>
    /// Interface IAuthenticationProfileService
    /// </summary>
    /// <typeparam name="TUserInfo">The type of the t user information.</typeparam>
    /// <typeparam name="TUserCriteria">The type of the t user criteria.</typeparam>
    /// <typeparam name="TAuthenticationResult">The type of the t authentication result.</typeparam>
    /// <typeparam name="TFunctionalRole">The type of the t functional role.</typeparam>
    public interface IAuthenticationProfileService<TUserInfo, TUserCriteria, TAuthenticationResult, TFunctionalRole>
        where TUserInfo : IUserInfo<TFunctionalRole>
        where TUserCriteria : IUserCriteria<TFunctionalRole>
        where TAuthenticationResult : IAuthenticationResult<TUserInfo, TFunctionalRole>
        where TFunctionalRole : struct, IConvertible
    {
        #region Authentication

        /// <summary>
        /// Authenticates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>TAuthenticationResult.</returns>
        [ApiOperation(ApiResourceNames.Token, HttpConstants.HttpMethod.Put)]
        TAuthenticationResult Authenticate(AuthenticationRequest request);

        /// <summary>
        /// Log Out.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="realm">The realm.</param>
        [ApiOperation(ApiResourceNames.Token, HttpConstants.HttpMethod.Delete)]
        void Logout(string token, string realm = null);

        /// <summary>
        /// Gets the user information by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="realm">The realm.</param>
        /// <returns>
        /// TUserInfo.
        /// </returns>
        [ApiOperation(ApiResourceNames.Token, HttpConstants.HttpMethod.Get)]
        TUserInfo GetUserInfoByToken(string token, string realm = null);

        /// <summary>
        /// Renews the token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="realm">The realm.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.Token, HttpConstants.HttpMethod.Post)]
        TAuthenticationResult RenewToken(string token, string realm = null);

        /// <summary>
        /// Gets the current session information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="realm">The realm.</param>
        /// <returns>
        /// SessionInfo.
        /// </returns>
        [ApiOperation(ApiResourceNames.SessionInfo, HttpConstants.HttpMethod.Get)]
        SessionInfo GetSessionInfoByToken(string token, string realm = null);

        /// <summary>
        /// Queries the session information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>SessionInfo.</returns>
        [ApiOperation(ApiResourceNames.SessionInfo, HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        List<SessionInfo> QuerySessionInfo(SessionCriteria criteria);

        #endregion Authentication

        /// <summary>
        /// Queries the user information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.UserInfo, HttpConstants.HttpMethod.Post)]
        [TokenRequired(true)]
        List<TUserInfo> QueryUserInfo(TUserCriteria criteria);

        /// <summary>
        /// Gets the current user profile.
        /// </summary>
        /// <param name="userKey">The user key.</param>
        /// <returns></returns>
        [ApiOperation(ApiResourceNames.UserInfo, HttpConstants.HttpMethod.Get)]
        TUserInfo GetUserInfoByUserKey(Guid? userKey);
    }
}