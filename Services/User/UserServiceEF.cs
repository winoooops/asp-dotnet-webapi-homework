using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;
using MyWebAPI.Models.Context;

namespace MyWebAPI.Services.User;

public class UserServiceEF : IUserServiceEF
{
  private readonly ModelDBContext _db;

  public UserServiceEF(ModelDBContext db)
  {
    _db = db;
  }

  public IQueryable<Models.User> GetQueryable()
  {
    return _db.Users
      .Include(u => u.Teacher)
      .AsNoTracking();
  }
  
  public async Task<IEnumerable<Models.User>> GetAllAsync()
  {
    return await GetQueryable().ToListAsync(); 
  }

  public async Task<Models.User?> GetByIdAsync(int id)
  {
    // return await _db.Users.FindAsync(id);
    return await _db.Users.Include(u => u.Teacher)
      .FirstOrDefaultAsync(u => u.Id == id);
  }

  public async Task<Models.User> CreateAsync(Models.User user)
  {
    await _db.Users.AddAsync(user);
    await _db.SaveChangesAsync();
    return user;
  }

  public async Task<Models.User?> UpdateAsync(Models.User user)
  {
    var existingUser = await _db.Users.FindAsync(user.Id);
    if (existingUser == null)
    {
      return null;
    }
    
    _db.Entry(existingUser).CurrentValues.SetValues(user);
    await _db.SaveChangesAsync();

    return user;
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var existingUser = await _db.Users.FindAsync(id);
    if (existingUser == null)
    {
      return false;
    }

    _db.Users.Remove(existingUser);
    await _db.SaveChangesAsync();

    return true;
  }
}
