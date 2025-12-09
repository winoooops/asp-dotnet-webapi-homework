using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DTOs;
using MyWebAPI.Models;
using MyWebAPI.Services.User;

namespace MyWebAPI.Controllers;

[NonController]
[Route("api/user")]
[ApiController]
public class UserController: ControllerBase
{
  private readonly IUserService _userService;

  public UserController(IUserService userService)
  {
    _userService = userService;
  }
  
  [HttpGet]
  [Route("")]
  public async Task<ActionResult<CommonResult<List<User>>>> GetUsers()
  {
    var users = await _userService.GetAll();
    return CommonResult<List<User>>.Success(users);
  }

  [HttpPost]
  [Route("")]
  public async Task<ActionResult<CommonResult<User>>> CreateUser([FromQuery] string email, [FromQuery] string username, [FromQuery] string? address, [FromQuery] int? gender)
  {
    var userDto = new UserDTO
    {
      Email = email,
      Username = username,
      Address = address,
      Gender = gender
    };
    var user = await _userService.Create(userDto);

    if (user == null)
    {
      return CommonResult<User>.Fail("Could not create user");
    }
    
    return CommonResult<User>.Success(user);
  }

  [HttpGet]
  [Route("{userId}")]
  public async Task<ActionResult<CommonResult<User>>> GetUser(int userId)
  {
    var user = await _userService.GetUserById(userId);
    if (user == null)
    {
      return CommonResult<User>.Fail("Could not get user");
    }
    return CommonResult<User>.Success(user); 
  }

  [HttpPut]
  [Route("{userId}")]
  public async Task<ActionResult<CommonResult<User>>> UpdateUser(int userId, [FromQuery] string email,
    [FromQuery] string username)
  {
      var user = await _userService.UpdateUserInfo(userId, email, username);
      if (user == null)
      {
        return CommonResult<User>.Fail("Could not update user");
      }
      
      return CommonResult<User>.Success(user);
  }

  [HttpDelete]
  [Route("{userId}")]
  public async Task<ActionResult<CommonResult<bool>>> DeleteUser(int userId)
  {
    var result = await _userService.DeleteUserById(userId);
    if (result == false)
    {
      return CommonResult<bool>.Fail("Could not delete user");
    }
    
    return CommonResult<bool>.Success(result);
  }
}
