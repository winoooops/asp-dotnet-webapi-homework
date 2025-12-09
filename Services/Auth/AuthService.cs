using MyWebAPI.DTOs;

namespace MyWebAPI.Services.Auth;

public class AuthService : IAuthService
{
  private readonly IJwtTokenService _jwtTokenService;
  
  private readonly IUserServiceEF _userService;

  public AuthService(IJwtTokenService jwtTokenService, IUserServiceEF userService)
  {
    _jwtTokenService = jwtTokenService;
    _userService = userService;
  }
  
  public async Task<AuthResponse> SignUpAsync(SignUpRequest req)
  {
    var existingUser = await _userService.GetByEmailAsync(req.Email);
    if (existingUser != null)
    {
      throw new InvalidOperationException("Email already exists, plz login instead.");
    }
    
    var model = new Models.User
    {
      Email = req.Email,
      Username = req.Username,
      Address = req.Address,
      Gender = req.Gender,
      PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
    };

    var user = await _userService.CreateAsync(model);

    return new AuthResponse
    {
      AccessToken = _jwtTokenService.GenerateAccessToken(user),
      RefreshToken = _jwtTokenService.GenerateRefreshToken(),
    };
  }

  public async Task<AuthResponse> SignInAsync(SignInRequest req)
  {
    var existingUser = await _userService.GetByEmailAsync(req.Email);
    if (existingUser == null)
    {
      throw new UnauthorizedAccessException("User not found, plz signup instead.");
    }
    
    var isValid = BCrypt.Net.BCrypt.Verify(req.Password, existingUser.PasswordHash);
    if (!isValid)
      throw new UnauthorizedAccessException("Invalid credentials.");

    return new AuthResponse
    {
      AccessToken = _jwtTokenService.GenerateAccessToken(existingUser),
      RefreshToken = _jwtTokenService.GenerateRefreshToken(),
    };
  }

  public async Task<AuthResponse> RefreshAccessTokenAsync(RefreshRequest req)
  {
    var existingUser = await _userService.GetByRefreshTokenAsync(req.RefreshToken);
    if (existingUser == null)
    {
      throw new UnauthorizedAccessException("Not a valid refresh token.");
    }

    return new AuthResponse
    {
      AccessToken = _jwtTokenService.GenerateAccessToken(existingUser),
      RefreshToken = _jwtTokenService.GenerateRefreshToken(),
    };
  }
}