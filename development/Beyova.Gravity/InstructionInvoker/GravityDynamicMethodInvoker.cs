using System;
using Beyova.ProgrammingIntelligence;
using Newtonsoft.Json.Linq;

namespace Beyova.Gravity
{
    /// <summary>
    /// </summary>
    public class GravityDynamicMethodInvoker : IGravityInstructionInvoker
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get { return GravityBuiltInInstructionTypes.Dynamic; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="GravityDynamicMethodInvoker"/> class.
        /// </summary>
        internal GravityDynamicMethodInvoker()
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
                action.CheckEmptyString(nameof(action));
                parameters.CheckNullObject(nameof(parameters));

                var methodOptins = parameters.ToObject<DynamicStaticMethodOptions>();
                if (methodOptins != null)
                {
                    var method = new DynamicStaticMethod(methodOptins);
                    result = method?.Invoke();
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