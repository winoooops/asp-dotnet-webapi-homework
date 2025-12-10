namespace MyWebAPI.Configs;

public class JwtSettings
{
  public string Issuer { get; set; } 
  
  public string Key { get; set; }
  
  public string Audience { get; set; }
  
  public double  AccessTokenMinutes { get; set; }
  
  public int RefreshTokenDays { get; set; }
}