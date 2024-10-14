using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
