using FiveMinutes.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteTemplateController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> userManager;

		public FiveMinuteTemplateController(UserManager<AppUser> userManager)
        { 
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
