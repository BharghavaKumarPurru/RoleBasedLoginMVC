using System.Collections.Generic;
using WebApplication1.Models;
using WebApplication1;
using System.Linq;

public class AuthService
{
    private readonly YourDbContext _dbContext;

    public AuthService()
    {
        _dbContext = new YourDbContext();
    }

    public bool ValidateUser(string username, string password, out string role, out int userId)
{
    var user = _dbContext.Users
        .Where(u => u.Username == username && u.Password == password)
        .FirstOrDefault();

    if (user != null)
    {
        role = user.Role;
        userId = user.Id; // Include the user ID
        return true;
    }

    role = null;
    userId = 0;
    return false;
}


    public List<User> GetNonAdminUsers()
    {
        return _dbContext.Users.ToList(); // Retrieve all users
    }


    public void MakeUserAdmin(int userId)
    {
        var user = _dbContext.Users.Find(userId);
        if (user != null)
        {
            user.Role = "Admin";
            _dbContext.SaveChanges();
        }
    }

    public void DeleteUser(int userId)
    {
        var user = _dbContext.Users.Find(userId);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}
