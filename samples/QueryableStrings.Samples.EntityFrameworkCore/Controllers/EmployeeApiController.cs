using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueryableStrings.EntityFrameworkCore.Models;
using NthCommit.AspNetCore.Mvc.QueryableStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryableStrings.EntityFrameworkCore.Controllers
{
    [Route("api/employees")]
    public class EmployeeApiController : QueryableController
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeApiController(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Pageable(DefaultPageSize = 3, MaximumPageSize = 10), Orderable, Selectable]
        public async Task<IActionResult> List()
        {
            return Ok(
                await _dbContext.Employees.Query(Query).ToListAsync(),
                await _dbContext.Employees.CountAsync());
        }

        [HttpGet]
        [Route("paged")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Pageable(DefaultPageSize = 3, MaximumPageSize = 10)]
        public async Task<IActionResult> List_Paged()
        {
            return Ok(
                await _dbContext.Employees.Page(PageQuery).ToListAsync(),
                await _dbContext.Employees.CountAsync());
        }

        [HttpGet]
        [Route("ordered")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Orderable]
        public async Task<IActionResult> List_Ordered()
        {
            return Ok(await _dbContext.Employees.Order(OrderQuery).ToListAsync());
        }

        [HttpGet]
        [Route("selectable")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Selectable]
        public async Task<IActionResult> List_Selectable()
        {
            return Ok(await _dbContext.Employees.Select(SelectQuery).ToListAsync());
        }
    }
}
