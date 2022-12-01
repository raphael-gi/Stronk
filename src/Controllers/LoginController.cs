using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Stronk.Data;
using Stronk.Models;

namespace Stronk.Controllers;

public class LoginController : Controller
{
    private readonly UserRepository _userRepository;

    public LoginController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(User user)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Inputs aren't valid");
            return Redirect("/Login");
        }

        int id = _userRepository.Login(user);
        if (id < 0)
        {
            return Redirect("/Login");
        }

        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Username)
        };
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Login");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        Thread.CurrentPrincipal = claimsPrincipal;

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        
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

        if (!_userRepository.Register(user))
        {
            Console.WriteLine("Register failed");
            return Redirect("/Login/Register");
        }

        return Redirect("/Login");
    }

    public ActionResult Denied()
    {
        return View();
    }
}