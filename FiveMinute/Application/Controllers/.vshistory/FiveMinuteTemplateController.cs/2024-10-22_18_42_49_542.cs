using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteTemplateController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> userManager;

		public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager)
		{
			_logger = logger;
			this.userManager = userManager;
		}

		public IActionResult Index() { return View(); }

        [HttpPost]
        public IActionResult Create() 
        {
            

            var newFMT = new FiveMinuteTemplate 
            { 
                CreationTime = DateTime.Now,
                LastModificationTime=DateTime.Now,  
                UserOwnerId = 
            };

            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
