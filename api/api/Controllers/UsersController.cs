using api.Data;
using api.DTOs;
using api.Entities;
using api.Extensions;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace api.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
      _userRepository = userRepository;
      _mapper = mapper;
      _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
      return Ok(await _userRepository.GetMembersAsync());
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<AppUser>> GetUser(int id) =>
    //  await _userRepository.GetUserByIdAsync(id);

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
      // give users username from the token that api uses to authenticate user
      var username = User.GetUserName();
      var user = await _userRepository.GetUserByUsernameAsync(username!);

      _mapper.Map(memberUpdateDto, user);
      _userRepository.Update(user);

      if (await _userRepository.SaveAllAsync()) return NoContent();
      return BadRequest("Failed to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
      var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
      var result = await _photoService.AddPhotoAsync(file);

      if (result.Error != null) return BadRequest(result.Error.Message);

      var photo = new Photo
      {
        Url = result.SecureUrl.AbsoluteUri,
        PublicId = result.PublicId
      };

      if (user.Photos!.Count == 0)
      {
        photo.IsMain = true;
      }

      user.Photos.Add(photo);

      if(await _userRepository.SaveAllAsync())
      {
        return _mapper.Map<Photo, PhotoDto>(photo);
      }
      return BadRequest("Problem when adding a photo");
    }
  }
}
