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
		public async Task<IActionResult> Create()
		{
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null)
			{
				throw new ApplicationException("Хуйня какая-то, как ты без регистрации пытаешься создать пятиминутку?");
			}

			var newFMT = new FiveMinuteTemplate
			{
				CreationTime = DateTime.UtcNow,
				LastModificationTime = DateTime.UtcNow,
				UserOwnerId = currentUser.Id,
				UserOwner = currentUser,
				Questions = new List<Question>() {
					new Question
					{
						QuestionText = "Вопрос 1",
						Position = 0,
						ResponseType = Models.ResponseType.SingleChoice,
					}
				},
				ShowInProfile = true,
				// Вот тут надо будет переделать, чтобы добавлялся номер в конец, чтобы избавиться от повторения
				Name = "Новая пятиминутка"
			};

			if (fiveMinuteTemplateRepository.Add(newFMT))
			{
				currentUser.AddFMT(newFMT);

				var fmtViewModel = new FiveMinuteTemplateEditViewModel
				{
					Name = "Модель представления пятиминутки",
					ShowInProfile = true,
					Questions = new List<QuestionEditViewModel> {new QuestionEditViewModel
					{
						QuestionText= "LOL",
						Position = 0,
						ResponseType = Models.ResponseType.SingleChoice
					} }
				};
				TempData["fmtViewModel"] = fmtViewModel;
				return RedirectToAction("Edit", fmtViewModel);
			}
			throw new ApplicationException("Неудачная попытка добавить в бд пятиминутку");
		}


		public IActionResult Edit()
		{
			return View(TempData["fmtViewModel"]);
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
		public async Task<IActionResult> Save(FiveMinuteTemplateEditViewModel fmt)
		{
			Console.WriteLine("Save was called!");

			if (ModelState.IsValid)
			{
				//if (fiveMinuteTemplateRepository.Update(fmt)) 
				//	return Json(new { success = true });
			}

			return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

		}
	}
}
