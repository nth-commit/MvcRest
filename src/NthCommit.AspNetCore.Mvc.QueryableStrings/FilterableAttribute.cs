using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class FilterableAttribute : TypedQueryFilterAttribute
    {
        private readonly string[] _propertyNames;

        public FilterMode Mode { get; set; } = FilterMode.Blacklist;

        public FilterableAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        protected override void OnActionExecuting(ActionExecutingContext context, QueryableController controller, Type resolvedResourceType)
        {
            var properties = resolvedResourceType.GetCachedProperties();
            var normalizedPropertyNames = properties.Select(p => p.Name.ToLowerInvariant());
            var queryStringKeysNormalized = context.HttpContext.Request.Query.Select(kvp => kvp.Key.ToLowerInvariant());
            var requestedPropertyNames = normalizedPropertyNames.Intersect(queryStringKeysNormalized);

            var disallowedPropertyNames = _propertyNames
                .Select(p => p.ToLowerInvariant())
                .Fork(
                    Mode == FilterMode.Blacklist,
                    x => x.Intersect(requestedPropertyNames),
                    x => requestedPropertyNames.Except(x));

            if (disallowedPropertyNames.Count() > 0)
            {
                context.Result = new BadRequestResult();
                return;
            }

            controller.FilterQuery = new FilterQuery(requestedPropertyNames
                .Select(p => properties
                    .Where(pi => pi.Name.ToLowerInvariant() == p)
                    .Select(pi => pi.Name)
                    .First())
                .Select(p => new FilterQueryDescriptor(
                    p,
                    context.HttpContext.Request.Query.FirstOrDefaultWithKey(p),
                    FilterQueryDescriptorType.Equals)));
        }

        protected override void OnActionExecuted(ActionExecutedContext context, QueryableController controller, Type resolvedResourceType)
        {
        }


        #region Helpers

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
