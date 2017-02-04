using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Ordering
{
    public class OrderRequest
    {
        public IEnumerable<OrderDescriptor> OrderDescriptors { get; private set; }

        public OrderRequest(List<OrderDescriptor> orderDescriptors)
        {
            OrderDescriptors = orderDescriptors;
        }
    }
}
