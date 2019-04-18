using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;
using Beyova.Api;
using Beyova.Diagnostic;

namespace Beyova.Web
{
    /// <summary>
    ///
    /// </summary>
    [RestApiContextConsistence]
    [TokenRequired]
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// The default error partial view
        /// </summary>
        protected const string defaultErrorPartialView = "_Exception";

        /// <summary>
        /// The default error view
        /// </summary>
        protected const string defaultErrorView = "Error";

        /// <summary>
        /// Gets the error page route.
        /// </summary>
        /// <param name="exceptionKey">The exception key.</param>
        /// <returns></returns>
        protected abstract RouteValueDictionary GetErrorPageRoute(Guid? exceptionKey);

        /// <summary>
        /// Handles the exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="operationName">Name of the operation.</param>
        /// <param name="sourceFilePath">The source file path.</param>
        /// <param name="sourceLineNumber">The source line number.</param>
        /// <returns></returns>
        protected virtual ActionResult HandleException(Exception ex, ActionResultType resultType = ActionResultType.PartialView,
            [CallerMemberName] string operationName = null,
            [CallerFilePath] string sourceFilePath = null,
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            var exception = ex?.Handle(new { routeData = ControllerContext.RequestContext.RouteData.ToJson() }, operationName: operationName, sourceFilePath: sourceFilePath, sourceLineNumber: sourceLineNumber);
            ActionResult result = null;

            if (exception != null)
            {
                if (exception.Code.Major == ExceptionCode.MajorCode.OperationFailure)
                {
                    Framework.ApiTracking?.LogException(exception.ToExceptionInfo());
                }

                switch (resultType)
                {
                    case ActionResultType.PartialView:
                        result = PartialView(defaultErrorPartialView, exception.ToSimpleExceptionInfo());
                        break;

                    case ActionResultType.View:
                        result = View(defaultErrorView, exception.ToSimpleExceptionInfo());
                        break;

                    case ActionResultType.Default:
                    case ActionResultType.Json:
                    default:
                        this.PackageResponse(null, exception);
                        result = null;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Queries the entity.
        /// </summary>
        /// <typeparam name="TCriteria">The type of the criteria.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The criteria.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <returns></returns>
        protected virtual ActionResult QueryEntityView<TCriteria, TOutput>(Func<TCriteria, List<TOutput>> query, TCriteria criteria, string viewName, ActionResultType resultType = ActionResultType.PartialView)
        {
            try
            {
                query.CheckNullObject(nameof(query));
                criteria.CheckNullObject(nameof(criteria));
                viewName.CheckEmptyString(nameof(viewName));

                var result = query(criteria);
                return resultType == ActionResultType.PartialView ? PartialView(viewName, result) : View(viewName, result) as ActionResult;
            }
            catch (Exception ex)
            {
                return HandleException(ex, resultType);
            }
        }

        /// <summary>
        /// Gets the entity view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="get">The get.</param>
        /// <param name="key">The key.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="postValidation">The post validation.</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">T</exception>
        protected virtual ActionResult GetEntityView<T>(Func<Guid?, T> get, Guid? key, string viewName, ActionResultType resultType = ActionResultType.View, Func<T, bool> postValidation = null)
        {
            return GetEntityView<Guid?, T>(get, key, viewName, x => x.HasValue, resultType, postValidation);
        }

        /// <summary>
        /// Gets the entity view.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="get">The get.</param>
        /// <param name="key">The key.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="postValidation">The post validation.</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">T</exception>
        protected virtual ActionResult GetEntityView<T>(Func<string, T> get, string key, string viewName, ActionResultType resultType = ActionResultType.View, Func<T, bool> postValidation = null)
        {
            return GetEntityView<string, T>(get, key, viewName, x => !string.IsNullOrWhiteSpace(x), resultType, postValidation);
        }

        /// <summary>
        /// Gets the entity view.
        /// </summary>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="get">The get.</param>
        /// <param name="key">The key.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="inputValidator">The input validator.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="postValidation">The post validation.</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">TEntity</exception>
        private ActionResult GetEntityView<TInput, TEntity>(Func<TInput, TEntity> get, TInput key, string viewName, Func<TInput, bool> inputValidator, ActionResultType resultType, Func<TEntity, bool> postValidation)
        {
            try
            {
                get.CheckNullObject(nameof(get));
                viewName.CheckEmptyString(nameof(viewName));

                var result = inputValidator(key) ? get.Invoke(key) : default(TEntity);
                if (result != null && (postValidation == null || postValidation(result)))
                {
                    return resultType == ActionResultType.PartialView ? PartialView(viewName, result) : View(viewName, result) as ActionResult;
                }
                else
                {
                    throw new ResourceNotFoundException(nameof(TEntity), key.ToString());
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex, resultType);
            }
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <typeparam name="TCriteria">The type of the criteria.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="key">The key.</param>
        /// <param name="viewName">Name of the view.</param>
        /// <param name="resultType">Type of the result.</param>
        /// <param name="postValidation">The post validation.</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">TOutput</exception>
        protected virtual ActionResult GetEntityView<TCriteria, TOutput>(Func<TCriteria, List<TOutput>> query, Guid? key, string viewName, ActionResultType resultType = ActionResultType.View, Func<TOutput, bool> postValidation = null)
            where TCriteria : IIdentifier, new()
        {
            try
            {
                query.CheckNullObject(nameof(query));
                viewName.CheckEmptyString(nameof(viewName));

                var result = key.HasValue ? query(new TCriteria { Key = key }).SafeFirstOrDefault() : default(TOutput);
                if (result != null && (postValidation == null || postValidation(result)))
                {
                    return resultType == ActionResultType.PartialView ? PartialView(viewName, result) : View(viewName, result) as ActionResult;
                }
                else
                {
                    throw new ResourceNotFoundException(nameof(TOutput), key.ToString());
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex, resultType);
            }
        }

        /// <summary>
        /// Creates the or update entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual ActionResult CreateOrUpdateEntity<TEntity, TOutput>(Func<TEntity, TOutput> action, TEntity entity)
        {
            try
            {
                action.CheckNullObject(nameof(action));
                entity.CheckNullObject(nameof(entity));

                return JsonNet(action(entity));
            }
            catch (Exception ex)
            {
                return HandleException(ex, ActionResultType.Json);
            }
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual ActionResult DeleteEntity<TEntity, TOutput>(Func<TEntity, TOutput> action, TEntity entity)
        {
            try
            {
                action.CheckNullObject(nameof(action));
                entity.CheckNullObject(nameof(entity));

                return JsonNet(action(entity));
            }
            catch (Exception ex)
            {
                return HandleException(ex, ActionResultType.Json);
            }
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        protected virtual ActionResult DeleteEntity<TEntity>(Action<TEntity> action, TEntity entity)
        {
            try
            {
                action.CheckNullObject(nameof(action));
                entity.CheckNullObject(nameof(entity));

                action(entity);
                return JsonNet(DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                return HandleException(ex, ActionResultType.Json);
            }
        }

        /// <summary>
        /// Jsons the net.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        protected JsonNetResult JsonNet(object obj)
        {
            return ControllerExtension.JsonNet(this, obj);
        }
    }
}