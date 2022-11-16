using Microsoft.AspNetCore.Mvc;
using Stronk.Models;
using System.Data.SqlClient;
using BCrypt.Net;

namespace Stronk.Controllers;

public class LoginController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public RedirectResult Index(User user)
    {
        if (BCrypt.Net.BCrypt.Verify(user.Password, "f"))
        {
            Console.WriteLine("correct");
        }
        return Redirect("/Home");
    }
    public ActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public RedirectResult Register(User user)
    {
        string usernameHash = BCrypt.Net.BCrypt.HashPassword(user.Username);
        SqlConnection sqlConnection = new SqlConnection();

        return Redirect("/Login/Register");
    }
}