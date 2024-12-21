using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
