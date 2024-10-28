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
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> userManager;
        private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;

		public FiveMinuteTemplateController(UserManager<AppUser> userManager, ApplicationDbContext context)
        { 
			this.userManager = userManager;
            this.fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
		}

		public IActionResult Index() { return View(); }



        public IActionResult Edit(FiveMinuteTemplate fmt)
        {
            // ОЧЕНЬ ПЛОХО, НО МОЖЕТ И НЕТЬ
            return View(fmt);
        }
        
        public IActionResult TestCreation()
        {
	        return View();
        }
    
        public IActionResult AllFiveMinutesTemplates()
        {
	        return View();
        }
        
        public IActionResult FiveMinuteFolder()
        {
	        return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetFiveMinuteTemplate(int id)
        {
	        var fmt=await Task.Run(() =>fiveMinuteTemplateRepository.GetByIdAsync(id));
	        if (fmt == null)
	        {
		        return NotFound();
	        }
	        return View(fmt);
        }

        [HttpPost]
        public IActionResult Create(FiveMinuteTemplate model)
        {
	        model.CreationTime = DateTime.UtcNow;
	        model.LastModificationTime = DateTime.UtcNow;
	        fiveMinuteTemplateRepository.Add(model);
	        return RedirectToAction("Index");
        }
    }
}
