using FiveMinutes.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FiveMinutes.Controllers
{
	public class WelcomeController : Controller
	{
		private readonly ILogger<WelcomeController> _logger;

		public WelcomeController(ILogger<WelcomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
