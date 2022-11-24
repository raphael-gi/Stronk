using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stronk.Data;
using Stronk.Models;

namespace Stronk.Controllers;

public class HomeController : Controller
{
    private DatabaseConn _databaseConn;

    public HomeController()
    {
        //Check for session
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Workouts()
    {
        return View();
    }
    public IActionResult Exercises()
    {
        _databaseConn = new DatabaseConn("SELECT [Name], [Description], MuscleName FROM vw_Exercise");
        List<Exercise> exercises = _databaseConn.Exercise();
        ViewBag.Exercises = exercises;
        return View();
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