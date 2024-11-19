using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Runtime.Intrinsics.X86;

namespace FiveMinutes.Controllers
{
	public class FiveMinuteTemplateController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<AppUser> userManager;
		public readonly ApplicationDbContext context;
		private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;

		public FiveMinuteTemplateController(UserManager<AppUser> userManager, ApplicationDbContext context)
		{
			this.userManager = userManager;
			this.context = context;
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

				context.SaveChanges();

				var fmtViewModel = new FiveMinuteTemplateEditViewModel
				{
					Id = newFMT.Id,
					Name = "Модель представления пятиминутки",
					ShowInProfile = true,
					Questions = new List<QuestionEditViewModel> {new QuestionEditViewModel
					{
						QuestionText= "LOL",
						Position = 0,
						ResponseType = Models.ResponseType.SingleChoice
					} }
				};
				HttpContext.Session.SetString("FmtViewModel", JsonConvert.SerializeObject(fmtViewModel));

				return RedirectToAction("Edit");
			}
			throw new ApplicationException("Неудачная попытка добавить в бд пятиминутку");
		}


		public IActionResult Edit()
		{
			var fmtViewModelJson = HttpContext.Session.GetString("FmtViewModel");
			if (string.IsNullOrEmpty(fmtViewModelJson))
			{
				// Handle the case where the view model is not found in session state
				return RedirectToAction("Index"); // or some other appropriate action
			}

			var fmtViewModel = JsonConvert.DeserializeObject<FiveMinuteTemplateEditViewModel>(fmtViewModelJson);
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

		public async Task<IActionResult> Save(FiveMinuteTemplateEditViewModel fmt)
		{
			Console.WriteLine("Save was called!");

			if (ModelState.IsValid)
			{
				var currentFmtJson = HttpContext.Session.GetString("FmtViewModel");

				if (currentFmtJson is not null)
				{
					var currentFmt = JsonConvert.DeserializeObject<FiveMinuteTemplateEditViewModel>(currentFmtJson);

					if (currentFmt != null)
					{
						var id = currentFmt.Id; // Access the Id property

						// Retrieve the entity from the database
						var existingFmt = await fiveMinuteTemplateRepository.GetByIdAsyncNoTracking(id);

						if (existingFmt != null)
						{
							// Attach the entity to the context
							context.FiveMinuteTemplates.Attach(existingFmt);

							// Update the entity with the new values from the view model
							existingFmt.Name = fmt.Name;
							existingFmt.ShowInProfile = fmt.ShowInProfile;
							existingFmt.LastModificationTime = DateTime.UtcNow;
							existingFmt.Questions = fmt.Questions
								.Select(question => new Question
								{
									QuestionText = question.QuestionText,
									FiveMinuteTemplate = existingFmt,
									Position = question.Position,
									ResponseType = question.ResponseType,
									FiveMinuteTemplateId = fmt.Id,
									Answers = question.Answers
								}).ToList();

							// Mark the entity as modified
							context.Entry(existingFmt).State = EntityState.Modified;

							fiveMinuteTemplateRepository.Save();

							return Json(new { success = true });
						}
					}
				}
			}

			return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
		}




		private static FiveMinuteTemplate RecreateFMTByFMTAndViewModel(FiveMinuteTemplate fmt, FiveMinuteTemplateEditViewModel fmtViewModel)
		{
			var res = new FiveMinuteTemplate
			{
				Id = fmt.Id,
				Name = fmt.Name,
				CreationTime = fmt.CreationTime,
				LastModificationTime = DateTime.UtcNow,
				ShowInProfile = fmt.ShowInProfile,
				UserOwner = fmt.UserOwner,
				UserOwnerId = fmt.UserOwnerId,
				// Кажется есть проблемы с тем, что у прошлые вопросы не удаляются в бд
				Questions = fmtViewModel.Questions
					.Select(question => new Question
					{
						QuestionText =question.QuestionText,
						FiveMinuteTemplate = fmt,
						Position = question.Position,
						ResponseType = question.ResponseType,
						FiveMinuteTemplateId = fmt.Id,
						Answers = question.Answers
					})
			};

			return res;
		}
	}
}
