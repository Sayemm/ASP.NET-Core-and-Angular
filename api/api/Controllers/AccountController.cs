﻿using api.Data;
using api.DTOs;
using api.Entities;
using api.Interfaces;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public AccountController(DataContext context, ITokenService tokenService, IMapper mapper)
    {
      _context = context;
      _tokenService = tokenService;
      _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
      if (await UserExists(registerDto.UserName!)) return BadRequest("UserName is taken");

      var user = _mapper.Map<AppUser>(registerDto);

      using var hmac = new HMACSHA512();

      user.UserName = registerDto.UserName!.ToLower();
      user.PaswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password!));
      user.PasswordSalt = hmac.Key;

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return new UserDto
      {
        UserName = user.UserName,
        Token = _tokenService.CreateToken(user),
        KnownAs = user.KnownAs
      };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
      var user = await _context.Users
        .Include(p => p.Photos)
        .SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);

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
        Token = _tokenService.CreateToken(user),
        PhotoUrl = user.Photos!.FirstOrDefault(x => x.IsMain)?.Url,
        KnownAs = user.KnownAs
      };
    }

    private async Task<bool> UserExists(string userName)
    {
      return await _context.Users.AnyAsync(x => x.UserName == userName.ToLower());
    }
  }
}
