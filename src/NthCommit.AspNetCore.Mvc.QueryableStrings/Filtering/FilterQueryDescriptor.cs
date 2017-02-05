using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering
{
    public class FilterQueryDescriptor
    {
        public string PropertyName { get; private set; }

        public string Value { get; set; }

        public FilterQueryDescriptorType Type { get; private set; }

        public FilterQueryDescriptor(string propertyName, string value, FilterQueryDescriptorType type)
        {
            PropertyName = propertyName;
            Value = value;
            Type = type;
        }
    }
}
