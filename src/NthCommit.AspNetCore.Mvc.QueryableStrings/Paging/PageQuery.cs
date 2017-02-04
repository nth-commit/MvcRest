using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.QueryableStrings.Paging
{
    public class PageQuery
    {
        public int Number { get; private set; }

        public int Size { get; private set; }

        public PageQuery(int number, int size)
        {
            Number = number;
            Size = size;
        }
    }
}
