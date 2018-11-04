using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Beyova.AspNet
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonNetValueProviderResult : ValueProviderResult
    {
        private JToken _jtoken;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetValueProviderResult"/> class.
        /// </summary>
        /// <param name="valueRaw">The value raw.</param>
        /// <param name="key">The key.</param>
        /// <param name="info">The information.</param>
        public JsonNetValueProviderResult(JToken valueRaw, string key, CultureInfo info)
        {
            _jtoken = valueRaw;
        }

        /// <summary>
        /// Converts to.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        [System.Diagnostics.DebuggerHidden]
        public override object ConvertTo(Type type, CultureInfo culture)
        {
            return _jtoken?.ToObject(type);
        }
    }
}
