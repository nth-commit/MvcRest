using NthCommit.AspNetCore.Mvc.Rest.Includes;
using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using NthCommit.AspNetCore.Mvc.Rest.Pageable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public static class QueryableExtensions
    {
        public static IQueryable<object> Query<TResult>(
            this IQueryable<TResult> queryable,
            RestQuery query)
        {
            return queryable
                .Order(query.OrderRequest)
                .Page(query.PageRequest)
                .Select(query.IncludeRequest);
        }

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

            var result = queryable.OrderBy(orderDescriptors.First());
            foreach (var orderDescriptor in orderDescriptors.Skip(1))
            {
                result = result.ThenBy(orderDescriptor);
            }
            return result;
        }

        public static IQueryable<object> Select<TResult>(
            this IQueryable<TResult> queryable,
            IncludeRequest includeRequest)
        {
            var properties = includeRequest.Properties;
            if (properties.Count() == 0)
            {
                return queryable.Cast<object>();
            }

            return queryable
                .Select($"new ({string.Join(", ", properties.ToArray())})")
                .Cast<object>();
        }
    }
}
