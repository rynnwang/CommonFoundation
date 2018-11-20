using System.Web.Mvc;

namespace Beyova.Web
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