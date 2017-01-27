using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Includes
{
    public class IncludeRequest
    {
        public IEnumerable<string> Properties { get; private set; }

        internal IncludeRequest(IEnumerable<string> properties)
        {
            Properties = properties.ToArray();
        }
    }
}
