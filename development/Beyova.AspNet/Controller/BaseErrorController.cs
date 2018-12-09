using System;
using System.Web.Mvc;
using Beyova.ExceptionSystem;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BaseErrorController : Controller
    {
        /// <summary>
        /// Gets the exception information by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected abstract ExceptionInfo GetExceptionInfoByKey(Guid? key);

        /// <summary>
        /// Indexes the specified code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="minor">The minor.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual ActionResult Index(int? code = null, string minor = null, Guid? key = null)
        {
            ExceptionInfo exception = null;

            if (key.HasValue)
            {
                exception = GetExceptionInfoByKey(key);
            }
            else
            {
                exception = new ExceptionInfo
                {
                    Code = new ExceptionCode
                    {
                        Major = code.Int32ToEnum<ExceptionCode.MajorCode>(ExceptionCode.MajorCode.Undefined),
                        Minor = minor
                    }
                };
            }

            return View("Error", exception);
        }
    }
}