using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronk.Data;
using Stronk.Models;
using Stronk.Repositories;

namespace Stronk.Controllers;

[Authorize]
public class WorkoutController : Controller
{
    private readonly WorkoutRepository _workoutRepository;
    private readonly ExerciseRepository _exerciseRepository;

    public WorkoutController(WorkoutRepository workoutRepository, ExerciseRepository exerciseRepository)
    {
        _workoutRepository = workoutRepository;
        _exerciseRepository = exerciseRepository;
    }

    private int GetId()
    {
        return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
    }
    public async Task<IActionResult> Index()
    {
        ViewBag.Workouts = await _workoutRepository.GetWorkouts();
        return View();
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Exercises = await _exerciseRepository.GetExercises(GetId());
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

        workout.UserId = 1;
        if (!await _workoutRepository.CreateWorkout(workout, exercises))
        {
            Console.WriteLine("Workout could not be added");
            return Redirect("/Workout/Create");
        }
        return Redirect("/Workout");
    }
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Exercises = await _exerciseRepository.GetExercises(GetId());
        ViewBag.SelectedExercises = await _workoutRepository.GetSelectedExercises(id);
        return View(await _workoutRepository.GetWorkout(id));
    }
    [HttpPost]
    public async Task<RedirectResult> Edit(Workout workout, int[] exercises)
    {
        if (exercises.Length < 1)
        {
            return Redirect("/Workout/Edit" + workout.Id);
        }
        if (!await _workoutRepository.EditWorkout(workout, exercises))
        {
            return Redirect("/Workout/Edit" + workout.Id);
        }
        
        return Redirect("/Workout");
    }
}