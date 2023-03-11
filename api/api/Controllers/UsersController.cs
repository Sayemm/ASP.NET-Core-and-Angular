using api.Data;
using api.Entities;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
  [Authorize]
  public class UsersController : BaseApiController
  {
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers() =>
      Ok(await _userRepository.GetUserAsync());

    //[HttpGet("{id}")]
    //public async Task<ActionResult<AppUser>> GetUser(int id) =>
    //  await _userRepository.GetUserByIdAsync(id);

    [HttpGet("{username}")]
    public async Task<ActionResult<AppUser>> GetUser(string username) =>
      await _userRepository.GetUserByUsernameAsync(username);
  }
}
