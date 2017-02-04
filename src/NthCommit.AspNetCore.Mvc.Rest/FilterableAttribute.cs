using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public class FilterableAttribute : Attribute, IActionFilter
    {

        private readonly IEnumerable<string> _allowedProperties;
        private readonly Type _type;

        public FilterableAttribute(params string[] allowedProperties)
            : this(null, allowedProperties) { }

        public FilterableAttribute(Type type, params string[] allowedProperties)
        {
            _type = type;
            _allowedProperties = allowedProperties;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var type = GetType(context);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        


        private Type GetType(ActionExecutingContext context)
        {
            if (_type != null)
            {
                return _type;
            }

            return context.Filters
                .Select(f => f as ProducesResponseTypeAttribute)
                .Where(f => f != null)
                .Select(f => f.Type)
                .FirstOrDefault();
        }
    }
}
