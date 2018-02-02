using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Beyova;
using Beyova.CommonAdminService;
using Beyova.Elastic;
using EF.E1Technology.Developer.Core;

namespace EF.E1Technology.Developer.Portal.Controllers
{
    [EnvironmentBased]
    public class ElasticController : ApiTrackingController<ElasticApiTracking>
    {
        protected override EnvironmentEndpoint GetEnvironmentEndpoint(string environment)
        {
            return ServiceConfigurationUtility.GetEndpoint(this._moduleCode, environment);
        }
    }
}