using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions
{
    public static class ActionExecutingContextExtensions
    {

        public static Type GetValueType(this ActionExecutingContext context)
        {
            var producesResponseTypeAttr = context.Filters
                .Select(f => f as ProducesResponseTypeAttribute)
                .Where(f => f != null)
                .FirstOrDefault();

            if (producesResponseTypeAttr == null)
            {
                return null;
            }

            return producesResponseTypeAttr.Type;
        }
    }
}
