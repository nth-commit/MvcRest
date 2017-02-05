using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class OrderableAttribute : TypedQueryFilterAttribute, IActionFilter
    {
        private readonly IEnumerable<string> _allowedProperties;

        public OrderableAttribute(params string[] allowedProperties)
        {
            _allowedProperties = allowedProperties;
        }

        protected override void OnActionExecuting(ActionExecutingContext context, QueryableController controller, Type resolvedResourceType)
        {
            OrderQuery request = null;
            var orderValue = context.HttpContext.Request.Query.FirstOrDefaultWithKey("orderby") ?? string.Empty;
            if (string.IsNullOrWhiteSpace(orderValue))
            {
                request = new OrderQuery(new List<OrderQueryDescriptor>());
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
                        return new OrderQueryDescriptor(GetPropertyName(resolvedResourceType, unsignedPropertyName), !descendingRequested);
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

        protected override void OnActionExecuted(ActionExecutedContext context, QueryableController controller, Type resolvedResourceType)
        {
        }


        #region Helpers
        
        private string GetPropertyName(Type resolvedResourceType, string requestedPropertyName)
        {
            if (resolvedResourceType == null)
            {
                throw new Exception("Unable to resolve type of response value.");
            }
            
            var matchedPropertyName = resolvedResourceType
                .GetCachedProperties()
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

        #endregion
    }
}
