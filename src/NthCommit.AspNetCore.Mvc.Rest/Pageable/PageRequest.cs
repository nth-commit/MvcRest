using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Pageable
{
    public class PageRequest
    {
        public int Number { get; private set; }

        public int Size { get; private set; }

        public PageRequest(int number, int size)
        {
            Number = number;
            Size = size;
        }
    }
}
