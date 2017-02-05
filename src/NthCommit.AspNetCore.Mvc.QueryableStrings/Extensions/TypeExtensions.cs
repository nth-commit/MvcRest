using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions
{
    public static class TypeExtensions
    {
        public static Type GetGenericIEnumerableType(this Type type)
        {
            return IsGenericIEnumerable(type) ?
                type :
                type.GetInterfaces()
                    .Where(IsGenericIEnumerable)
                    .FirstOrDefault();
        }

        private static bool IsGenericIEnumerable(Type type)
        {
            return type.IsGenericType == true && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
    }
}
