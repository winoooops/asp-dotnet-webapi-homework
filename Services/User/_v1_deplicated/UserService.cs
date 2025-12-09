using Microsoft.Extensions.Options;
using MyWebAPI.Configs;
using MyWebAPI.DTOs;
using MyWebAPI.Repositories;

namespace MyWebAPI.Services.User;

public class UserService : IUserService
{
  private readonly List<Models.User> _users = new();

  private UserRepository _repository;

  public UserService(IOptions<DBConfig> options)
  {
    _repository = new UserRepository(options);
  }
  
  public async Task<Models.User?> Create(UserDTO userDto)
  {
    
    var user = await _repository.CreateUser(userDto);
    return user;
  }

  public async Task<List<Models.User>> GetAll()
  {
    var users = await _repository.GetAll() ;

    return users;
  }

  public async Task<Models.User?> GetUserById(int userId)
  {
    var user = await _repository.GetUserById(userId);

    return user;
  }

  public async Task<Models.User?> UpdateUserInfo(int userId, string email, string username)
  {
    var user = await _repository.UpdateUser(userId, email, username);
    return user;
  }

  public async Task<bool> DeleteUserById(int userId)
  {
    var result = await _repository.DeleteUser(userId);
    return result;
  }
}