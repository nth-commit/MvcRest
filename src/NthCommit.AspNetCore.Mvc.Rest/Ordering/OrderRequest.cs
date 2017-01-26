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

        public OrderRequest(Type type, IEnumerable<OrderDescriptor> orderDescriptors)
        {
            OrderDescriptors = orderDescriptors;
        }

        public IEnumerable<T> Order<T>(IEnumerable<T> source)
        {
            return Order(source.AsQueryable());
        }

        public IQueryable<T> Order<T>(IQueryable<T> source)
        {
            var result = source;
            var firstOrderDescriptor = OrderDescriptors.FirstOrDefault();
            if (firstOrderDescriptor != null)
            {
                var orderedSource = source.OrderBy(firstOrderDescriptor);
                foreach (var orderDescriptor in OrderDescriptors.Skip(1))
                {
                    orderedSource = orderedSource.ThenBy(orderDescriptor);
                }
                result = orderedSource;
            }
            return result;
        }
    }
}
