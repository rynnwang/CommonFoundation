using Beyova.Diagnostic;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Beyova
{
    /// <summary>
    /// Class ExceptionFactory.
    /// </summary>
    public static class ExceptionFactory
    {
        /// <summary>
        /// Checks empty string.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="Beyova.Diagnostic.NullObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        /// <exception cref="NullObjectException"></exception>
        public static void CheckEmptyString(this string anyString, string objectIdentity, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(anyString))
            {
                throw new NullObjectException(objectIdentity, friendlyHint, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the empty string as invalid exception.
        /// </summary>
        /// <param name="anyString">Any string.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="InvalidObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckEmptyStringAsInvalidException(this string anyString, string objectIdentity, FriendlyHint friendlyHint = null,
           [CallerMemberName] string memberName = null,
           [CallerFilePath] string sourceFilePath = null,
           [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (string.IsNullOrWhiteSpace(anyString))
            {
                throw new InvalidObjectException(objectIdentity, hint: friendlyHint, scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the empty crypto key.
        /// </summary>
        /// <param name="cryptoKey">The crypto key.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="NullObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckEmptyCryptoKey(this CryptoKey cryptoKey, string objectIdentity, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (cryptoKey.ByteValue == null)
            {
                throw new NullObjectException(objectIdentity, friendlyHint, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the empty cellphone number.
        /// </summary>
        /// <param name="cellphoneNumber">The cellphone number.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="NullObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckEmptyCellphoneNumber(this CellphoneNumber cellphoneNumber, string objectIdentity = null, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (cellphoneNumber == null || string.IsNullOrWhiteSpace(cellphoneNumber.Number))
            {
                throw new NullObjectException(objectIdentity.SafeToString(nameof(cellphoneNumber)), friendlyHint, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks null object.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="Beyova.Diagnostic.NullObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        /// <exception cref="NullObjectException"></exception>
        public static void CheckNullObject(this object anyObject, string objectIdentity, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (anyObject == null)
            {
                throw new NullObjectException(objectIdentity, friendlyHint, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the default value object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="anyObject">Any object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="NullObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckDefaultValueObject<T>(this T anyObject, string objectIdentity, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
            where T : struct, IConvertible
        {
            if ((anyObject as IConvertible).ToInt32(null) == 0)
            {
                throw new NullObjectException(objectIdentity, friendlyHint, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Validates the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetObject">The target object.</param>
        /// <param name="validator">The validator. Return true if passed validation.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="externalDataReference">The external data reference.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="InvalidObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void ValidateObject<T>(this T targetObject, Func<T, bool> validator, string objectIdentity, string reason, object externalDataReference = null, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            targetObject.CheckNullObject(objectIdentity);
            if (!validator(targetObject))
            {
                throw new InvalidObjectException(objectIdentity, data: new { targetObject, externalDataReference }, reason: reason, hint: friendlyHint, scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the null object as invalid.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="data">The external data reference.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="InvalidObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckNullObjectAsInvalid(this object targetObject, string objectIdentity, string reason = null, object data = null, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (targetObject == null)
            {
                throw new InvalidObjectException(objectIdentity, data: new { data }, reason: reason, hint: friendlyHint, scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the null object as resource not found.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="objectIdentifier">The object identifier.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="ResourceNotFoundException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckNullObjectAsResourceNotFound(this object targetObject, string objectIdentity, string objectIdentifier = null, FriendlyHint friendlyHint = null,
           [CallerMemberName] string memberName = null,
           [CallerFilePath] string sourceFilePath = null,
           [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (targetObject == null)
            {
                throw new ResourceNotFoundException(objectIdentity, objectIdentifier, friendlyHint: friendlyHint, scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Expects the not null object. This is used to check if a key obejct during business processing is null.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="data">The data.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="OperationFailureException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void ExpectNotNullObject(this object targetObject, string objectIdentity, object data = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (targetObject == null)
            {
                throw new OperationFailureException(data: new { data, expectNotNullObjectName = objectIdentity }, minor: "ExpectNotNullObject", scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the null resource.
        /// </summary>
        /// <param name="anyObject">Any object.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="resourceIdentity">The resource identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="Beyova.Diagnostic.ResourceNotFoundException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckNullResource(this object anyObject, string resourceName, string resourceIdentity, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (anyObject == null)
            {
                throw new ResourceNotFoundException(resourceName, resourceIdentity, friendlyHint, new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the zero enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumObject">The enum object.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="InvalidObjectException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckZeroEnum<T>(this T enumObject, string resourceName = null, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
            where T : struct, IConvertible
        {
            if (enumObject.EnumToInt32() == 0)
            {
                throw new InvalidObjectException(resourceName.SafeToString(typeof(T).FullName), data: new { type = typeof(T).FullName }, reason: "ZeroEnumValue", hint: friendlyHint, scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Checks the null or empty collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="objectIdentity">The object identity.</param>
        /// <param name="friendlyHint">The friendly hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <exception cref="InvalidObjectException">Null or empty collection</exception>
        /// <exception cref="ExceptionScene"></exception>
        public static void CheckNullOrEmptyCollection<T>(this IEnumerable<T> collection, string objectIdentity, FriendlyHint friendlyHint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!collection.HasItem())
            {
                throw new InvalidObjectException(objectIdentity, reason: "EmptyCollection", hint: friendlyHint, scene: new ExceptionScene
                {
                    FilePath = sourceFilePath,
                    LineNumber = sourceLineNumber,
                    MethodName = memberName
                });
            }
        }

        /// <summary>
        /// Creates the operation exception.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>
        /// OperationFailureException.
        /// </returns>
        /// <exception cref="Beyova.Diagnostic.OperationFailureException"></exception>
        /// <exception cref="ExceptionScene"></exception>
        public static OperationFailureException CreateOperationException(object data = null, string reason = null, FriendlyHint hint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new OperationFailureException(data: data, minor: reason, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the invalid object exception.
        /// </summary>
        /// <param name="objectIdentifier">The object identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>InvalidObjectException.</returns>
        public static InvalidObjectException CreateInvalidObjectException(string objectIdentifier, object data = null, string reason = null, FriendlyHint hint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new InvalidObjectException(objectIdentifier, data: data, reason: reason, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the operation forbidden exception.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="data">The data.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>OperationForbiddenException.</returns>
        public static OperationForbiddenException CreateOperationForbiddenException(string reason, Exception innerException = null, object data = null, FriendlyHint hint = null,
            [CallerMemberName] string actionName = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new OperationForbiddenException(actionName, reason, innerException, data, hint, new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the unauthorized token exception.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns></returns>
        public static UnauthorizedTokenException CreateUnauthorizedTokenException(string token, FriendlyHint hint = null,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new UnauthorizedTokenException(data: token, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }

        /// <summary>
        /// Creates the unsupported exception.
        /// </summary>
        /// <param name="objectIdentifier">The object identifier.</param>
        /// <param name="data">The data.</param>
        /// <param name="reason">The reason.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="hint">The hint.</param>
        /// <param name="memberName">Name of the member.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns>UnsupportedException.</returns>
        public static UnsupportedException CreateUnsupportedException(string objectIdentifier, object data = null, string reason = null, Exception innerException = null, FriendlyHint hint = null,
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = 0)
        {
            return new UnsupportedException(objectIdentifier, data: data, innerException: innerException, reason: reason, hint: hint, scene: new ExceptionScene
            {
                FilePath = sourceFilePath,
                LineNumber = sourceLineNumber,
                MethodName = memberName
            });
        }
    }
}