using NthCommit.AspNetCore.Mvc.Rest.Pageable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public static class QueryableExtensions
    {
        public static IQueryable<TResult> Page<TResult>(
            this IQueryable<TResult> queryable,
            PageRequest pageRequest)
        {
            return queryable
                .Skip((pageRequest.Number - 1) * pageRequest.Size)
                .Take(pageRequest.Size);
        }
    }
}
