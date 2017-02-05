using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Ordering
{
    public class OrderQuery
    {
        public IEnumerable<OrderQueryDescriptor> Descriptors { get; private set; }

        public OrderQuery(List<OrderQueryDescriptor> descriptors)
        {
            Descriptors = descriptors;
        }
    }
}
