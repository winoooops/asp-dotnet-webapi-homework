using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyWebAPI.Configs;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace MyWebAPI.Services.Auth;

public class JwtTokenService : IJwtTokenService
{
  private readonly JwtSettings _settings;
  private readonly byte[] _key;

  public JwtTokenService(IOptions<JwtSettings> options)
  {
    _settings = options.Value;
    
    _key = Encoding.UTF8.GetBytes(_settings.Key);
  }
  
  
  public string GenerateAccessToken(Models.User user)
  {
    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Name, user.Username),
      new Claim(JwtRegisteredClaimNames.Email, user.Email),
      new Claim(JwtRegisteredClaimNames.Gender, user.Gender.ToString()),
      new Claim(JwtRegisteredClaimNames.Sub,  user.Id.ToString()),
    };
   
    // making some changes to class demo
    var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: _settings.Issuer,
      claims: claims,
      signingCredentials:credentials,
      expires: DateTimeOffset.Now.LocalDateTime.AddMinutes(_settings.AccessTokenMinutes) 
    );
    
    return new JwtSecurityTokenHandler().WriteToken(token);
  }

  public string GenerateRefreshToken()
  {
    var randomBytes = RandomNumberGenerator.GetBytes(64);
    return Convert.ToBase64String(randomBytes); 
  }
}