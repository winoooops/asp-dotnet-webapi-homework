namespace MyWebAPI.Models;

public class User
{
    public int Id { get; init; }
    
    public string Email { get; set; }
    
    public string Username { get; set; }
    
    public string Address { get; set; }
    
    public int Gender { get; set; }
    
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