using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Selecting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class SelectableAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var queryableController = context.Controller as QueryableController;
            if (queryableController == null)
            {
                return;
            }

            var fields = context.HttpContext.Request.Query.GetQueryValues("fields");
            queryableController.SelectQuery = new SelectQuery(fields);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
