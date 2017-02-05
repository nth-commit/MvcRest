using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class OrderableAttribute : QueryFilterAttribute, IActionFilter
    {
        private readonly IEnumerable<string> _allowedProperties;

        public Type Type { get; set; }

        public OrderableAttribute(params string[] allowedProperties)
        {
            _allowedProperties = allowedProperties;
        }

        protected override void OnActionExecuting(ActionExecutingContext context, QueryableController controller)
        {
            OrderQuery request = null;
            var orderValue = context.HttpContext.Request.Query.FirstOrDefaultWithKey("orderby") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(orderValue))
            {
                request = new OrderQuery(new List<OrderDescriptor>());
            }
            else
            {
                var descriptors = orderValue
                    .Split(',')
                    .Select(o => o.Trim())
                    .Select(o =>
                    {
                        var descendingRequested = o.StartsWith("-");
                        var ascendingRequested = o.StartsWith("+");
                        var unsignedPropertyName = descendingRequested || ascendingRequested ? o.Substring(1) : o;
                        return new OrderDescriptor(GetPropertyName(context, unsignedPropertyName), !descendingRequested);
                    });

                if (descriptors.Any(a => a.PropertyName == null))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                request = new OrderQuery(descriptors.ToList());
            }

            controller.OrderQuery = request;
        }

        protected override void OnActionExecuted(ActionExecutedContext context, QueryableController controller)
        {
        }


        #region Helpers

        private static ConcurrentDictionary<Type, PropertyInfo[]> _propertyInfoByType = new ConcurrentDictionary<Type, PropertyInfo[]>();

        private string GetPropertyName(ActionExecutingContext context, string requestedPropertyName)
        {
            var type = GetResolvedType(context);
            var properties = _propertyInfoByType.GetOrAdd(type, t => t.GetProperties());
            var matchedPropertyName = properties
                .Where(p => p.Name.ToLowerInvariant() == requestedPropertyName.ToLowerInvariant())
                .Select(p => p.Name)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                return null;
            }

            if (_allowedProperties.Count() > 0 &&
                !_allowedProperties.Any(p => p.ToLowerInvariant() == matchedPropertyName.ToLowerInvariant()))
            {
                return null;
            }

            return matchedPropertyName;
        }

        private object _resolvedTypeLock = new object();
        private Type _resolvedType = null;
        private bool? _isResolvedTypeInvalid = null;

        private Type GetResolvedType(ActionExecutingContext context)
        {
            if (!HasResolvedType())
            {
                lock (_resolvedTypeLock)
                {
                    if (!HasResolvedType())
                    {
                        _resolvedType = ResolveType(context);
                        if (_resolvedType == null)
                        {
                            _isResolvedTypeInvalid = true;
                        }
                    }
                }
            }

            if (_isResolvedTypeInvalid.GetValueOrDefault())
            {
                throw new Exception("ProducesResponseType must return value of type IEnumerable<>");
            }

            return _resolvedType;            
        }

        private bool HasResolvedType() => _resolvedType != null || _isResolvedTypeInvalid.HasValue;

        private Type ResolveType(ActionExecutingContext context)
        {
            if (Type == null)
            {
                var valueType = context.GetValueType();
                if (valueType == null)
                {
                    return null;
                }

                Type enumerableType = valueType.GetGenericIEnumerableType();
                if (enumerableType == null)
                {
                    return null;
                }

                return enumerableType.GetGenericArguments().FirstOrDefault();
            }
            return Type;
        }

        

        #endregion
    }
}
