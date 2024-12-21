using FiveMinutes.Data;
using FiveMinutes.Models;
using Microsoft.AspNetCore.Authorization;
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
		[Authorize(Roles = UserRoles.Admin + "," + UserRoles.Teacher)]
		public IActionResult Create() 
        {
            var currentUser = userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                throw new ApplicationException("Хуйня какая-то, как ты без регистрации пытаешься создать пятиминутку?");
            }

            var newFMT = new FiveMinuteTemplate 
            { 
                CreationTime = DateTime.Now,
                LastModificationTime=DateTime.Now,  
                UserOwnerId = currentUser.Id,
                ShowInProfile = true,
                Name = "Новая пятиминутка"
            };

            return View();
        }

        public IActionResult Edit()
        {
            return View();
        }
    }
}
