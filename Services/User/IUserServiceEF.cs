namespace MyWebAPI.Services;

public interface IUserServiceEF
{
  public Task<IEnumerable<Models.User>> GetAllAsync();
  
  public Task<Models.User?> GetByIdAsync(int id);
  
  public Task<Models.User?> GetByEmailAsync(string email);
  
  public Task<Models.User?> GetByRefreshTokenAsync(string username);
  
  public Task<Models.User> CreateAsync(Models.User user);
  
  public Task<Models.User?> UpdateAsync(Models.User user);
  
  public Task<bool> DeleteAsync(int id);
}