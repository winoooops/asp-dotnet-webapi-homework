using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using MyWebAPI.Configs;
using MyWebAPI.Models;
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
  
  
  public string GenerateAccessToken(IamUser principal)
  {
    var claims = new List<Claim>
    {
      new Claim(JwtRegisteredClaimNames.Sub,  principal.Id.ToString()),
      new Claim(ClaimTypes.Role, principal.Role)
    };

    if (!string.IsNullOrWhiteSpace(principal.Email))
    {
      claims.Add(new Claim(JwtRegisteredClaimNames.Email, principal.Email));
    }

    // making some changes to class demo
    var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: _settings.Issuer,
      claims: claims,
      signingCredentials:credentials,
      audience: _settings.Audience,
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
