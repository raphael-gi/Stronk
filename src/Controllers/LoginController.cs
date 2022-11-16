using Microsoft.AspNetCore.Mvc;
using Stronk.Models;

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
        return Redirect("/Home");
    }
    public ActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public RedirectResult Register(User user)
    {
        Console.WriteLine(user.Username);
        return Redirect("/");
    }
}