using Beyova.Api;
using Beyova.Diagnostic;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;

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
        /// The partial v iew wrapper
        /// </summary>
        protected const string partialViewWrapperView = "PartialViewWrapper";

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
        /// <param name="criteriaBoundaryEnsurance">The criteria boundary ensurance.</param>
        /// <returns></returns>
        protected virtual ActionResult QueryEntityView<TCriteria, TOutput>(Func<TCriteria, List<TOutput>> query, TCriteria criteria, string viewName, ActionResultType resultType = ActionResultType.PartialView, Action<TCriteria> criteriaBoundaryEnsurance = null)
            where TCriteria : class
        {
            try
            {
                query.CheckNullObject(nameof(query));
                criteria.CheckNullObject(nameof(criteria));
                viewName.CheckEmptyString(nameof(viewName));

                criteria = criteria.StandardizeWebObject();
                criteriaBoundaryEnsurance?.Invoke(criteria);

                var result = query(criteria);
                return resultType == ActionResultType.PartialView ? PartialView(viewName, result) : View(viewName, result) as ActionResult;
            }
            catch (Exception ex)
            {
                return HandleException(ex, resultType);
            }
        }

        /// <summary>
        /// Operates the entity json.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="invoke">The invoke.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterBoundaryEnsurance">The parameter boundary ensurance.</param>
        /// <returns></returns>
        protected virtual ActionResult OperateEntityJson<TParameter, TOutput>(Func<TParameter, TOutput> invoke, TParameter parameter, Action<TParameter> parameterBoundaryEnsurance = null)
            where TParameter : class
        {
            BaseException exception = null;
            object result = null;

            try
            {
                invoke.CheckNullObject(nameof(invoke));
                parameter.CheckNullObject(nameof(parameter));
                parameter = parameter.StandardizeWebObject();

                parameterBoundaryEnsurance?.Invoke(parameter);

                result = invoke(parameter);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { parameter });
            }

            this.PackageResponse(result, exception);
            return null;
        }

        /// <summary>
        /// Operates the entity json.
        /// </summary>
        /// <typeparam name="TParameter">The type of the parameter.</typeparam>
        /// <param name="invoke">The invoke.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="parameterBoundaryEnsurance">The parameter boundary ensurance.</param>
        /// <returns></returns>
        protected virtual ActionResult OperateEntityJson<TParameter>(Action<TParameter> invoke, TParameter parameter, Action<TParameter> parameterBoundaryEnsurance = null)
                where TParameter : class
        {
            BaseException exception = null;

            try
            {
                invoke.CheckNullObject(nameof(invoke));
                parameter.CheckNullObject(nameof(parameter));
                parameter = parameter.StandardizeWebObject();

                parameterBoundaryEnsurance?.Invoke(parameter);

                invoke(parameter);
            }
            catch (Exception ex)
            {
                exception = ex.Handle(new { parameter });
            }

            this.PackageResponse(null, exception);
            return null;
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
        /// <param name="considerEmptyKeyAsNew">if set to <c>true</c> [consider empty key as new].</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">T</exception>
        protected virtual ActionResult GetEntityView<T>(Func<Guid?, T> get, Guid? key, string viewName, ActionResultType resultType = ActionResultType.View, Func<T, bool> postValidation = null, bool considerEmptyKeyAsNew = false)
        {
            return GetEntityView<Guid?, T>(get, key, viewName, x => x.HasValue, resultType, postValidation, considerEmptyKeyAsNew);
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
        /// <param name="considerEmptyKeyAsNew">if set to <c>true</c> [consider empty key as new].</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">T</exception>
        protected virtual ActionResult GetEntityView<T>(Func<string, T> get, string key, string viewName, ActionResultType resultType = ActionResultType.View, Func<T, bool> postValidation = null, bool considerEmptyKeyAsNew = false)
        {
            return GetEntityView<string, T>(get, key, viewName, x => !string.IsNullOrWhiteSpace(x), resultType, postValidation, considerEmptyKeyAsNew);
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
        /// <param name="considerEmptyKeyAsNew">if set to <c>true</c> [consider empty key as new].</param>
        /// <param name="partialViewWrapper">The partial view wrapper.</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">TEntity</exception>
        private ActionResult GetEntityView<TInput, TEntity>(Func<TInput, TEntity> get, TInput key, string viewName, Func<TInput, bool> inputValidator, ActionResultType resultType, Func<TEntity, bool> postValidation, bool considerEmptyKeyAsNew = false, PartialViewWrapper partialViewWrapper = null)
        {
            try
            {
                get.CheckNullObject(nameof(get));
                viewName.CheckEmptyString(nameof(viewName));

                var hasKey = inputValidator(key);
                TEntity result = default(TEntity);

                if (hasKey)
                {
                    result = get.Invoke(key);

                    if (result == null)
                    {
                        // force to set considerEmptyKeyAsNew as false
                        // When KEY is specified and no related object found, return 404
                        considerEmptyKeyAsNew = false;
                    }
                }

                if (result != null || considerEmptyKeyAsNew)
                {
                    if (resultType == ActionResultType.PartialView)
                    {
                        return PartialView(viewName, result);
                    }
                    else
                    {
                        if (partialViewWrapper != null)
                        {
                            partialViewWrapper.Model = result;
                            partialViewWrapper.PartialView = viewName;
                            return View(partialViewWrapperView, partialViewWrapper);
                        }
                        else
                        {
                            return View(viewName, result);
                        }
                    }
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
        /// <param name="partialViewWrapper">The partial view wrapper.</param>
        /// <param name="criteriaBoundaryEnsurance">The criteria boundary ensurance.</param>
        /// <returns></returns>
        /// <exception cref="ResourceNotFoundException">TOutput</exception>
        protected virtual ActionResult GetEntityView<TCriteria, TOutput>(Func<TCriteria, List<TOutput>> query, Guid? key, string viewName, ActionResultType resultType = ActionResultType.View, Func<TOutput, bool> postValidation = null, PartialViewWrapper partialViewWrapper = null, Action<TCriteria> criteriaBoundaryEnsurance = null)
            where TCriteria : class, IIdentifier, new()
        {
            try
            {
                query.CheckNullObject(nameof(query));
                viewName.CheckEmptyString(nameof(viewName));

                var criteria = key.HasValue ? new TCriteria { Key = key } : null;
                if (criteria != null)
                {
                    criteriaBoundaryEnsurance?.Invoke(criteria);
                }
                var result = criteria != null ? query(criteria).SafeFirstOrDefault() : default(TOutput);

                if (result == null || (postValidation != null && !postValidation(result)))
                {
                    throw new ResourceNotFoundException(nameof(TOutput), key.ToString());
                }

                if (resultType == ActionResultType.PartialView)
                {
                    return PartialView(viewName, result);
                }
                else
                {
                    if (partialViewWrapper != null)
                    {
                        partialViewWrapper.Model = result;
                        partialViewWrapper.PartialView = viewName;
                        return View(partialViewWrapperView, partialViewWrapper);
                    }
                    else
                    {
                        return View(viewName, result);
                    }
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
        /// <param name="entityBoundaryEnsurance">The entity boundary ensurance.</param>
        /// <returns></returns>
        protected virtual ActionResult CreateOrUpdateEntity<TEntity, TOutput>(Func<TEntity, TOutput> action, TEntity entity, Action<TEntity> entityBoundaryEnsurance = null)
                where TEntity : class
        {
            try
            {
                action.CheckNullObject(nameof(action));
                entity.CheckNullObject(nameof(entity));

                entity = entity.StandardizeWebObject();
                entityBoundaryEnsurance?.Invoke(entity);

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
        /// <typeparam name="TEntityKey">The type of the entity.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="entityKey">The entity.</param>
        /// <param name="entityKeyBoundaryEnsurance">The entity key boundary ensurance.</param>
        /// <returns></returns>
        protected virtual ActionResult DeleteEntity<TEntityKey, TOutput>(Func<TEntityKey, TOutput> action, TEntityKey entityKey, Action<TEntityKey> entityKeyBoundaryEnsurance = null)
        {
            try
            {
                action.CheckNullObject(nameof(action));
                entityKey.CheckNullObject(nameof(entityKey));
                entityKeyBoundaryEnsurance?.Invoke(entityKey);

                return JsonNet(action(entityKey));
            }
            catch (Exception ex)
            {
                return HandleException(ex, ActionResultType.Json);
            }
        }

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <typeparam name="TEntityKey">The type of the entity key.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="entityKey">The entity.</param>
        /// <param name="entityKeyBoundaryEnsurance">The entity key boundary ensurance.</param>
        /// <returns></returns>
        protected virtual ActionResult DeleteEntity<TEntityKey>(Action<TEntityKey> action, TEntityKey entityKey, Action<TEntityKey> entityKeyBoundaryEnsurance = null)
        {
            try
            {
                action.CheckNullObject(nameof(action));
                entityKey.CheckNullObject(nameof(entityKey));

                entityKeyBoundaryEnsurance?.Invoke(entityKey);
                action(entityKey);
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