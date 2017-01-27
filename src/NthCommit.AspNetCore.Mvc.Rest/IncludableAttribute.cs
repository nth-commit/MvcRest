﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NthCommit.AspNetCore.Mvc.Rest.Extensions;
using NthCommit.AspNetCore.Mvc.Rest.Includes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public class IncludableAttribute : Attribute, IActionFilter
    {
        private IncludeRequest _includeRequest;

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var restController = context.Controller as RestApiController;
            if (restController == null)
            {
                return;
            }

            var fields = context.HttpContext.Request.Query
                .WhereHasKey("fields")
                .SelectMany(v => v.Split(','))
                .Distinct();
            _includeRequest = new IncludeRequest(fields);
            restController.IncludeRequest = _includeRequest;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (_includeRequest == null || _includeRequest.Properties.Count() == 0)
            {
                return;
            }

            var okObjectResult = context.Result as OkObjectResult;
            if (okObjectResult != null)
            {
                var value = okObjectResult.Value;
                var enumerable = value as IEnumerable<object>;
                var newValue = enumerable == null ? CreateResult(value) : enumerable.Select(o => CreateResult(o));
                context.Result = new OkObjectResult(newValue);
            }
        }

        private object CreateResult(object inputResult)
        {
            return inputResult.GetType().GetProperties()
                .Where(p => _includeRequest.Properties.Contains(p.Name.ToLowerInvariant()))
                .ToDictionary(p => p.Name, p => p.GetValue(inputResult));
        }
    }
}
