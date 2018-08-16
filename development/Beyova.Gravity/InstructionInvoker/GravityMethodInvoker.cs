using System;
using Beyova.ProgrammingIntelligence;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// </summary>
    public class GravityMethodInvoker : IGravityInstructionInvoker
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get { return GravityBuiltInInstructionTypes.Method; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityMethodInvoker"/> class.
        /// </summary>
        internal GravityMethodInvoker()
        {
        }

        /// <summary>
        /// Invokes this instance.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// JToken.
        /// </returns>
        public JToken Invoke(string action, JToken parameters)
        {
            MethodInvokeResult result = null;

            try
            {
                MethodInvoker _invoker = MethodInvoker.CreateMethodInvoker(action);
                var requiredParameters = _invoker.GetParameterRequirements();

                if (requiredParameters.HasItem())
                {
                    if (requiredParameters.Count == 1)
                    {
                        result = _invoker.Invoke(parameters.ToObject(requiredParameters[0].Type));
                    }
                    else
                    {
                        var parameterDictionary = parameters as JObject;
                        parameterDictionary.CheckNullObject(nameof(parameterDictionary));

                        object[] parameterObjects = new object[requiredParameters.Count];

                        foreach (var one in requiredParameters)
                        {
                            var propertyObject = parameterDictionary.GetProperty(one.Name);
                            parameterObjects[one.Sequence] = propertyObject == null ? one.Type.GetDefaultValue() : propertyObject.ToObject(one.Type);
                        }

                        result = _invoker.Invoke(parameterObjects);
                    }
                }
                else
                {
                    result = _invoker.Invoke();
                }
            }
            catch (Exception ex)
            {
                result = new MethodInvokeResult
                {
                    Exception = ex.Handle(new { parameters })
                };
            };

            return result.ToJson(false);
        }
    }
}