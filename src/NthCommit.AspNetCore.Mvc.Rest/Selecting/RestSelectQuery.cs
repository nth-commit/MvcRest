using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Selecting
{
    public class RestSelectQuery
    {
        public IEnumerable<string> Properties { get; private set; }

        internal RestSelectQuery(IEnumerable<string> properties)
        {
            Properties = properties.ToArray();
        }
    }
}
