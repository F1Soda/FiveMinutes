using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers;

public class AppointedFiveMinuteTemplateController : Controller
{
    public readonly ApplicationDbContext context;
    private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;
    public IActionResult AllTests()
    {
        return View();
    }
    
    public IActionResult Test()
    {
        return View();
    }
    
    public IActionResult TestPassing(int testId)
    {
        var fiveMinuteTemplate = fiveMinuteTemplateRepository.GetByIdAsyncNoTracking(testId);
        
        return View();
    }


}