using NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Paging;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Selecting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public class Query
    {
        public OrderQuery OrderQuery { get; set; }

        public SelectQuery SelectQuery { get; set; }

        public PageQuery PageQuery { get; set; }

        public FilterQuery FilterQuery { get; set; }
    }
}
