﻿using api.Data;
using api.DTOs;
using api.Entities;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
      var users = await _userRepository.GetUserAsync();
      var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
      return Ok(usersToReturn);
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<AppUser>> GetUser(int id) =>
    //  await _userRepository.GetUserByIdAsync(id);

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
      var user = await _userRepository.GetUserByUsernameAsync(username);
      return _mapper.Map<MemberDto>(user);
    }
  }
}
