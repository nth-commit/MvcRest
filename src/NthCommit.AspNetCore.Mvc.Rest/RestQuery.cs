using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using NthCommit.AspNetCore.Mvc.Rest.Paging;
using NthCommit.AspNetCore.Mvc.Rest.Selecting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public class RestQuery
    {
        public RestOrderQuery OrderQuery { get; set; }

        public RestSelectQuery SelectQuery { get; set; }

        public RestPageQuery PageQuery { get; set; }
    }
}
