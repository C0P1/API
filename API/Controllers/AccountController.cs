namespace API.Controllers;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.DataEntities;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD

public class AccountController(
    DataContext context,
    ITokenServices tokenServices) : BaseApiController
=======
using AutoMapper;

public class AccountController(
    DataContext context,
    ITokenServices tokenService,
    IMapper mapper) : BaseApiController
>>>>>>> Parcial04
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterAsync(RegisterRequest request)
    {
        if(await UserExistsAsync(request.Username))
        {
            return BadRequest("Username already in use");
        }
<<<<<<< HEAD
        return Ok();
        // using var hmac = new HMACSHA512();

        // var user= new AppUser{
        //     UserName = request.Username,
        //     PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
        //     PasswordSalt= hmac.Key
        // };

        // context.Users.Add(user);
        // await context.SaveChangesAsync();

        // return new UserResponse{
        //     Username = user.UserName,
        //     Token = tokenServices.CreateToken(user)
        // };
=======
        using var hmac = new HMACSHA512();
        var user = mapper.Map<AppUser>(request);
        user.UserName = request.Username;
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));
        user.PasswordSalt = hmac.Key;

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs
        };
>>>>>>> Parcial04
    }// RegisterAsync

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> LoginAsync(LoginRequest request)
    {
<<<<<<< HEAD
        var user = await context.Users.FirstOrDefaultAsync(x =>
        x.UserName.ToLower()== request.Username.ToLower());
=======
        var user = await context.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.UserName.ToLower() == request.Username.ToLower());
>>>>>>> Parcial04

        if(user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        for (var i=0; i<computeHash.Length; i++)
        {
            if(computeHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid username or password");
            }
        }

<<<<<<< HEAD
        return new UserResponse{
            Username = user.UserName,
            Token = tokenServices.CreateToken(user)
        };
    }

    private async Task<bool> UserExistsAsync(string username)
    {
        return await context.Users.AnyAsync(
            user => user.UserName.ToLower() == username.ToLower()
        );
    }
=======
        return new UserResponse
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
        };
    }

    private async Task<bool> UserExistsAsync(string username)=>
        await context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower());
>>>>>>> Parcial04
}