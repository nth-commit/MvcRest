using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions
{
    public static class EnumerableExtensions
    {

        public static IEnumerable<TResult> Fork<TResult>(
            this IEnumerable<TResult> source,
            bool isMatch,
            Func<IEnumerable<TResult>, IEnumerable<TResult>> matchingPathFunc,
            Func<IEnumerable<TResult>, IEnumerable<TResult>> nonMatchingPathFunc)
        {
            return isMatch ? matchingPathFunc(source) : nonMatchingPathFunc(source);
        }

    }
}
