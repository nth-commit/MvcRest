using NthCommit.AspNetCore.Mvc.Rest.Ordering;
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

        public static IQueryable<TResult> Order<TResult>(
            this IQueryable<TResult> queryable,
            OrderRequest orderRequest)
        {
            var orderDescriptors = orderRequest.OrderDescriptors;
            if (orderDescriptors.Count() == 0)
            {
                return queryable;
            }
            else
            {
                var result = queryable.OrderBy(orderDescriptors.First());
                foreach (var orderDescriptor in orderDescriptors.Skip(1))
                {
                    result = result.ThenBy(orderDescriptor);
                }
                return result;
            }
        }
    }
}
