using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Paging
{
    public class OkPagedResult : OkObjectResult
    {
        public int TotalItems { get; private set; }

        public OkPagedResult(IEnumerable items, int totalItems) : base(items)
        {
            TotalItems = totalItems;
        }
    }
}
