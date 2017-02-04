using Microsoft.EntityFrameworkCore;
using QueryableStrings.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryableStrings.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<EmployeeJob> EmployeeJobs { get; set; }

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
