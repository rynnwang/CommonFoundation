using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Beyova.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonNetModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Binds the model.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerHidden]
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var provider = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (provider != null)
            {
                try
                {
                    return provider.ConvertTo(bindingContext.ModelType);
                }
                catch { }
            }
            return base.BindModel(controllerContext, bindingContext);

        }
    }
}
