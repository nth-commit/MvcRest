using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using NthCommit.AspNetCore.Mvc.Rest.Paging;
using NthCommit.AspNetCore.Mvc.Rest.Selecting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<object> Query<TResult>(
            this IEnumerable<TResult> enumerable,
            RestQuery query)
        {
            return enumerable.AsQueryable().Query(query);
        }

        public static IEnumerable<TResult> Page<TResult>(
            this IEnumerable<TResult> enumerable,
            RestPageQuery pageQuery)
        {
            return enumerable.AsQueryable().Page(pageQuery);
        }

        public static IEnumerable<TResult> Order<TResult>(
            this IEnumerable<TResult> enumerable,
            RestOrderQuery orderQuery)
        {
            return enumerable.AsQueryable().Order(orderQuery);
        }

        public static IEnumerable<object> Select<TResult>(
            this IEnumerable<TResult> enumerable,
            RestSelectQuery selectQuery)
        {
            return enumerable.AsQueryable().Select(selectQuery);
        }
    }
}
