using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NthCommit.AspNetCore.Mvc.Rest.Includes;
using NthCommit.AspNetCore.Mvc.Rest.Pageable;
using System.Collections;
using NthCommit.AspNetCore.Mvc.Rest.Ordering;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public abstract class RestApiController : Controller
    {
        public object Filter { get; set; }

        public object Expand { get; set; }

        public OrderRequest OrderRequest { get; set; }

        public IncludeRequest IncludeRequest { get; set; }

        public PageRequest PageRequest { get; set; }

        public OkPagedResult Ok(IEnumerable items, int totalItems)
        {
            return new OkPagedResult(items, totalItems);
        }
    }
}
