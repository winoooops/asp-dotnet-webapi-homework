namespace MyWebAPI.Services.Auth;

public interface IJwtTokenService
{
  public string GenerateAccessToken(Models.User user);

  public string GenerateRefreshToken();
}