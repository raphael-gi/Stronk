using System.Security.Cryptography;
using System.Text;
using Stronk.Models;

namespace Stronk.Data;

public class UserRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public int Login(User user)
    {
        List<User> check = _databaseContext.Users.Where(u => user.Username == u.Username && Hash(user.Password) == u.Password).ToList();
        if (!check.Any())
        {
            Console.WriteLine("User doesn't exist");
            return -1;
        }
        return check[0].Id;
    }

    public bool Register(User user)
    {
        List<int> taken = _databaseContext.Users.Where(u => u.Username == user.Username).Select(u => u.Id).ToList();
        if (taken.Any())
        {
            Console.WriteLine("Username already taken");
            return false;
        }

        user.Password = Hash(user.Password);
        _databaseContext.Users.Add(user);
        _databaseContext.SaveChanges();
        return true;
    }
    static string Hash(string password)
    {
        string hash = String.Empty;
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            foreach (byte b in hashValue) {
                hash += $"{b:X2}";
            }
        }
        return hash;
    }
}