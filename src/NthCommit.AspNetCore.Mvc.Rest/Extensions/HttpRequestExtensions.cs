using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Extensions
{
    public static class HttpRequestExtensions
    {
        public static Uri GetUri(this HttpRequest request)
        {
            return new Uri($"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}");
        }
    }
}
