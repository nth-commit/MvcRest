using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.Rest.Extensions;
using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public class OrderableAttribute : Attribute, IActionFilter
    {
        private OrderRequest _orderRequest;

        private readonly IEnumerable<string> _allowedProperties;
        private readonly Type _type;

        public OrderableAttribute(Type type, string allowedField, params string[] additionalAllowedFields)
        {
            _type = type;

            var allowedFields = new List<string>() { allowedField };
            allowedFields.AddRange(additionalAllowedFields);
            _allowedProperties = allowedFields;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var restController = context.Controller as RestApiController;
            if (restController == null)
            {
                return;
            }

            var orderValue = context.HttpContext.Request.Query.FirstOrDefaultWithKey("order");
            if (!string.IsNullOrWhiteSpace(orderValue))
            {
                var descriptors = orderValue
                    .Split(',')
                    .Select(o => o.Trim())
                    .Select(o =>
                    {
                        var descendingRequested = o.StartsWith("-");
                        var ascendingRequested = o.StartsWith("+");
                        var unsignedPropertyName = descendingRequested || ascendingRequested ? o.Substring(1) : o;
                        return new OrderDescriptor(
                            _allowedProperties
                                .Where(p => p.ToLowerInvariant() == unsignedPropertyName.ToLowerInvariant())
                                .FirstOrDefault(),
                            !descendingRequested
                        );
                    });

                if (descriptors.Any(a => a.PropertyName == null))
                {
                    context.Result = new BadRequestResult();
                    return;
                }

                _orderRequest = new OrderRequest(_type, descriptors);
                restController.OrderRequest = _orderRequest;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
