namespace MyWebAPI.Models;

public class UserModel
{
    public string UserId { get; init; }
    
    public string Email { get; set; }
    
    public string Username { get; set; }

    private string _password;

    public UserModel(string email, string username)
    {
        UserId = Guid.NewGuid().ToString();
        Email = email;
        Username = username;
        _password = "*#06#";
    }

    public UserModel UpdateUserInfo(string email, string username)
    {
        Email = email;
        Username = username;
        return this;
    }

    public UserModel SetPassword(string password)
    {
        _password = password;
        return this;
    }
}