using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronk.Repositories;
using Stronk.Models;

namespace Stronk.Controllers;

[Authorize]
public class ExerciseController : Controller
{
    private readonly ExerciseRepository _exerciseRepository;
    
    public ExerciseController(ExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public ActionResult Index()
    {
        ViewBag.Muscles = _exerciseRepository.GetMuscles();
        ViewBag.Exercises = _exerciseRepository.GetExercises();
        return View();
    }

    public ActionResult Create()
    {
        ViewBag.Muscles = _exerciseRepository.GetMuscles();
        return View();
    }

    [HttpPost]
    public RedirectResult Create(Exercise exercise, string[] muscle)
    {
        muscle = muscle.Where(m => !m.Contains("false")).ToArray();
        //if (!ModelState.IsValid)
        //{
        //    Console.WriteLine(ModelState.ValidationState);
        //    Console.WriteLine("Model State is not valid");
        //    return Redirect("/Exercise/Create");
        //}
        if (!_exerciseRepository.Add(exercise, muscle))
        {
            Console.WriteLine("Exercise couldn't be added");
            return Redirect("/Exercise/Create");
        }

        return Redirect("/Exercise");
    }

    public ActionResult Edit(int id)
    {
        Exercise exercise = _exerciseRepository.GetExercise(id);
        Console.WriteLine(exercise.Name);
        ViewBag.Exercise = exercise;
        ViewBag.Muscles = _exerciseRepository.GetMuscles();
        return View();
    }
}