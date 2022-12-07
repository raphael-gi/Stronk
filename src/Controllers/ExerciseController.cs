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
    public async Task<RedirectResult> Create(Exercise exercise, int[] muscles)
    {
        if (muscles.Length < 1)
        {
            Console.WriteLine("Please select a muscle group");
            return Redirect("/Exercise/Create");
        }

        exercise.UserId = GetId();
        if (!await _exerciseRepository.CreateExercise(exercise, muscles))
        {
            Console.WriteLine("Exercise couldn't be added");
            return Redirect("/Exercise/Create");
        }

        return Redirect("/Exercise");
    }
    public async Task<ActionResult> Edit(int id)
    {
        Exercise exercise = await _exerciseRepository.GetExercise(id);
        ViewBag.SelectedMuscles = await _muscleRepository.GetSelectedMuscles(id);
        ViewBag.Muscles = await _muscleRepository.GetMuscles();
        return View(exercise);
    }

    [HttpPost]
    public async Task<RedirectResult> Edit(Exercise exercise, int[] muscles)
    {
        if (muscles.Length < 1)
        {
            return Redirect("/Exercise/Edit/" + exercise.Id);
        }

        if (!await _exerciseRepository.EditExercise(exercise, muscles))
        {
            return Redirect("/Exercise/Edit/" + exercise.Id);
        }

        return Redirect("/Exercise");
    }

    public async Task<RedirectResult> Delete(int id)
    {
        if (!await _exerciseRepository.DeleteExercise(id))
        {
            Console.WriteLine("Couldn't be deleted");
        }
        return Redirect("/Exercise");
    }
    public async Task<IActionResult> Details(int id)
    {
        ViewBag.Exercise = await _exerciseRepository.GetExercise(id);
        ViewBag.Muscles = await _exerciseRepository.GetMusclesByExercise(id);
        return View();
    }
}