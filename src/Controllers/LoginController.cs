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
    public async Task<RedirectToActionResult> Index(User user)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Inputs aren't valid";
            return RedirectToAction("Index");
        }

        List<User> loginUser = await _userRepository.Login(user);
        if (!loginUser.Any())
        {
            TempData["Error"] = "Username or Password is incorrect";
            return RedirectToAction("Index");
        }

        List<Claim> claims = new List<Claim>
        {
            new (ClaimTypes.Name, loginUser[0].Username),
            new (ClaimTypes.NameIdentifier, loginUser[0].Id.ToString()),
            new (ClaimTypes.Role, loginUser[0].Admin.ToString())
        };
        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "User");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        Thread.CurrentPrincipal = claimsPrincipal;

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));
        
        return RedirectToAction("Index", "Home");
    }
    public ActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<RedirectToActionResult> Register(User user)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Inputs aren't valid";
            return RedirectToAction("Register");
        }

        if (!await _userRepository.Register(user))
        {
            TempData["Error"] = "Username already taken";
            return RedirectToAction("Register");
        }

        return RedirectToAction("Index");
    }
    
    public async Task<RedirectToActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index");
    }
}