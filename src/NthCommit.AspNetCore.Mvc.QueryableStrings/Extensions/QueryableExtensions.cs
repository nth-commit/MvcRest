using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(
            this IQueryable<T> source,
            string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            string propertyName)
        {
            return source.ThenBy(ToLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(
            this IOrderedQueryable<T> source,
            string propertyName)
        {
            return source.ThenByDescending(ToLambda<T>(propertyName));
        }


        #region Helpers

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }

        #endregion
    }
}
