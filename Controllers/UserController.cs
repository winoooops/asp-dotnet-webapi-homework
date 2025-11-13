using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;
using MyWebAPI.Services.User;

namespace MyWebAPI.Controllers;

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
  public ActionResult<CommonResult<List<UserModel>>> GetUsers()
  {
    var users = _userService.GetAll();
    return CommonResult<List<UserModel>>.Success(users);
  }

  [HttpPost]
  [Route("")]
  public ActionResult<CommonResult<UserModel>> CreateUser([FromQuery] string email, [FromQuery] string username)
  {
    var user = _userService.Create(email, username);
    return CommonResult<UserModel>.Success(user);
  }

  [HttpGet]
  [Route("{userId}")]
  public ActionResult<CommonResult<UserModel>> GetUser(string userId)
  {
    try
    {
      if (!_userService.TryGetUserById(userId, out var user) || user == null)
      {
         throw new Exception("User not found"); 
      }
      return CommonResult<UserModel>.Success(user);
    }
    catch (Exception e)
    {
      return CommonResult<UserModel>.Fail(e.Message);
    }
  }

  [HttpPut]
  [Route("{userId}")]
  public ActionResult<CommonResult<UserModel>> UpdateUser(string userId, [FromQuery] string email,
    [FromQuery] string username)
  {
    try
    {
      if(!_userService.TryGetUserById(userId, out var user) ||  user == null)
      {
        throw new Exception("User not found");
      }
      var result = user.UpdateUserInfo(email, username);
      return CommonResult<UserModel>.Success(result);
    }
    catch (Exception e)
    {
      return CommonResult<UserModel>.Fail(e.Message);
    }
  }
}