namespace MyWebAPI.Models;

public interface IamUser
{
  int Id { get; }

  string Email { get; }

  string? PasswordHash { get; set; }

  string Role { get; set; }
  
  // In a real app, refresh tokens should live in a secure store/cache instead of the DB.
  string? RefreshToken { get; set; }
}
  
