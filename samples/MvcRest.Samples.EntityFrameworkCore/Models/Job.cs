using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRest.Samples.EntityFrameworkCore.Models
{
    public class Job
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public double MinSalary { get; set; }

        public double MaxSalary { get; set; }
    }
}
