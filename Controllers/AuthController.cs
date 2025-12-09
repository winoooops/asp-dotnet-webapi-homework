using Microsoft.AspNetCore.Mvc;
using MyWebAPI.DTOs;
using MyWebAPI.Models;
using MyWebAPI.Services.Auth;

namespace MyWebAPI.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }
  
  
  [HttpPost]
  [Route("/signup")]
  public async Task<ActionResult<CommonResult<AuthResponse>>> SignUp(SignUpRequest request)
  {
    try
    {
      var response = await _authService.SignUpAsync(request);
      return CommonResult<AuthResponse>.Success(response);
    }
    catch (InvalidOperationException e)
    {
      return CommonResult<AuthResponse>.Fail(e.Message);
    }
  }

  [HttpPost]
  [Route("/signin")]
  public async Task<ActionResult<CommonResult<AuthResponse>>> SignIn(SignInRequest request)
  {
    try
    {
      var response = await _authService.SignInAsync(request);
      return CommonResult<AuthResponse>.Success(response);
    }
    catch (UnauthorizedAccessException e)
    {
      return CommonResult<AuthResponse>.Fail(e.Message);
    }  
  }

  [HttpPost]
  [Route("/refresh")]
  public async Task<ActionResult<CommonResult<AuthResponse>>> Refresh(RefreshRequest request)
  {
    try
    {
      var response = await _authService.RefreshAccessTokenAsync(request);
      return CommonResult<AuthResponse>.Success(response);
    }
    catch (UnauthorizedAccessException e)
    {
      return CommonResult<AuthResponse>.Fail(e.Message);
    }
  } 
}