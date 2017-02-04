using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRest.Samples.EntityFrameworkCore.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        public double Salary { get; set; }

        [InverseProperty(nameof(EmployeeJob.Employee))]
        public virtual ICollection<EmployeeJob> Jobs { get; set; }
    }
}
