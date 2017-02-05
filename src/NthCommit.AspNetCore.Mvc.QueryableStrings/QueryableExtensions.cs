using NthCommit.AspNetCore.Mvc.QueryableStrings.Paging;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Selecting;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public static class QueryableExtensions
    {
        public static IQueryable<object> Query<TResult>(
            this IQueryable<TResult> queryable,
            Query query)
        {
            var typedResult = queryable;

            if (query.FilterQuery != null)
            {
                typedResult = typedResult.Filter(query.FilterQuery);
            }

            if (query.OrderQuery != null)
            {
                typedResult = typedResult.Order(query.OrderQuery);
            }

            if (query.PageQuery != null)
            {
                typedResult = typedResult.Page(query.PageQuery);
            }

            return query.SelectQuery == null ?
                typedResult.Cast<object>() :
                typedResult.Select(query.SelectQuery);
        }

        public static IQueryable<TResult> Page<TResult>(
            this IQueryable<TResult> queryable,
            PageQuery pageQuery)
        {
            return queryable
                .Skip((pageQuery.Number - 1) * pageQuery.Size)
                .Take(pageQuery.Size);
        }

        public static IQueryable<TResult> Order<TResult>(
            this IQueryable<TResult> queryable,
            OrderQuery orderQuery)
        {
            var descriptors = orderQuery.Descriptors;
            if (descriptors.Count() == 0)
            {
                return queryable;
            }

            var result = queryable.OrderBy(descriptors.First());
            foreach (var orderDescriptor in descriptors.Skip(1))
            {
                result = result.ThenBy(orderDescriptor);
            }
            return result;
        }

        public static IQueryable<TResult> Filter<TResult>(
            this IQueryable<TResult> queryable,
            FilterQuery filterQuery)
        {
            var descriptors = filterQuery.Descriptors;
            if (descriptors.Count() == 0)
            {
                return queryable;
            }

            var whereStr = string.Join(" And ", descriptors.Select(d => $"{d.PropertyName}=\"{d.Value}\""));
            return queryable.Where(whereStr);
        }

        public static IQueryable<object> Select<TResult>(
            this IQueryable<TResult> queryable,
            SelectQuery selectQuery)
        {
            var properties = selectQuery.PropertyNames;
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
