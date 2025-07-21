using SimpleAuthApi.Models;

namespace SimpleAuthApi.Static;

public static class UserStore
{
    public static List<User> Users = new()
    {
        new User { Username = "son", Password = "123", FullName = "Son Huynh" },
        new User { Username = "john", Password = "123", FullName = "John Doe" },
        new User { Username = "anna", Password = "456", FullName = "Anna Smith" }
    };
}
