using api.Data;
using api.DTOs;
using api.Entities;
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

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
      _userRepository = userRepository;
      _mapper = mapper;
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
      var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var user = await _userRepository.GetUserByUsernameAsync(username!);

      _mapper.Map(memberUpdateDto, user);
      _userRepository.Update(user);

      if (await _userRepository.SaveAllAsync()) return NoContent();
      return BadRequest("Failed to update user");
    }
  }
}
