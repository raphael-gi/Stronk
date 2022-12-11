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
        ViewBag.Workouts = await _workoutRepository.GetWorkouts(GetId());
        return View();
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Exercises = await _exerciseRepository.GetExercises(GetId());
        return View();
    }
    [HttpPost]
    public async Task<RedirectToActionResult> Create(Workout workout, int[] exercises)
    {
        string? errorMessage = CheckErrors(workout, exercises);
        if (errorMessage != null)
        {
            TempData["Error"] = errorMessage;
            return RedirectToAction("Edit", "Workout", workout.Id);
        }
        
        workout.UserId = GetId();
        if (!await _workoutRepository.CreateWorkout(workout, exercises))
        {
            TempData["Error"] = "Workout could not be added";
            return RedirectToAction("Create");
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Edit(int id)
    {
        ViewBag.Exercises = await _exerciseRepository.GetExercises(GetId());
        ViewBag.SelectedExercises = await _workoutRepository.GetSelectedExercises(id);
        return View(await _workoutRepository.GetWorkout(id));
    }
    [HttpPost]
    public async Task<RedirectToActionResult> Edit(Workout workout, int[] exercises)
    {
        string? errorMessage = CheckErrors(workout, exercises);
        if (errorMessage != null)
        {
            TempData["Error"] = errorMessage;
            return RedirectToAction("Edit", "Workout", workout.Id);
        }
        if (!await _workoutRepository.EditWorkout(workout, exercises))
        {
            return RedirectToAction("Edit", "Workout", workout.Id);
        }

        return RedirectToAction("Index");
    }
    private string? CheckErrors(Workout workout, int[] exercises)
    {
        string? error = null;
        if (workout.Name == null || workout.Name.Length > 50)
        {
            error = "Please make sure all of the inputs are correct";
        }
        if (exercises.Length < 1)
        {
            error = "Please select an exercise";
        }
        return error;
    }

    public async Task<RedirectToActionResult> Delete(int id)
    {
        if (!await _workoutRepository.DeleteWorkout(id))
        {
            Console.WriteLine("Workout could not be deleted");
        }

        return RedirectToAction("Index");
    }
}