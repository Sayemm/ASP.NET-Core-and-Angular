﻿using api.Data;
using api.DTOs;
using api.Entities;
using api.Interfaces;
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
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService)
    {
      _context = context;
      _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.UserName!)) return BadRequest("UserName is taken");

      using var hmac = new HMACSHA512();
      var user = new AppUser
      {
        UserName = registerDto.UserName!.ToLower(),
        PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!)),
        PasswordSalt = hmac.Key
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return new UserDto
      {
        UserName = user.UserName,
        Token = _tokenService.CreateToken(user)
      };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

      if (user == null) return Unauthorized("Invalid Username");

      using var hmac = new HMACSHA512(user.PasswordSalt!);
      var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password!));

      for(int i = 0; i < computedHash.Length; i++)
      {
        if (computedHash[i] != user.PaswordHash![i]) return Unauthorized("Invalid Password");
      }

      return new UserDto
      {
        UserName = user.UserName,
        Token = _tokenService.CreateToken(user)
      };
    }

    private async Task<bool> UserExists(string userName)
    {
      return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
    }
  }
}
