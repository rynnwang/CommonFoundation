using System;

namespace Beyova.Gravity
{
    /// <summary>
    /// Class GravityContext.
    /// </summary>
    public static class GravityContext
    {
        [ThreadStatic]
        private static GravityMemberInfo _currentProductInfo = null;

        /// <summary>
        /// The product key
        /// </summary>
        [ThreadStatic]
        private static Guid? _productKey;

        /// <summary>
        /// Gets or sets the product key.
        /// </summary>
        /// <value>The product key.</value>
        public static Guid? ProductKey
        {
            get { return _productKey; }
            internal set { _productKey = value; }
        }

        /// <summary>
        /// Gets the gravity member information.
        /// </summary>
        /// <value>
        /// The gravity member information.
        /// </value>
        public static GravityMemberInfo GravityMemberInfo
        {
            get { return _currentProductInfo; }
            internal set { _currentProductInfo = value; }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        internal static void Dispose()
        {
            _productKey = null;
            _currentProductInfo = null;
        }
    }
}