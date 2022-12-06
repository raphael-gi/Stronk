using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Stronk.Models;
using Stronk.Data;

namespace Stronk.Repositories;

public class UserRepository
{
    private readonly DatabaseContext _databaseContext;

    public UserRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<int> Login(User user)
    {
        List<User> check = await _databaseContext.Users.Where(u => user.Username == u.Username && Hash(user.Password) == u.Password).ToListAsync();
        if (!check.Any())
        {
            Console.WriteLine("User doesn't exist");
            return -1;
        }
        return check[0].Id;
    }

    public async Task<bool> Register(User user)
    {
        List<int> taken = await _databaseContext.Users.Where(u => u.Username == user.Username).Select(u => u.Id).ToListAsync();
        if (taken.Any())
        {
            Console.WriteLine("Username already taken");
            return false;
        }

        user.Password = Hash(user.Password);
        await _databaseContext.Users.AddAsync(user);
        if (await _databaseContext.SaveChangesAsync() < 1)
        {
            return false;
        }
        return true;
    }
    private string Hash(string password)
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