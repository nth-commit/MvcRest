using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public abstract class QueryFilterAttribute : Attribute, IActionFilter
    {
        protected abstract void OnActionExecuting(ActionExecutingContext context, QueryableController controller);

        protected abstract void OnActionExecuted(ActionExecutedContext context, QueryableController controller);
        
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var queryableController = context.Controller as QueryableController;
            if (queryableController == null)
            {
                return;
            }

            OnActionExecuting(context, queryableController);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var queryableController = context.Controller as QueryableController;
            if (queryableController == null)
            {
                return;
            }

            OnActionExecuted(context, queryableController);
        }
    }
}
