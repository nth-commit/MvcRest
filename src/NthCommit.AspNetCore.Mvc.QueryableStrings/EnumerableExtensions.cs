using NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Paging;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Selecting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<object> Query<TResult>(
            this IEnumerable<TResult> enumerable,
            Query query)
        {
            return enumerable.AsQueryable().Query(query);
        }

        public static IEnumerable<TResult> Page<TResult>(
            this IEnumerable<TResult> enumerable,
            PageQuery pageQuery)
        {
            return enumerable.AsQueryable().Page(pageQuery);
        }

        public static IEnumerable<TResult> Order<TResult>(
            this IEnumerable<TResult> enumerable,
            OrderQuery orderQuery)
        {
            return enumerable.AsQueryable().Order(orderQuery);
        }

        public static IEnumerable<object> Select<TResult>(
            this IEnumerable<TResult> enumerable,
            SelectQuery selectQuery)
        {
            return enumerable.AsQueryable().Select(selectQuery);
        }
    }
}
