using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using NthCommit.AspNetCore.Mvc.Rest.Extensions;
using NthCommit.AspNetCore.Mvc.Rest.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public class PageableAttribute : Attribute, IActionFilter
    {
        private const string DefaultPageNumberKey = "page";
        private const string DefaultPageSizeKey = "page-size";
        private const int DefaultPageNumber = 1;
        private const int DefaultDefaultPageSize = 10;
        private const int DefaultMaxPageSize = 50;

        private RestPageQuery _pageRequest;

        public int MaximumPageSize { get; set; } = DefaultMaxPageSize;

        public int DefaultPageSize { get; set; } = DefaultDefaultPageSize;

        public string PageNumberKey { get; set; } = DefaultPageNumberKey;

        public string PageSizeKey { get; set; } = DefaultPageSizeKey;


        public void OnActionExecuting(ActionExecutingContext context)
        {
            var restController = context.Controller as RestApiController;
            if (restController == null)
            {
                return;
            }

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

            _pageRequest = new RestPageQuery(pageNumber, requestedPageSize);
            restController.PageQuery = _pageRequest;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_pageRequest == null)
            {
                return;
            }

            var okPagedResult = context.Result as OkPagedResult;
            if (okPagedResult != null)
            {
                AddTotalCountHeader(context, okPagedResult);
                AddLinkHeader(context, okPagedResult);
            }
        }


        #region Helpers

        private void AddTotalCountHeader(ActionExecutedContext context, OkPagedResult result)
        {
            var totalItems = result.TotalItems;
            context.HttpContext.Response.Headers.Add("X-Total-Count", new StringValues(totalItems.ToString()));
        }

        private void AddLinkHeader(ActionExecutedContext context, OkPagedResult result)
        {
            var pageNumber = _pageRequest.Number;
            var totalPages = (int)Math.Ceiling(result.TotalItems / (double)_pageRequest.Size);

            var links = new List<LinkHeaderElement>()
            {
                new LinkHeaderElement()
                {
                    Rel = "first",
                    Url = GetPageUrl(context, 1)
                },
                new LinkHeaderElement()
                {
                    Rel = "last",
                    Url = GetPageUrl(context, totalPages)
                }
            };

            if (totalPages > 1)
            {
                if (pageNumber < totalPages)
                {
                    links.Add(new LinkHeaderElement()
                    {
                        Rel = "next",
                        Url = GetPageUrl(context, pageNumber + 1)
                    });
                }

                if (pageNumber > 1)
                {
                    links.Add(new LinkHeaderElement()
                    {
                        Rel = "prev",
                        Url = GetPageUrl(context, pageNumber - 1)
                    });
                }
            }

            var linksHeaderValues = links.Select(l => $"<{l.Url}>; rel=\"{l.Rel}\"");
            context.HttpContext.Response.Headers.Add("Link", string.Join($", ", linksHeaderValues));
        }

        private string GetPageUrl(ActionExecutedContext context, int pageNumber)
        {
            var request = context.HttpContext.Request;
            var requestQueryDictionary = QueryHelpers.ParseQuery(request.QueryString.Value);
            requestQueryDictionary[DefaultPageNumberKey] = new StringValues(pageNumber.ToString());
            requestQueryDictionary[DefaultPageSizeKey] = new StringValues(_pageRequest.Size.ToString());
            return QueryHelpers.AddQueryString(
                new Uri($"{request.Scheme}://{request.Host}{request.Path}").ToString(),
                requestQueryDictionary
                    .Where(kvp => kvp.Value.Count() > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.FirstOrDefault()));
        }

        private class LinkHeaderElement
        {
            public string Rel { get; set; }

            public string Url { get; set; }
        }

        #endregion
    }
}
