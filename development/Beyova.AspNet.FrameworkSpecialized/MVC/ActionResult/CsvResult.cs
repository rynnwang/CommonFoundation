using System.Web.Mvc;

namespace Beyova.Web
{
    /// <summary>
    /// Class CsvResult.
    /// </summary>
    public class CsvResult : ActionResult
    {
        #region Property

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public string Data { get; set; }

        #endregion Property

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvResult"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public CsvResult(string data)
        {
            Data = data;
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult" /> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (Data == null)
            {
                new EmptyResult().ExecuteResult(context);
                return;
            }

            context.HttpContext.Response.ContentType = "text/csv";

            context.HttpContext.Response.Output.Write(Data.SafeToString());
        }
    }
}