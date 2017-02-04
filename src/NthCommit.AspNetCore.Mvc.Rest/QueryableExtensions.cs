using NthCommit.AspNetCore.Mvc.Rest.Paging;
using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using NthCommit.AspNetCore.Mvc.Rest.Selecting;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public static class QueryableExtensions
    {
        public static IQueryable<object> Query<TResult>(
            this IQueryable<TResult> queryable,
            RestQuery query)
        {
            return queryable
                .Order(query.OrderQuery)
                .Page(query.PageQuery)
                .Select(query.SelectQuery);
        }

        public static IQueryable<TResult> Page<TResult>(
            this IQueryable<TResult> queryable,
            RestPageQuery pageQuery)
        {
            return queryable
                .Skip((pageQuery.Number - 1) * pageQuery.Size)
                .Take(pageQuery.Size);
        }

        public static IQueryable<TResult> Order<TResult>(
            this IQueryable<TResult> queryable,
            RestOrderQuery orderQuery)
        {
            var orderDescriptors = orderQuery.OrderDescriptors;
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
            RestSelectQuery selectQuery)
        {
            var properties = selectQuery.Properties;
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
