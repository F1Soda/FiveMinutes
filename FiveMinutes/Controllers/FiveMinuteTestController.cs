using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteTestController : Controller
    {
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
