using System;
using System.Runtime.CompilerServices;
using Beyova.Api;
using Newtonsoft.Json.Linq;

namespace Beyova.ExceptionSystem
{
    /// <summary>
    /// Class for unauthorized operation exception
    /// </summary>
    public class UnauthorizedOperationException : BaseException
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="minorCode">The minor code.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="scene">The scene.</param>
        public UnauthorizedOperationException(Exception innerException, string minorCode = null, object data = null, FriendlyHint hint = null, ExceptionScene scene = null)
            : base(string.Format("Unauthorized operation on [{0}].", scene?.MethodName),
                  new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = minorCode.SafeToString("Operation") }, innerException, data, hint, scene)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="minorCode">The reason.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="operationName">Name of the operation.</param>
        public UnauthorizedOperationException(string minorCode = null, object data = null, FriendlyHint hint = null, [CallerMemberName] string operationName = null)
                 : base(string.Format("Unauthorized operation on [{0}].", operationName),
                       new ExceptionCode { Major = ExceptionCode.MajorCode.UnauthorizedOperation, Minor = minorCode.SafeToString("Operation") }, data: data, hint: hint)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnauthorizedOperationException" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="createdStamp">The created stamp.</param>
        /// <param name="message">The message.</param>
        /// <param name="scene">The scene.</param>
        /// <param name="code">The code.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="operatorCredential">The operator credential.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        internal UnauthorizedOperationException(Guid key, DateTime createdStamp, string message, ExceptionScene scene, ExceptionCode code, Exception innerException, BaseCredential operatorCredential, JToken data, FriendlyHint hint)
                  : base(key, createdStamp, message, scene, code, innerException, operatorCredential, data, hint)
        {
        }

        #endregion Constructor
    }
}