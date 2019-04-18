using System;
using Beyova.Diagnostic;
using Newtonsoft.Json.Linq;

namespace Beyova
{
    /// <summary>
    /// Class SandboxMarshalInvokeResult.
    /// </summary>
    public sealed class SandboxInvokeResult
    {
        /// <summary>
        /// The value
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// The type
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The exception information
        /// </summary>
        public string ExceptionInfo { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="SandboxInvokeResult"/> class from being created.
        /// </summary>
        private SandboxInvokeResult() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SandboxInvokeResult"/> class.
        /// </summary>
        /// <param name="result">The result.</param>
        internal SandboxInvokeResult(SandboxMarshalInvokeResult result)
        {
            if (result != null)
            {
                Type = result.Type;
                ExceptionInfo = result.ExceptionInfo;
                Value = result.Value;
            }
        }

        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <returns>System.Object.</returns>
        public object GetObject()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Value) || string.IsNullOrWhiteSpace(Type))
                {
                    return null;
                }

                Type type = ReflectionExtension.SmartGetType(Type, true);
                return JToken.Parse(Value).ToObject(type);
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { Value, Type });
            }
        }

        /// <summary>
        /// Gets the exception information.
        /// </summary>
        /// <returns>ExceptionInfo.</returns>
        public ExceptionInfo GetExceptionInfo()
        {
            return string.IsNullOrWhiteSpace(ExceptionInfo) ? null : JToken.Parse(ExceptionInfo).ToObject<ExceptionInfo>();
        }
    }
}