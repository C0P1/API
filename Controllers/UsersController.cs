using API.Data;
using API.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController : ControllerBase
{
    private readonly DataContext _context;

    public UsersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers()
    {
        var users= _context.Users.ToList();
        
        return Ok(users);
    }   

    [HttpGet("{id}")] //api/v1/users/2
    public ActionResult<AppUser> GetUsersById(int id)
    {
        var user = _context.Users.Find(id);
        
        if (user == null) return NotFound();

        return user;
    }
}