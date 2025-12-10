using MyWebAPI.Models;

namespace MyWebAPI.Services.Auth;

public interface IJwtTokenService
{
  public string GenerateAccessToken(IamUser principal);

  public string GenerateRefreshToken();
}
