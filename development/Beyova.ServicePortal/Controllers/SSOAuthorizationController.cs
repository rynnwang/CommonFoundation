using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.CommonAdminService;
using Beyova.CommonServiceInterface;
using E1.Content;
using EF.E1Technology.AuthenticationProfileService;
using EF.E1Technology.AuthenticationProfileService.Model;
using EF.E1Technology.Developer.Core;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    public class SSOAuthorizationController : SSOAuthorizationBaseController<UserInfo, FunctionalRole, AuthenticationResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SSOAuthorizationController"/> class.
        /// </summary>
        public SSOAuthorizationController() : base(ModuleCodes.AuthenticationProfileService)
        {

        }

        /// <summary>
        /// Gets the client.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns></returns>
        protected override ISSOAuthorizationService<UserInfo, FunctionalRole, SSOAuthorizationPartner, SSOAuthorizationPartnerCriteria, SSOAuthorization, SSOAuthorizationCriteria, AuthenticationResult> GetClient(string environment)
        {
            var endpoint = this.GetEnvironmentEndpoint(environment);
            return new SingleSignOnServiceApiClient(endpoint);
        }

        /// <summary>
        /// Gets the environment endpoint.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>EnvironmentEndpoint.</returns>
        protected override EnvironmentEndpoint GetEnvironmentEndpoint(string environment)
        {
            return ServiceConfigurationUtility.GetEndpoint(this._moduleCode, environment);
        }
    }
}