using FiveMinutes.Data;
using FiveMinutes.Models;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers;

public class FiveMinutesController : Controller
{
    public IActionResult Test()
    {
        return View();
    }

    public IActionResult TestPassing()
    {
        return View();
    }

    public IActionResult TestCreation()
    {
        return View();
    }

    public IActionResult AllFiveMinutesTemplates()
    {
        return View();
    }

    public IActionResult AllTests()
    {
        return View();
    }

    public IActionResult FiveMinuteFolder()
    {
        return View();
    }
}