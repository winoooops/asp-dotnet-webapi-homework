namespace MyWebAPI.DTOs;

public class UserDTO
{
  public required string Email { get; set; }
    
  public required string Username { get; set; }
    
  public string? Address { get; set; }
    
  public int? Gender { get; set; }
}