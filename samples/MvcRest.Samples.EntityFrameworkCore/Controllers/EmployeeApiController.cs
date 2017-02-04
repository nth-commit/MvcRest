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
        [Route("paged")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        [Pageable(DefaultPageSize = 3, MaximumPageSize = 10)]
        public async Task<IActionResult> List_Paged()
        {
            return Ok(await _dbContext.Employees
                .Page(PageRequest)
                .ToListAsync());
        }

        [HttpGet]
        [Route("ordered")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        public async Task<IActionResult> List_Ordered()
        {
            return Ok(await _dbContext.Employees
                .Order(OrderRequest)
                .ToListAsync());
        }
    }
}
