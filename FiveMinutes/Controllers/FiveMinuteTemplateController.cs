using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteTemplateController : Controller
    {
		private readonly ILogger<WelcomeController> _logger;
		private readonly UserManager<AppUser> userManager;
        private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;

		public FiveMinuteTemplateController(UserManager<AppUser> userManager,ApplicationDbContext context)
        { 
			this.userManager = userManager;
            this.fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
		}

		public IActionResult Index() { return View(); }

        [HttpPost]
		public IActionResult Create() 
        {
	        var currentUser = userManager.GetUserAsync(User);
	        if (currentUser == null)
	        {
		        throw new ApplicationException("Попытка создать шаблон не зарегистрировавшись");
	        }
	        var newFMT = new FiveMinuteTemplate 
            { 
                CreationTime = DateTime.UtcNow,
                LastModificationTime=DateTime.UtcNow,  
                UserOwnerId = currentUser.Id,
                ShowInProfile = true,
                // Вот тут надо будет переделать, чтобы добавлялся номер в конец, чтобы избавиться от повторения
                Name = "Новая пятиминутка"
            };
            if (fiveMinuteTemplateRepository.Add(newFMT))
            {
				return RedirectToAction("Edit", new { id = newFMT.Id });
			}
			throw new ApplicationException("Неудачная попытка добавить в бд пятиминутку");
		}

        public IActionResult Edit(FiveMinuteTemplate fmt)
        {

	        Create();
            return View(fmt);
        }
    }
}
