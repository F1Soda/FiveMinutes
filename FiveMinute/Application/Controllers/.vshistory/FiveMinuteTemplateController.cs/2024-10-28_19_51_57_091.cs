using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
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
				Questions = new List<Question>(),
				ShowInProfile = true,
				// Вот тут надо будет переделать, чтобы добавлялся номер в конец, чтобы избавиться от повторения
				Name = "Новая пятиминутка"
			};

			if (fiveMinuteTemplateRepository.Add(newFMT))
			{
				//var fmtViewModel = new FiveMinuteTemplateEditViewModel
				//{
				//	Name = newFMT.Name,
				//	ShowInProfile = true,
				//	Questions = new List<QuestionEditViewModel>()
				//};

				return RedirectToAction("Edit", newFMT);
			}
			throw new ApplicationException("Неудачная попытка добавить в бд пятиминутку");
		}


		public IActionResult Edit(FiveMinuteTemplate fmt)
		{
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

		[HttpPost]
		public void Save([FromBody] FiveMinuteTemplate fmt)
		{
			if (!fiveMinuteTemplateRepository.Update(fmt))
			{
				Console.WriteLine("Fail to save FMT to data base");
			}
		}
	}
}
