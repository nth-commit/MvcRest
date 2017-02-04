using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcRest.Samples.EntityFrameworkCore;
using MvcRest.Samples.EntityFrameworkCore.Models;
using NthCommit.AspNetCore.Mvc.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcRest.Samples.EntityFrameworkCore.Controllers
{
    [Route("api/employees")]
    public class EmployeeApiController : RestApiController
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
        [Pageable(DefaultPageSize = 3, MaximumPageSize = 10), Orderable, Includable]
        public async Task<IActionResult> List()
        {
            return Ok(
                await _dbContext.Employees.Order(OrderRequest).Page(PageRequest).ToListAsync(),
                await _dbContext.Employees.CountAsync());
        }

        [HttpGet]
        [Route("paged")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Pageable(DefaultPageSize = 3, MaximumPageSize = 10)]
        public async Task<IActionResult> List_Paged()
        {
            return Ok(
                await _dbContext.Employees.Page(PageRequest).ToListAsync(),
                await _dbContext.Employees.CountAsync());
        }

        [HttpGet]
        [Route("ordered")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Orderable]
        public async Task<IActionResult> List_Ordered()
        {
            return Ok(await _dbContext.Employees.Order(OrderRequest).ToListAsync());
        }

        [HttpGet]
        [Route("includable")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Includable]
        public async Task<IActionResult> List_Includable()
        {
            return Ok(await _dbContext.Employees.ToListAsync());
        }
    }
}
