using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering
{
    public class OrderDescriptor
    {
        public string PropertyName { get; private set; }

        public bool IsAscending { get; private set; }

        public OrderDescriptor(string propertyName, bool isAscending)
        {
            PropertyName = propertyName;
            IsAscending = isAscending;
        }
    }
}
