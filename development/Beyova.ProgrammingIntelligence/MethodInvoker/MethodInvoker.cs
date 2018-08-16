using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Beyova.ExceptionSystem;
using Newtonsoft.Json.Linq;

namespace Beyova.ProgrammingIntelligence
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MethodInvoker
    {
        /// <summary>
        /// The method
        /// </summary>
        private MethodInfo _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodInvoker"/> class.
        /// </summary>
        /// <param name="method">The method.</param>
        private MethodInvoker(MethodInfo method)
        {
            _method = method;
        }

        /// <summary>
        /// Invokes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public MethodInvokeResult Invoke(params object[] parameters)
        {
            return _method.InvokeMethod(null, parameters);
        }

        /// <summary>
        /// Invokes the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public MethodInvokeResult Invoke(JObject parameters)
        {
            return _method.InvokeMethod(null, parameters);
        }


        /// <summary>
        /// Gets the parameter requirements.
        /// </summary>
        /// <returns></returns>
        public List<MethodInvokeParameter> GetParameterRequirements()
        {
            List<MethodInvokeParameter> result = new List<MethodInvokeParameter>();

            var parameters = this._method.GetParameters();
            int sequence = 0;
            foreach (var one in parameters)
            {
                result.Add(new MethodInvokeParameter
                {
                    IsOptional = one.IsOptional,
                    Name = one.Name,
                    Type = one.ParameterType,
                    Sequence = sequence,
                    IsOut = one.IsOut,
                    IsIn = one.IsIn
                });

                sequence++;
            }

            return result;
        }

        /// <summary>
        /// Creates the method invoker.
        /// </summary>
        /// <param name="methodCodeFullName">Full name of the method code.</param>
        /// <returns></returns>
        public static MethodInvoker CreateMethodInvoker(string methodCodeFullName)
        {
            try
            {
                methodCodeFullName.CheckEmptyString(nameof(methodCodeFullName));
                //TODO. Consider simple case in this stage.
                // No generic, no instance. Only static method. {namespace}+.{class}.{method}

                var lastDot = methodCodeFullName.LastIndexOf('.');
                if (lastDot > -1)
                {
                    string method = methodCodeFullName.Substring(lastDot);
                    var typeFullName = methodCodeFullName.Substring(0, lastDot);

                    var type = ReflectionExtension.SmartGetType(typeFullName, false);
                    type.CheckNullObjectAsInvalid(nameof(methodCodeFullName), externalDataReference: new { method, typeFullName });

                    var methodInfo = type.GetMethod(method);
                    methodInfo.CheckNullObjectAsInvalid(nameof(methodCodeFullName), externalDataReference: new { method, typeFullName });

                    return new MethodInvoker(methodInfo);
                }
                else
                {
                    throw ExceptionFactory.CreateInvalidObjectException(nameof(methodCodeFullName), data: new { methodCodeFullName });
                }
            }
            catch (BaseException bex)
            {
                throw bex;
            }
            catch (Exception ex)
            {
                throw ex.Handle(new { methodCodeFullName });
            }
        }
    }
}
