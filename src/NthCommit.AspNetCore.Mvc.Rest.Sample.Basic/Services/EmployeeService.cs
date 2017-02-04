using NthCommit.AspNetCore.Mvc.Rest.Sample.Basic.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Sample.Basic.Services
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllEmployees();

        Employee GetEmployee(int id);
    }

    public class EmployeeService : IEmployeeService
    {
        public IEnumerable<Employee> GetAllEmployees()
        {
            var result = EmployeeData.AsQueryable();



            return EmployeeData;
        }

        public Employee GetEmployee(int id)
        {
            return EmployeeData.FirstOrDefault(e => e.Id == id);
        }


        #region Data

        private static readonly IEnumerable<Employee> EmployeeData = new List<Employee>()
        {
            new Employee()
            {
                Id = 1,
                FirstName = "Alice",
                LastName = "Z",
                Age = 25
            },
            new Employee()
            {
                Id = 2,
                FirstName = "Bob",
                LastName = "Y",
                Age = 32
            },
            new Employee()
            {
                Id = 3,
                FirstName = "Charlotte",
                LastName = "X",
                Age = 21
            },
            new Employee()
            {
                Id = 4,
                FirstName = "David",
                LastName = "W",
                Age = 40
            },
            new Employee()
            {
                Id = 5,
                FirstName = "Edward",
                LastName = "V",
                Age = 28
            }
        };

        #endregion
    }
}
