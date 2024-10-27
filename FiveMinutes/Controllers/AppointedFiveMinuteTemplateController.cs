using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers;

public class AppointedFiveMinuteTemplateController : Controller
{
    public IActionResult AllTests()
    {
        return View();
    }
    
    public IActionResult Test()
    {
        return View();
    }
    
    public IActionResult TestPassing()
    {
        return View();
    }


}