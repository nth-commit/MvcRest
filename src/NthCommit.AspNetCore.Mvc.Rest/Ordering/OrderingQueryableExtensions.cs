using NthCommit.AspNetCore.Mvc.Rest.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Ordering
{
    public static class OrderingQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            OrderDescriptor orderDescriptor)
        {
            return source.OrderBy(orderDescriptor.PropertyName, orderDescriptor.IsAscending);
        }

        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            string propertyName,
            bool isAscending)
        {
            return isAscending ? source.OrderBy(propertyName) : source.OrderByDescending(propertyName);
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            OrderDescriptor orderDescriptor)
        {
            return source.ThenBy(orderDescriptor.PropertyName, orderDescriptor.IsAscending);
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            string propertyName,
            bool isAscending)
        {
            return isAscending ? source.OrderBy(propertyName) : source.OrderByDescending(propertyName);
        }
    }
}
