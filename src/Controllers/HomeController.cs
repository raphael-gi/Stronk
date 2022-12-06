using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronk.Models;
using Stronk.Repositories;

namespace Stronk.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly PostRepository _postRepository;

    public HomeController(PostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    public async Task<IActionResult> Index()
    {
        ViewBag.Posts = await _postRepository.GetPosts();
        return View();
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Workouts = await _postRepository.GetWorkouts();
        return View();
    }
    [HttpPost]
    public async Task<RedirectResult> Create(Post post, int[] workouts)
    {
        if (workouts.Length < 1)
        {
            Console.WriteLine("Please select atleast one workout");
            return Redirect("/Home/Create");
        }
        post.UserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        if (!await _postRepository.CreatePost(post, workouts))
        {
            return Redirect("/Home/Create");
        }
        return Redirect("/Home");
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}