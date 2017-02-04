using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRest.Samples.EntityFrameworkCore.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ManagerId { get; set; }

        public virtual Employee Manager { get; set; }
    }
}
