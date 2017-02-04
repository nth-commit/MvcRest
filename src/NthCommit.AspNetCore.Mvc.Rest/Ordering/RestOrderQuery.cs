using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Ordering
{
    public class RestOrderQuery
    {
        public IEnumerable<OrderDescriptor> OrderDescriptors { get; private set; }

        public RestOrderQuery(List<OrderDescriptor> orderDescriptors)
        {
            OrderDescriptors = orderDescriptors;
        }
    }
}
