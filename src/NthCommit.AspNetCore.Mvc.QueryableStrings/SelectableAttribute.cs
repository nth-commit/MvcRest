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
    public class SelectableAttribute : TypedQueryFilterAttribute
    {
        protected override void OnActionExecuting(ActionExecutingContext context, QueryableController controller, Type resolvedResourceType)
        {
            var propertyNames = context.HttpContext.Request.Query.GetQueryValues("fields");
            if (!ArePropertiesValid(resolvedResourceType, propertyNames))
            {
                context.Result = new BadRequestResult();
                return;
            }

            controller.SelectQuery = new SelectQuery(propertyNames);
        }

        protected override void OnActionExecuted(ActionExecutedContext context, QueryableController controller, Type resolvedResourceType)
        {
            var propertyNames = controller.SelectQuery.PropertyNames;
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

        private bool ArePropertiesValid(Type resolvedResourceType, IEnumerable<string> requestedPropertyNames)
        {
            if (resolvedResourceType == null)
            {
                return true; // Nothing to validate against.
            }

            var properties = _propertyInfoByType.GetOrAdd(resolvedResourceType, t => t.GetProperties());
            var normalizedPropertyNames = properties.Select(p => p.Name.ToLowerInvariant());
            var normalizedRequestedPropertyNames = requestedPropertyNames.Select(p => p.ToLowerInvariant());

            var invalidPropertyNames = normalizedRequestedPropertyNames.Except(normalizedPropertyNames);
            return invalidPropertyNames.Count() == 0;
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
