using System;

namespace Beyova.UnitTestKit
{
    /// <summary>
    /// Class TestStep.
    /// </summary>
    public class TestStep : IIdentifier
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public Guid? Key { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the next step key.
        /// </summary>
        /// <value>The next step key.</value>
        public Guid? NextStepKey { get; set; }


    }
}
