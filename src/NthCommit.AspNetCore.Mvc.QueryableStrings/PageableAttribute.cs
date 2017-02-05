using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class PageableAttribute : QueryFilterAttribute, IActionFilter
    {
        private const string DefaultPageNumberKey = "page";
        private const string DefaultPageSizeKey = "page-size";
        private const int DefaultPageNumber = 1;
        private const int DefaultDefaultPageSize = 10;
        private const int DefaultMaxPageSize = 50;

        public int MaximumPageSize { get; set; } = DefaultMaxPageSize;

        public int DefaultPageSize { get; set; } = DefaultDefaultPageSize;

        public string PageNumberKey { get; set; } = DefaultPageNumberKey;

        public string PageSizeKey { get; set; } = DefaultPageSizeKey;

        protected override void OnActionExecuting(ActionExecutingContext context, QueryableController controller)
        {
            int pageNumber;
            var pageNumberStr = context.HttpContext.Request.Query.FirstOrDefaultWithKey(DefaultPageNumberKey);
            if (string.IsNullOrWhiteSpace(pageNumberStr))
            {
                pageNumber = DefaultPageNumber;
            }
            else if (int.TryParse(pageNumberStr, out pageNumber))
            {
                if (pageNumber < 1)
                {
                    context.Result = new BadRequestResult();
                    return;
                }
            }
            else
            {
                context.Result = new BadRequestResult();
                return;
            }

            int requestedPageSize = DefaultPageSize;
            var requestedPageSizeStr = context.HttpContext.Request.Query.FirstOrDefaultWithKey(DefaultPageSizeKey);
            if (!string.IsNullOrWhiteSpace(requestedPageSizeStr))
            {
                int.TryParse(requestedPageSizeStr, out requestedPageSize);
            }

            if (requestedPageSize > MaximumPageSize)
            {
                context.Result = new BadRequestResult();
                return;
            }

            controller.PageQuery = new PageQuery(pageNumber, requestedPageSize);
        }

        protected override void OnActionExecuted(ActionExecutedContext context, QueryableController controller)
        {
        }
    }
}
