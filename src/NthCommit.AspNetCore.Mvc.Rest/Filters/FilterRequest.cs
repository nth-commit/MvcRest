using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Filters
{
    public class FilterRequest : Dictionary<string, string>
    {

        public FilterRequest()
        {
            
        }

        public bool TryGetIntValue(string fieldName, out int value)
        {
            value = 0;
            string valueStr;
            return TryGetValue(fieldName, out valueStr) && int.TryParse(valueStr, out value);
        }
    }
}
