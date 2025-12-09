using System.ComponentModel.DataAnnotations;

namespace MyWebAPI.Models;

public class User : BaseModel
{
    [EmailAddress(ErrorMessage = "Email is invalid")]
    public string Email { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    [Required]
    public int Gender { get; set; }
    
    public Teacher? Teacher { get; set; }
    
    public int? TeacherId { get; set; }
    
    public User() { }

    public User(int id, string email, string username,  string address, int gender)
    {
        Id = id; 
        Email = email;
        Username = username;
        Address = address;
        Gender = gender;
    }

    public User UpdateUserInfo(string email, string username)
    {
        Email = email;
        Username = username;
        return this;
    }
}
