using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Beyova.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class StartUp
    {
        /// <summary>
        /// Sets the json net as default json serializer.
        /// </summary>
        public static void SetJsonNetAsDefaultJsonSerializer()
        {
            ValueProviderFactories.Factories.Remove(ValueProviderFactories.Factories.
                OfType<JsonValueProviderFactory>().FirstOrDefault());
            ValueProviderFactories.Factories.Add(new JsonNetValueProviderFactory());

            ModelBinders.Binders.DefaultBinder = new JsonNetModelBinder();
        }
    }
}
