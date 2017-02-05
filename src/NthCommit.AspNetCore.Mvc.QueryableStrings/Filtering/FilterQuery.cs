using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering
{
    public class FilterQuery
    {
        public IEnumerable<FilterQueryDescriptor> Descriptors { get; private set; }

        public FilterQuery(IEnumerable<FilterQueryDescriptor> descriptors)
        {
            Descriptors = descriptors.ToList();
        }
    }
}
