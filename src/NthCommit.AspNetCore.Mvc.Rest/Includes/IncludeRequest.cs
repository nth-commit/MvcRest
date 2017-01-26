using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Includes
{
    public class IncludeRequest : IEnumerable<string>
    {
        private readonly IEnumerable<string> _fields;

        internal IncludeRequest(IEnumerable<string> fields)
        {
            _fields = fields.ToArray();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _fields.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
