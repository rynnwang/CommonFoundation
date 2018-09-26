using System;
using System.Collections.Generic;
using System.Text;

namespace Beyova.Api
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApiRouterExtensionFeature
    {
        /// <summary>
        /// Gets the documentat generation.
        /// </summary>
        /// <value>
        /// The documentat generation.
        /// </value>
        public static ParameterlessPrioritizedFunctionInjection<byte[]> DocumentatGeneration { get; private set; } = new ParameterlessPrioritizedFunctionInjection<byte[]>();
    }
}
