using Microsoft.AspNetCore.Mvc;
using SqlServerApi.Data;
using SqlServerApi.Models;
using Microsoft.EntityFrameworkCore;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> Get()
    {
        return await _context.Users.ToListAsync();
    }
}
