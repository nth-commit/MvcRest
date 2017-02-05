using Microsoft.AspNetCore.Mvc;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Paging;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Selecting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public abstract class QueryableController : Controller
    {
        public OrderQuery OrderQuery { get; set; }

        public SelectQuery SelectQuery { get; set; }

        public PageQuery PageQuery { get; set; }

        public FilterQuery FilterQuery { get; set; }

        public Query Query => new Query()
        {
            OrderQuery = OrderQuery,
            SelectQuery = SelectQuery,
            PageQuery = PageQuery,
            FilterQuery = FilterQuery
        };
    }
}
