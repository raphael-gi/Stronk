using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronk.Repositories;
using Stronk.Models;

namespace Stronk.Controllers;

[Authorize]
public class ExerciseController : Controller
{
    private readonly ExerciseRepository _exerciseRepository;
    private readonly MuscleRepository _muscleRepository;
    
    public ExerciseController(ExerciseRepository exerciseRepository, MuscleRepository muscleRepository)
    {
        _exerciseRepository = exerciseRepository;
        _muscleRepository = muscleRepository;
    }
    
    private int GetId()
    {
        return int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value!);
    }


    public async Task<ActionResult> Index()
    {
        ViewBag.Exercises = await _exerciseRepository.GetExercises(GetId());
        return View();
    }
    public async Task<ActionResult> Create()
    {
        ViewBag.Muscles = await _muscleRepository.GetMuscles();
        return View();
    }
    [HttpPost]
    public async Task<RedirectToActionResult> Create(Exercise exercise, int[] muscles)
    {
        if (muscles.Length < 1)
        {
            Console.WriteLine("Please select a muscle group");
            return RedirectToAction("Create");
        }

        exercise.UserId = GetId();
        if (!await _exerciseRepository.CreateExercise(exercise, muscles))
        {
            Console.WriteLine("Exercise couldn't be added");
            return RedirectToAction("Create");
        }

        return RedirectToAction("Index");
    }
    public async Task<ActionResult> Edit(int id)
    {
        Exercise exercise = await _exerciseRepository.GetExercise(id);
        ViewBag.SelectedMuscles = await _muscleRepository.GetSelectedMuscles(id);
        ViewBag.Muscles = await _muscleRepository.GetMuscles();
        return View(exercise);
    }

    [HttpPost]
    public async Task<RedirectToActionResult> Edit(Exercise exercise, int[] muscles)
    {
        if (muscles.Length < 1)
        {
            return RedirectToAction("Edit", "Exercise", exercise.Id);
        }

        if (!await _exerciseRepository.EditExercise(exercise, muscles))
        {
            return RedirectToAction("Edit", "Exercise", exercise.Id);
        }
        return RedirectToAction("Index");
    }

    public async Task<RedirectToActionResult> Delete(int id)
    {
        if (!await _exerciseRepository.DeleteExercise(id))
        {
            Console.WriteLine("Couldn't be deleted");
        }
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Details(int id)
    {
        ViewBag.Exercise = await _exerciseRepository.GetExercise(id);
        ViewBag.Muscles = await _exerciseRepository.GetMusclesByExercise(id);
        return View();
    }
}