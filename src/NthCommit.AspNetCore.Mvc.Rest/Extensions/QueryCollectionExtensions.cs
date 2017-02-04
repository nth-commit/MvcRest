using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Extensions
{
    internal static class QueryCollectionExtensions
    {
        public static IEnumerable<string> WhereHasKey(
            this IQueryCollection queryCollection,
            string key)
        {
            return queryCollection
                .Where(kvp => kvp.Key.ToLowerInvariant() == key.ToLowerInvariant())
                .Select(kvp => kvp.Value)
                .FirstOrDefault();
        }

        public static string FirstOrDefaultWithKey(
            this IQueryCollection queryCollection,
            string key)
        {
            return queryCollection.WhereHasKey(key).FirstOrDefault();
        }

        public static IEnumerable<string> GetQueryValues(
            this IQueryCollection queryCollection,
            string key,
            char separator = ',')
        {
            return queryCollection.WhereHasKey(key)
                .SelectMany(v => v.Split(separator))
                .Distinct();
        }
    }
}
