using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class FilterableAttribute : Attribute, IActionFilter
    {
        private readonly string[] _propertyNames;

        public FilterMode Mode { get; set; } = FilterMode.Blacklist;

        public Type Type { get; set; }

        public FilterableAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        //private IEnumerable<string> GetPropertyNames(ActionExecutingContext context)
        //{
        //    context.HttpContext.Request.Query
        //        .Where()
        //}
    }
}
