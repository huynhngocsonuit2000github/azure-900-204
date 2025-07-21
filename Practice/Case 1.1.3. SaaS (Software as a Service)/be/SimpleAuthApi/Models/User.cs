namespace SimpleAuthApi.Models;

public class User
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // Plain for simplicity (not for real-world!)
    public string FullName { get; set; } = string.Empty;
}
