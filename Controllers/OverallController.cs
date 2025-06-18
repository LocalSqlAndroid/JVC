using Microsoft.AspNetCore.Mvc;
using SqlServerApi.Data;
using Microsoft.EntityFrameworkCore;
namespace SqlServerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OverallController : Controller
    {
      

        private readonly AppDbContext _context;

        public OverallController(AppDbContext context)
        {
            _context = context;
        }

        // Get distinct customers for spinner
        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _context.DyOverall
                                .Select(x => x.customer)
                                .Distinct()
                                .OrderBy(x => x)
                                .ToListAsync();
            return Ok(customers);
        }

        // Get all records for selected customer
        [HttpGet("GetByCustomer")]
        public async Task<IActionResult> GetByCustomer(string customer)
        {
            var result = await _context.DyOverall
                                .Where(x => x.customer == customer)
                                .OrderBy(x => x.stage)
                                .ToListAsync();
            return Ok(result);
        }
    }
}
