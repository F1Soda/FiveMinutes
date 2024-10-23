using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FiveMinutes.Controllers
{
	public class WelcomeController : Controller
	{
		private readonly ILogger<WelcomeController> _logger;
        private readonly UserManager<AppUser> userManager;

		public WelcomeController(ILogger<WelcomeController> logger)
		{
			_logger = logger;
			this.userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			return View(userManager);
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
