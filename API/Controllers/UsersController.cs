namespace API.Controllers;

using System.Security.Claims;
using API.Data;
using API.DTOs;
<<<<<<< HEAD
=======
using API.Entities;
using API.Extensions;
using API.Services;
>>>>>>> Parcial04
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _repository;
<<<<<<< HEAD
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
=======
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IPhotoService photoService, IMapper mapper)
    {
        _repository = repository;
        _photoService = photoService;
>>>>>>> Parcial04
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllAsync()
    {
        var response = await _repository.GetMembersAsync();
        return Ok(response);
    }

<<<<<<< HEAD
    [HttpGet("{username}")] //api/v1/users/Olaf
=======
    [HttpGet("{username}", Name = "GetByUsername")] //api/v1/users/Olaf
>>>>>>> Parcial04
    public async Task<ActionResult<MemberResponse>> GetByUsernameAsync(string username)
    {
        var member = await _repository.GetMemberAsync(username);
        if(member == null)
        {
            return NotFound();
        }

        return member;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateRequest request){
<<<<<<< HEAD
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null)
        {
            return BadRequest("No username found in token");
        }

        var user = await _repository.GetByUsernameAsync(username);
=======
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
>>>>>>> Parcial04
        if (user == null)
        {
            return BadRequest("Could not find user");
        }

        _mapper.Map(request, user);
        _repository.Update(user);
        if (await _repository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Update user failed!");
    }
<<<<<<< HEAD
=======

    [HttpPost("photo")]
    public async Task<ActionResult<PhotoResponse>> AddPhoto(IFormFile file)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
        if (user == null)
        {
            return BadRequest("Cannot update user");
        }

        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null)
        {
            return BadRequest(result.Error.Message);
        }

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        user.Photos.Add(photo);

        if (await _repository.SaveAllAsync())
        {
            return CreatedAtAction("GetByUsername",
                new { username = user.UserName }, _mapper.Map<PhotoResponse>(photo));
        }

        return BadRequest("Problems adding the photo");
    }

    [HttpPut("photo/{photoId:int}")]
    public async Task<ActionResult> SetPhotoAsMain(int photoId)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());
        if (user == null) return BadRequest("User not found");
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("Can't set this photo as the main one!");
        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        if (await _repository.SaveAllAsync()) return NoContent();
        return BadRequest("There was a problem.");
    }

    [HttpDelete("photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUserName());

        if (user == null) return BadRequest("User not found");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("This photo can´t be deleted");

        if(photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);
        
        if (await _repository.SaveAllAsync()) return Ok();

        return BadRequest("There was a problem when deleting the photo");
    }
>>>>>>> Parcial04
}