using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRest.Samples.EntityFrameworkCore.Models
{
    public class EmployeeJob
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public int? JobId { get; set; }

        public virtual Job Job { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}
