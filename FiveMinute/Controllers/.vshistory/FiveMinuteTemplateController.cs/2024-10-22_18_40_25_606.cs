using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteTemplateController : Controller
    {
        public IActionResult Index() { return View(); }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
