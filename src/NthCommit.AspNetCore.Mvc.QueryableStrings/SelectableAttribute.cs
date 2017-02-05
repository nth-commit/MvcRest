using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Selecting;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class SelectableAttribute : Attribute, IActionFilter
    {
        public Type Type { get; set; }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var queryableController = context.Controller as QueryableController;
            if (queryableController == null)
            {
                return;
            }

            var propertyNames = context.HttpContext.Request.Query.GetQueryValues("fields");
            if (!ArePropertiesValid(context, propertyNames))
            {
                context.Result = new BadRequestResult();
                return;
            }

            queryableController.SelectQuery = new SelectQuery(propertyNames);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var queryableController = context.Controller as QueryableController;
            if (queryableController == null)
            {
                return;
            }

            var propertyNames = queryableController.SelectQuery.PropertyNames;
            if (propertyNames.Count() == 0)
            {
                return;
            }

            var okObjectResult = context.Result as OkObjectResult;
            if (okObjectResult != null)
            {
                var value = okObjectResult.Value;
                var enumerable = value as IEnumerable<object>;
                var newValue = enumerable == null ?
                    CreateResult(value, propertyNames) :
                    enumerable.Select(o => CreateResult(o, propertyNames));
                okObjectResult.Value = newValue;
            }
        }


        #region Helpers

        private ConcurrentDictionary<Type, PropertyInfo[]> _propertyInfoByType = new ConcurrentDictionary<Type, PropertyInfo[]>();

        private bool ArePropertiesValid(ActionExecutingContext context, IEnumerable<string> requestedPropertyNames)
        {
            var resourceType = GetResourceType(context);
            if (resourceType == null)
            {
                return true; // Nothing to validate against.
            }

            var properties = _propertyInfoByType.GetOrAdd(resourceType, t => t.GetProperties());
            var normalizedPropertyNames = properties.Select(p => p.Name.ToLowerInvariant());
            var normalizedRequestedPropertyNames = requestedPropertyNames.Select(p => p.ToLowerInvariant());

            var invalidPropertyNames = normalizedRequestedPropertyNames.Except(normalizedPropertyNames);
            return invalidPropertyNames.Count() == 0;
        }

        private object _resolvedTypeLock = new object();
        private Type _resolvedResourceType = null;
        private bool _isTypeResolved = false;

        private Type GetResourceType(ActionExecutingContext context)
        {
            if (!_isTypeResolved)
            {
                lock (_resolvedTypeLock)
                {
                    if (!_isTypeResolved)
                    {
                        _resolvedResourceType = ResolveResourceType(context);
                        _isTypeResolved = true;
                    }
                }
            }

            return _resolvedResourceType;
        }

        private Type ResolveResourceType(ActionExecutingContext context)
        {
            if (Type == null)
            {
                var producesResponseTypeAttr = context.Filters
                    .Select(f => f as ProducesResponseTypeAttribute)
                    .Where(f => f != null)
                    .FirstOrDefault();

                if (producesResponseTypeAttr == null)
                {
                    return null;
                }

                var valueType = producesResponseTypeAttr.Type;
                var enumerableType = GetTypeAsGenericIEnumerable(valueType);
                if (enumerableType == null)
                {
                    return valueType;
                }

                return enumerableType.GetGenericArguments().FirstOrDefault();
            }

            return Type;
        }
        
        private Type GetTypeAsGenericIEnumerable(Type type)
        {
            if (IsGenericIEnumerable(type))
            {
                return type;
            }
            else
            {
                return type
                    .GetInterfaces()
                    .Where(IsGenericIEnumerable)
                    .FirstOrDefault();
            }
        }

        private bool IsGenericIEnumerable(Type type)
        {
            return type.IsGenericType == true && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        private dynamic CreateResult(object inputResult, IEnumerable<string> propertyNames)
        {
            // TODO: Performance?
            return inputResult.GetType().GetProperties()
                .Where(p => propertyNames.Contains(p.Name.ToLowerInvariant()))
                .ToDictionary(p => p.Name, p => p.GetValue(inputResult));
        }

        #endregion
    }
}
