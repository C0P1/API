using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entitites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context) : BaseApiController
{
    [HttpPost("register")]   
    public async Task<ActionResult<AppUser>> RegisterAsync (RegisterRequest request)
    {
        if(await UserExistsAsync(request.Username))
        {
            return BadRequest("Username already in use");
        } 

        using var hmac = new HMACSHA512();

        var user= new AppUser{
            UserName = request.Username,
            PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt= hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return user;
    }

    [HttpPost("login")]
    public async  Task<ActionResult<AppUser>> Login(LoginRequest request)
    {}

    private async Task<bool> UserExistsAsync(string username)
    {
        return await context.Users.AnyAsync(
            user => user.UserName.ToLower() == username.ToLower()
        );
    }
}