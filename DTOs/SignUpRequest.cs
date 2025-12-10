using MyWebAPI.Models;

namespace MyWebAPI.DTOs;

public class SignUpRequest
{
  public string Email { get; set; } 
  public string Username { get; set; }
  public string Password { get; set; }
  public int Gender { get; set; }
  public string Address { get; set; }
  
  public string Role { get; set; }
}
