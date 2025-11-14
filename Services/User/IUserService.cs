using MyWebAPI.DTOs;
using MyWebAPI.Models;

namespace MyWebAPI.Services.User;

public interface IUserService
{
  public Task<Models.User?> Create(UserDTO userDto);
  
  public Task<List<Models.User>> GetAll();

  public Task<Models.User?> GetUserById(int userId);
  
  public Task<Models.User?> UpdateUserInfo(int userId, string email, string username);

  public Task<bool> DeleteUserById(int userId);
}