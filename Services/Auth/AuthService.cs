using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.Services.Auth;

public class AuthService : IAuthService
{
  private readonly IJwtTokenService _jwtTokenService;
  
  private readonly IIamUserService _iamUserService;

  public AuthService(IJwtTokenService jwtTokenService, IIamUserService iamUserService)
  {
    _jwtTokenService = jwtTokenService;
    _iamUserService = iamUserService;
  }
  
  public async Task<AuthResponse> SignUpAsync(SignUpRequest req)
  {
    var existing = await _iamUserService.FindByEmailAsync(req.Email);
    if (existing != null)
    {
      throw new InvalidOperationException("Email already exists, plz login instead.");
    }
    
    var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);
    var refreshToken = _jwtTokenService.GenerateRefreshToken();

    var principal = await _iamUserService.CreateAsync(req, hashedPassword, refreshToken);

    return new AuthResponse
    {
      AccessToken = _jwtTokenService.GenerateAccessToken(principal),
      RefreshToken = refreshToken,
    };
  }

  public async Task<AuthResponse> SignInAsync(SignInRequest req)
  {
    var principal = await _iamUserService.FindByEmailAsync(req.Email);
    if (principal == null || string.IsNullOrEmpty(principal.PasswordHash))
    {
      throw new UnauthorizedAccessException("User not found, plz signup instead.");
    }
    
    var isValid = BCrypt.Net.BCrypt.Verify(req.Password, principal.PasswordHash);
    if (!isValid)
      throw new UnauthorizedAccessException("Invalid credentials.");

    var newRefresh = _jwtTokenService.GenerateRefreshToken();
    await _iamUserService.UpdateRefreshTokenAsync(principal, newRefresh);

    return new AuthResponse
    {
      AccessToken = _jwtTokenService.GenerateAccessToken(principal),
      RefreshToken = newRefresh,
    };
  }

  public async Task<AuthResponse> RefreshAccessTokenAsync(RefreshRequest req)
  {
    var principal = await _iamUserService.FindByRefreshTokenAsync(req.RefreshToken);
    
    if (principal == null)
    {
      throw new UnauthorizedAccessException("Not a valid refresh token.");
    }

    var newRefresh = _jwtTokenService.GenerateRefreshToken();
    await _iamUserService.UpdateRefreshTokenAsync(principal, newRefresh);

    return new AuthResponse
    {
      AccessToken = _jwtTokenService.GenerateAccessToken(principal),
      RefreshToken = newRefresh,
    };
  }
}
