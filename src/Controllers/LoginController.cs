using Microsoft.AspNetCore.Mvc;
using Stronk.Models;
using Stronk.Data;

namespace Stronk.Controllers;

public class LoginController : Controller
{
    private DatabaseConn _databaseConn;
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public RedirectResult Index(User user)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Inputs aren't valid");
            return Redirect("/Login");
        }
        _databaseConn = new DatabaseConn("usp_Login");
        if (!_databaseConn.Login(user))
        {
            Console.WriteLine("Incorrect Login");
            return Redirect("/Login");
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
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Inputs aren't valid");
            return Redirect("/Login/Register");
        }

        _databaseConn = new DatabaseConn("usp_Register");
        if (!_databaseConn.Register(user))
        {
            Console.WriteLine("Register Unsuccessful");
            return Redirect("/Login/Register");
        }
        return Redirect("/Login");
    }
}