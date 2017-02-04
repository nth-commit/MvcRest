using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.Rest.Extensions;
using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public class OrderableAttribute : Attribute, IActionFilter
    {
        private readonly IEnumerable<string> _allowedProperties;

        public Type Type { get; set; }

        public OrderableAttribute(params string[] allowedProperties)
        {
            _allowedProperties = allowedProperties;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var restController = context.Controller as RestApiController;
            if (restController == null)
            {
                return;
            }

            OrderRequest request = null;
            var orderValue = context.HttpContext.Request.Query.FirstOrDefaultWithKey("order") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(orderValue))
            {
                request = new OrderRequest(new List<OrderDescriptor>());
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

                request = new OrderRequest(descriptors.ToList());
            }
            
            restController.OrderRequest = request;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }


        #region Helpers

        private ConcurrentDictionary<Type, PropertyInfo[]> _propertyInfoByType = new ConcurrentDictionary<Type, PropertyInfo[]>();

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
                var producesResponseTypeAttr = context.Filters
                    .Select(f => f as ProducesResponseTypeAttribute)
                    .Where(f => f != null)
                    .FirstOrDefault();

                if (producesResponseTypeAttr == null)
                {
                    return null;
                }

                var valueType = producesResponseTypeAttr.Type;
                Type enumerableType = null;
                if (IsGenericIEnumerable(valueType))
                {
                    enumerableType = valueType;
                }
                else
                {
                    enumerableType = valueType
                        .GetInterfaces()
                        .Where(IsGenericIEnumerable)
                        .FirstOrDefault();
                }

                if (enumerableType == null)
                {
                    return null;
                }

                return enumerableType.GetGenericArguments().FirstOrDefault();
            }
            return Type;
        }

        private bool IsGenericIEnumerable(Type type)
        {
            return type.IsGenericType == true && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        #endregion
    }
}
