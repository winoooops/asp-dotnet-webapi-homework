using MyWebAPI.Models;

namespace MyWebAPI.Services.User;

public class UserService : IUserService
{
  private readonly List<UserModel> _users = new();
  
  public UserModel Create(string email, string username)
  {
    var newUser = new UserModel(email, username);
    _users.Add(newUser);
    
    return newUser;
  }

  public List<UserModel> GetAll()
  {
    return _users;
  }

  public UserModel? GetUserById(string userId)
  {
    if (!TryGetUserById(userId, out var user) || user == null)
    {
      return null;
    }

    return user;
  }

  public bool TryGetUserById(string userId, out UserModel? user)
  {
    user = _users.FirstOrDefault(u => u.UserId == userId);
    return user != null;
  }


  public UserModel? UpdateUserInfo(string userId, string email, string username)
  {
    if (!TryGetUserById(userId, out var user) || user == null)
    {
      return null;
    }
    
    user.UpdateUserInfo(email, username);
    return user;
  }

  public bool DeleteUserById(string userId)
  {
    if (!TryGetUserById(userId, out var user) || user == null)
    {
      return false;
    }
    
    return _users.Remove(user);
  }
}