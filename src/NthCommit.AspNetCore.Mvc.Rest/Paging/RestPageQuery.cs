using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Paging
{
    public class RestPageQuery
    {
        public int Number { get; private set; }

        public int Size { get; private set; }

        public RestPageQuery(int number, int size)
        {
            Number = number;
            Size = size;
        }
    }
}
