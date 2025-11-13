using MyWebAPI.Models;

namespace MyWebAPI.Services.User;

public interface IUserService
{
  public UserModel Create(string email, string username);
  
  public List<UserModel> GetAll();

  public UserModel? GetUserById(string userId);
  
  public bool TryGetUserById(string userId, out UserModel? user); 
  
  public UserModel? UpdateUserInfo(string userId, string email, string username);

  public bool DeleteUserById(string userId);
}