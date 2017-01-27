using Microsoft.AspNetCore.Mvc;
using NthCommit.AspNetCore.Mvc.Rest.Sample.Controllers.Models;
using NthCommit.AspNetCore.Mvc.Rest.Sample.Services;
using NthCommit.AspNetCore.Mvc.Rest.Sample.Services.Models;
using NthCommit.AspNetCore.Mvc.Rest.Ordering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NthCommit.AspNetCore.Mvc.Rest.Sample.Controllers
{
    [Route("api/employees")]
    public class EmployeesApiController : RestApiController
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesApiController(
            IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeResult>), 200)]
        [Includable]
        [Pageable]
        [Orderable(typeof(Employee))]
        public IActionResult List()
        {
            var result = OrderRequest.Order(_employeeService.GetAllEmployees());
            return Ok(result, 50);
        }

        [HttpGet]
        [Route("{id}")]
        //[Filterable, Pageable, Sortable, Includable]
        [ProducesResponseType(typeof(EmployeeResult), 200)]
        [Includable]
        public IActionResult Get(int id)
        {
            return Ok(_employeeService.GetEmployee(id));
        }
    }
}
