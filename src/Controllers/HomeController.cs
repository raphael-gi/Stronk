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
    private int GetId()
    {
        return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
    }
    public async Task<IActionResult> Index()
    {
        Console.WriteLine(User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value);
        ViewBag.Posts = await _postRepository.GetPosts();
        ViewBag.Amount = await _postRepository.GetPostsAmount(GetId());
        return View();
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Workouts = await _postRepository.GetWorkouts(GetId());
        return View();
    }
    [HttpPost]
    public async Task<RedirectToActionResult> Create(Post post, int[] workouts)
    {
        if (workouts.Length < 1)
        {
            Console.WriteLine("Please select atleast one workout");
            return RedirectToAction("Create");
        }
        post.UserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
        if (!await _postRepository.CreatePost(post, workouts))
        {
            return RedirectToAction("Create");
        }
        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> Delete(int id)
    {
        if (!await _postRepository.DeletePost(id))
        {
            Console.WriteLine("Couldn't be deleted");
        }
        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> Copy(int id)
    {
        if (!await _postRepository.Copy(id, GetId()))
        {
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index", "Workout");
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