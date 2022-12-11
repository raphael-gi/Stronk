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
        return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
    }
    public async Task<IActionResult> Index()
    {
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
            TempData["Error"] = "Please select atleast one workout";
            return RedirectToAction("Create");
        }

        if (post.Title == null || post.Title.Length > 50)
        {
            TempData["Error"] = "Please make sure all of the inputs are correct";
            return RedirectToAction("Create");
        }
        post.UserId = GetId();
        if (!await _postRepository.CreatePost(post, workouts))
        {
            TempData["Error"] = "Post couldn't be added";
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
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}