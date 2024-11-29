using FiveMinute.Database;
using FiveMinute.Repository.FiveMinuteTemplateRepository;
using FiveMinute.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers;

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