using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers;

[Route("api/user")]
[ApiController]
public class UserControllerEF : ControllerBase
{
  private readonly IUserServiceEF _service;
  
  public UserControllerEF(IUserServiceEF service)
    {
    _service = service;
    }

  [HttpGet]
  [Route("")]
  public async Task<ActionResult<CommonResult<IEnumerable<User>>>> GetUsers()
  {
    var users = await _service.GetAllAsync();
    return CommonResult<IEnumerable<User>>.Success(users);
  }
  
  [HttpPost]
  [Route("")]
  public async Task<ActionResult<CommonResult<User>>> CreateUser(
    [FromQuery] string email,
    [FromQuery] string username,
    [FromQuery] string address,
    [FromQuery] int gender,
    [FromQuery] int? teacherId)
  {
    var model = new User
    {
      Email = email,
      Username = username,
      Address = address,
      Gender = gender,
      TeacherId = teacherId
    };
    var user = await _service.CreateAsync(model);
    
    return CommonResult<User>.Success(user);
  }

  [HttpGet]
  [Route("{id}")]
  public async Task<ActionResult<CommonResult<User>>> GetUser(int id)
  {
    var user = await _service.GetByIdAsync(id);
    if (user == null)
    {
      return CommonResult<User>.Fail("User not found");
    }
    
    return CommonResult<User>.Success(user);
  }

  [HttpPut]
  [Route("{userId}")]
  public async Task<ActionResult<CommonResult<User>>> UpdateUser(int userId, [FromBody] User user)
  {
    user.Id = userId;
    var existingUser = await _service.UpdateAsync(user);
    if (existingUser == null)
    {
      return CommonResult<User>.Fail("Could not update user");
    }
    
    return CommonResult<User>.Success(user);
  }

  [HttpDelete]
  [Route("{userId}")]
  [Authorize(Roles = "Admin")]
  public async Task<ActionResult<CommonResult<bool>>> DeleteUser([FromRoute] string userId)
  {
    if (int.TryParse(userId, out var id))
    {
      var result = await _service.DeleteAsync(id);
      if (result)
      {
        return CommonResult<bool>.Success(result);
      }
    }
    
    
    return CommonResult<bool>.Fail("Could not delete user");
  }
}
