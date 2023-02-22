using api.Data;
using api.DTOs;
using api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace api.Controllers
{
  public class AccountController : BaseApiController
  {
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
      _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.UserName!)) return BadRequest("UserName is Taken");

      using var hmac = new HMACSHA512();
      var user = new AppUser
      {
        UserName = registerDto.UserName!.ToLower(),
        PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!)),
        PasswordSalt = hmac.Key
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return user;
    }

    private async Task<bool> UserExists(string userName)
    {
      return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
    }
  }
}
