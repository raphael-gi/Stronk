using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronk.Data;
using Stronk.Models;

namespace Stronk.Controllers;

[Authorize]
public class WorkoutController : Controller
{
    private readonly WorkoutRepository _workoutRepository;

    public WorkoutController(WorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }
    public IActionResult Index()
    {
        ViewBag.Workouts = _workoutRepository.Workouts();
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.Exercises = _workoutRepository.Exercises();
        return View();
    }

    [HttpPost]
    public RedirectResult Create(Workout workout)
    {
        return Redirect("/Workout");
    }
}