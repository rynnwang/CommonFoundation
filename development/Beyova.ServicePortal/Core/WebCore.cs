using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Beyova.Cache;
using Beyova.Gravity;

namespace Beyova.ServicePortal
{
    public static class WebCore
    {
        #region Cache

        public static CollectionCacheContainer<ProductInfo> ProductCache = new CollectionCacheContainer<ProductInfo>("productCache", () =>
         {
             GravityManagementServiceCore service = new GravityManagementServiceCore();
             return service.QueryProductInfo(new ProductCriteria { });
         }, expirationInSecond: 15, handleException: (e) =>
           {
               Framework.ApiTracking.LogException(e.ToExceptionInfo());
               return false;
           });

        #endregion
    }
}