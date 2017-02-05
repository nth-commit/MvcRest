using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueryableStrings.EntityFrameworkCore.Models;
using NthCommit.AspNetCore.Mvc.QueryableStrings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NthCommit.AspNetCore.Mvc.QueryableStrings.Filtering;

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
        [Pageable(DefaultPageSize = 3, MaximumPageSize = 10), Orderable, Selectable, Filterable]
        public async Task<IActionResult> List()
        {
            return Ok(await _dbContext.Employees
                .Query(Query)
                .ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [Selectable]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _dbContext.Employees
                .Where(e => e.Id == id)
                .Query(Query)
                .FirstOrDefaultAsync());
        }

        [HttpGet]
        [Route("paged")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Pageable(DefaultPageSize = 3, MaximumPageSize = 10)]
        public async Task<IActionResult> List_Paged()
        {
            return Ok(await _dbContext.Employees
                .Page(PageQuery)
                .ToListAsync());
        }

        [HttpGet]
        [Route("ordered")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Orderable]
        public async Task<IActionResult> List_Ordered()
        {
            return Ok(await _dbContext.Employees
                .Order(OrderQuery)
                .ToListAsync());
        }

        [HttpGet]
        [Route("selected")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Selectable]
        public async Task<IActionResult> List_Selected()
        {
            return Ok(await _dbContext.Employees
                .Select(SelectQuery)
                .ToListAsync());
        }

        [HttpGet]
        [Route("filtered")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Filterable]
        public async Task<IActionResult> List_Filtered()
        {
            return Ok(await _dbContext.Employees
                .Filter(FilterQuery)
                .ToListAsync());
        }

        [HttpGet]
        [Route("selected/{id}")]
        [ProducesResponseType(typeof(Employee), 200)]
        [Selectable]
        public async Task<IActionResult> Get_Selected(int id)
        {
            return Ok(await _dbContext.Employees
                .Where(e => e.Id == id)
                .Select(SelectQuery)
                .FirstOrDefaultAsync());
        }
    }
}
