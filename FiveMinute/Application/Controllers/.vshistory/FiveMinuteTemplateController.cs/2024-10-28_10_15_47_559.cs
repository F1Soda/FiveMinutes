using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels;
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


		[HttpGet]
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
				LastModificationTime = DateTime.Now,
				UserOwnerId = currentUser.Id,
				ShowInProfile = true,
				// Вот тут надо будет переделать, чтобы добавлялся номер в конец, чтобы избавиться от повторения
				Name = "Новая пятиминутка"
			};

			if (fiveMinuteTemplateRepository.Add(newFMT))
			{
				var fmtViewModel = new FiveMinuteTemplateEditViewModel
				{
					Name = newFMT.Name,
					ShowInProfile = true,
					Questions = new List<QuestionEditViewModel>()
				};

				return RedirectToAction("Edit", fmtViewModel);
			}
			throw new ApplicationException("Неудачная попытка добавить в бд пятиминутку");
		}


		public IActionResult Edit(FiveMinuteTemplateEditViewModel fmtViewModel)
		{
			return View(fmtViewModel);
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

		[HttpPost]
		public IActionResult Save([FromBody] FiveMinuteTemplateEditViewModel fmtViewModel)
		{
			
			return RedirectToAction("Index");
		}
	}
}
