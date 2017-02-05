using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions;
using System.Reflection;
using System.Collections.Concurrent;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public abstract class TypedQueryFilterAttribute : QueryFilterAttribute
    {
        #region Abstract

        protected abstract void OnActionExecuted(ActionExecutedContext context, QueryableController controller, Type resolvedResourceType);

        protected abstract void OnActionExecuting(ActionExecutingContext context, QueryableController controller, Type resolvedResourceType);

        #endregion


        public Type Type { get; set; }

        protected override void OnActionExecuting(ActionExecutingContext context, QueryableController controller)
        {
            OnActionExecuting(context, controller, GetResolvedResourceType(context));
        }

        protected override void OnActionExecuted(ActionExecutedContext context, QueryableController controller)
        {
            OnActionExecuted(context, controller, GetResolvedResourceType(context));
        }


        #region Helpers

        private object _resolvedTypeLock = new object();
        private Type _resolvedResourceType = null;
        private bool _isTypeResolved = false;

        private Type GetResolvedResourceType(FilterContext context)
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

        private Type ResolveResourceType(FilterContext context)
        {
            if (Type == null)
            {
                var valueType = context.GetValueType();
                if (valueType == null)
                {
                    return null;
                }

                var enumerableType = valueType.GetGenericIEnumerableType();
                if (enumerableType == null)
                {
                    return valueType;
                }

                return enumerableType.GetGenericArguments().FirstOrDefault();
            }

            return Type;
        }

        #endregion
    }
}
