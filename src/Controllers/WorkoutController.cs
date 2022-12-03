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
    public async Task<IActionResult> Index()
    {
        ViewBag.Workouts = await _workoutRepository.GetWorkouts();
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.Exercises = _workoutRepository.Exercises();
        return View();
    }

    [HttpPost]
    public async Task<RedirectResult> Create(Workout workout, int[] exercises)
    {
        if (exercises.Length < 1)
        {
            Console.WriteLine("Please select an exercise");
            return Redirect("/Workout/Create");
        }
        if (!await _workoutRepository.CreateWorkout(workout, exercises))
        {
            Console.WriteLine("Workout could not be added");
            return Redirect("/Workout/Create");
        }
        return Redirect("/Workout");
    }
}