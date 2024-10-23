using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class EducationTestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
