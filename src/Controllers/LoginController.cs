using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Stronk.Models;
using Stronk.Repositories;

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

        int id = await _userRepository.Login(user);
        if (id < 0)
        {
            return Redirect("/Login");
        }

        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.NameIdentifier, id.ToString())
        };
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "User");
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
    public async Task<RedirectResult> Register(User user)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Inputs aren't valid");
            return Redirect("/Login/Register");
        }

        if (!await _userRepository.Register(user))
        {
            Console.WriteLine("Register failed");
            return Redirect("/Login/Register");
        }

        return Redirect("/Login");
    }
    
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/Login");
    }

    public ActionResult Denied()
    {
        return View();
    }
}