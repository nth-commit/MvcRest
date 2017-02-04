using Microsoft.Extensions.DependencyInjection;
using MvcRest.Samples.EntityFrameworkCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRest.Samples.EntityFrameworkCore.Data
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            new Startup().ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            AddEmployees(dbContext);
        }

        private static void AddEmployees(ApplicationDbContext dbContext)
        {
            dbContext.Employees.AddRange(
                new Employee()
                {
                    FirstName = "Marla",
                    LastName = "Lana",
                    Email = "marla.lana@mvcrest.com",
                    Salary = 50000,
                    HireDate = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567"
                },
                new Employee()
                {
                    FirstName = "Crispian",
                    LastName = "Norwood",
                    Email = "crispian.norwood@mvcrest.com",
                    Salary = 50000,
                    HireDate = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567"
                },
                new Employee()
                {
                    FirstName = "Shavonne",
                    LastName = "Maudie",
                    Email = "shavonne.maudie@mvcrest.com",
                    Salary = 50000,
                    HireDate = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567"
                },
                new Employee()
                {
                    FirstName = "Birtha",
                    LastName = "Briggs",
                    Email = "birta.briggs@mvcrest.com",
                    Salary = 50000,
                    HireDate = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567"
                },
                new Employee()
                {
                    FirstName = "Cosmo",
                    LastName = "Korey",
                    Email = "cosmo.corey@mvcrest.com",
                    Salary = 50000,
                    HireDate = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567"
                },
                new Employee()
                {
                    FirstName = "Maitland",
                    LastName = "Denholm",
                    Email = "maitland.denholm@mvcrest.com",
                    Salary = 50000,
                    HireDate = new DateTime(2000, 1, 1),
                    PhoneNumber = "1234567"
                });
            dbContext.SaveChanges();
        }
    }
}
