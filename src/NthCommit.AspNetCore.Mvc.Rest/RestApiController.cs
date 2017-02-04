using Microsoft.AspNetCore.Mvc;
using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using NthCommit.AspNetCore.Mvc.Rest.Paging;
using NthCommit.AspNetCore.Mvc.Rest.Selecting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public abstract class RestApiController : Controller
    {
        public RestOrderQuery OrderQuery { get; set; }

        public RestSelectQuery SelectQuery { get; set; }

        public RestPageQuery PageQuery { get; set; }

        public RestQuery Query => new RestQuery()
        {
            OrderQuery = OrderQuery,
            SelectQuery = SelectQuery,
            PageQuery = PageQuery
        };

        public OkPagedResult Ok(IEnumerable items, int totalItems)
        {
            return new OkPagedResult(items, totalItems);
        }
    }
}
