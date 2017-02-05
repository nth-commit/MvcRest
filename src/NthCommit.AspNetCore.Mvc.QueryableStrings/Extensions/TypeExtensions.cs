using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Extensions
{
    public static class TypeExtensions
    {
        private static ConcurrentDictionary<Type, PropertyInfo[]> _propertyInfoByType = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static Type GetGenericIEnumerableType(this Type type)
        {
            return IsGenericIEnumerable(type) ?
                type :
                type.GetInterfaces()
                    .Where(IsGenericIEnumerable)
                    .FirstOrDefault();
        }
        
        public static PropertyInfo[] GetCachedProperties(this Type type)
        {
            return _propertyInfoByType.GetOrAdd(type, t => t.GetProperties());
        }

        private static bool IsGenericIEnumerable(Type type)
        {
            return type.IsGenericType == true && type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }
    }
}
