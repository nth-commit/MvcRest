using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Selecting
{
    public class SelectQuery
    {
        public IEnumerable<string> PropertyNames { get; private set; }

        internal SelectQuery(IEnumerable<string> propertyNames)
        {
            PropertyNames = propertyNames.ToArray();
        }
    }
}
