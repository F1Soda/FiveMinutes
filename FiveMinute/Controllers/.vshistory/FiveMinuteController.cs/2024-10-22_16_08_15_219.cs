using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
