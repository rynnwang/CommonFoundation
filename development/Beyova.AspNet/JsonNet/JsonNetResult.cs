using Newtonsoft.Json;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <value>
        /// The settings.
        /// </value>
        public JsonSerializerSettings Settings { get; private set; }

        /// <summary>
        /// Executes the result.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = ContentType.SafeToString(HttpConstants.ContentType.Json);

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data == null)
            {
                return;
            }

            var scriptSerializer = JsonSerializer.Create(JsonExtension.SafeJsonSerializationSettings);

            using (var sw = new StringWriter())
            {
                scriptSerializer.Serialize(sw, Data);
                response.Write(sw.ToString());
            }
        }
    }
}