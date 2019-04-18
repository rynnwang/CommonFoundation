using System;
using System.Reflection;
using Beyova.Diagnostic;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public sealed class DynamicStaticMethod
    {
        /// <summary>
        /// The namespace
        /// </summary>
        private const string _namespace = "Beyova.DynamicStaticMethod";

        /// <summary>
        /// The method information
        /// </summary>
        private MethodInfo _methodInfo;

        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public DynamicStaticMethodOptions Options { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicStaticMethod"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        internal DynamicStaticMethod(DynamicStaticMethodOptions options)
        {
            Options = options;
            Initialize(options);
        }

        /// <summary>
        /// Initializes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        private void Initialize(DynamicStaticMethodOptions options)
        {
            try
            {
                options.CheckNullObject(nameof(options));
                options.DynamicCode.CheckEmptyString(nameof(options.DynamicCode));
                Type type = null;

                if (!string.IsNullOrWhiteSpace(options.ClassName) && !string.IsNullOrWhiteSpace(options.MethodName))
                {
                    if (options.ClassName == options.MethodName)
                    {
                        throw new InvalidObjectException(nameof(options), data: new { options }, reason: "Conflict name for compile closure.");
                    }

                    type = ReflectionExtension.SmartGetType(string.Format("{0}.{1}", _namespace, options.ClassName));
                    if (type != null)
                    {
                        _methodInfo = type.GetMethod(options.MethodName);

                        if (_methodInfo != null && _methodInfo.IsStatic)
                        {
                            return;
                        }
                    }
                }

                var tmpId = Guid.NewGuid().ToString("N");
                var generatedCode = BuildMethodCodeAsClass(options, tmpId);

                TempAssemblyProvider provider = new TempAssemblyProvider();
                var tempAssembly = provider.CreateTempAssembly(generatedCode.AsArray());

                type = ReflectionExtension.SmartGetType(string.Format("{0}.DynamicStatiClass{1}", _namespace, tmpId));
                type.CheckNullObjectAsInvalid(nameof(options), data: new { options, generatedCode, tmpId });

                _methodInfo = type.GetMethod(string.Format("DynamicStaticMethod{0}", tmpId));
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { options });
            }
        }

        /// <summary>
        /// Builds the method code as class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="tmpId">The temporary identifier.</param>
        /// <returns></returns>
        private static string BuildMethodCodeAsClass(DynamicStaticMethodOptions options, string tmpId)
        {
            const string fullFormat = @"
using System;
using Beyova;

namespace {0}{{
    public static class DynamicStatiClass{1}
    {{
        public static {2} DynamicStaticMethod{1}()
        {{
            {3}
        }}
    }}
}}
";

            return string.Format(fullFormat, _namespace, tmpId, (options.ReturnObjectType ?? typeof(void)).ToCodeLook(), options.DynamicCode);
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <returns></returns>
        public MethodInvokeResult Invoke()
        {
            MethodInvokeResult result = new MethodInvokeResult();

            try
            {
                result.ReturnObject = _methodInfo.Invoke(null, null);
            }
            catch (Exception ex)
            {
                result.Exception = ex.Handle();
            }

            return result;
        }

        /// <summary>
        /// Froms the method code.
        /// </summary>
        /// <param name="methodCode">The method code.</param>
        /// <returns></returns>
        public static DynamicStaticMethod FromMethodCode(string methodCode)
        {
            try
            {
                methodCode.CheckEmptyString(nameof(methodCode));

                return new DynamicStaticMethod(new DynamicStaticMethodOptions { DynamicCode = methodCode, ReturnObjectType = typeof(void) });
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { methodCode });
            }
        }
    }
}