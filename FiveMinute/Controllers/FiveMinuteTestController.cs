using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FiveMinute.Data;
using FiveMinute.ViewModels.AccountViewModels;
using Microsoft.EntityFrameworkCore;
using FiveMinute.ViewModels;
using FiveMinute.Repository;
using FiveMinute.Interfaces;

namespace FiveMinute.Controllers
{
	public class FiveMinuteTestController : Controller
	{
		private readonly UserManager<AppUser> userManager;
		private readonly ApplicationDbContext context;
		
		private readonly IFiveMinuteTestRepository fiveMinuteTestRepository;
		private readonly IFiveMinuteResultsRepository fiveMinuteResultsRepository;

		public FiveMinuteTestController(UserManager<AppUser> userManager, ApplicationDbContext context)
		{
			this.userManager = userManager;
			this.context = context;
			fiveMinuteTestRepository = new FiveMinuteTestRepository(context);
			fiveMinuteResultsRepository = new FiveMinuteResultRepository(context);
		}

		public async Task<IActionResult> Edit(int testId)
		{
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testId);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (fmTest == null)
				return View("NotFound");

			var fmTestViewModel = FiveMinuteTestEditViewModel.CreateByModel(fmTest);

			ViewData["TemplateId"] = fmTest.Id;
			return View(fmTestViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(FiveMinuteTestEditViewModel fmTestEditViewModel)
		{
			var existingFMTest = await fiveMinuteTestRepository.GetByIdAsync(fmTestEditViewModel.Id);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null) // || !canCreate
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (existingFMTest == null)
				return View("NotFound");

			var updatedTest = new FiveMinuteTest
			{
				Id = fmTestEditViewModel.Id,
				Name = fmTestEditViewModel.Name,
				FiveMinuteTemplate = fmTestEditViewModel.AttachedFMT,
				FiveMinuteTemplateId = fmTestEditViewModel.AttachedFMT.Id,
				Status = existingFMTest.Status,
				StartPlanned = fmTestEditViewModel.StartPlanned,
				StartTime = fmTestEditViewModel.StartTime,
				EndPlanned = fmTestEditViewModel.EndPlanned,
				EndTime = fmTestEditViewModel.EndTime,
				Results = existingFMTest.Results,
			};

			await fiveMinuteTestRepository.Update(existingFMTest, updatedTest);

			return RedirectToAction("Detail", new { id = existingFMTest.Id });
		}

		public async Task<IActionResult> Detail(int testId)
		{
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testId);
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights to this action"));

			if (fmTest == null)
				return View("NotFound");

			return View(FiveMinuteTestDetailViewModel.CreateByModel(fmTest));
		}

		public async Task<IActionResult> Create(int templateId)
		{
			var currentUser = await userManager.GetUserAsync(User);

			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));

			ViewData["templateId"] = templateId;
			
			var user = await context.Users.Include(x => x.FMTTemplates).FirstOrDefaultAsync(x => x.Id == currentUser.Id);
			var model = new UserDetailViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				FMTs = user.FMTTemplates,
				UserRole = user.UserRole,
				IsOwner = true,
			};
			
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(FiveMinuteTestDetailViewModel fmTestEditViewModel)
		{
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null || !currentUser.canCreate)
				return View("Error", new ErrorViewModel($"You don't have the rights for this action"));
			
			var user = await context.Users.Include(x => x.FMTTemplates).ThenInclude(x => x.Questions).FirstOrDefaultAsync(x => x.Id == currentUser.Id);
			var attachedTemplate = user.FMTTemplates.FirstOrDefault(x => x.Id == fmTestEditViewModel.AttachedFMTId);

			var test = new FiveMinuteTest
			{
				Name = fmTestEditViewModel.Name,
				FiveMinuteTemplate = attachedTemplate,
				FiveMinuteTemplateId = fmTestEditViewModel.AttachedFMTId,
				UserOrganizer = user,
				UserOrganizerId = user.Id,
				PositionsToInclude = attachedTemplate.Questions.Select(x => x.Position).ToList(),
				Results = new List<FiveMinuteTestResult>()
			};
			
			context.FiveMinuteTests.Add(test);
			await context.SaveChangesAsync();
			return RedirectToAction("Detail", "Account");
		}


		public async Task<IActionResult> Pass(int testId)
		{
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testId);
			if (fmTest is null)
			{
				return View("NotFound");
			}

			var fmt = fmTest.FiveMinuteTemplate;
			var test = new FiveMinuteTestViewModel
			{
				Name = fmt.Name,
				FMTestId = fmTest.Id,
				Questions = fmt.Questions.Where(x => fmTest.PositionsToInclude.Contains(x.Position))
										 .Select(x => new QuestionViewModel
				{
					Id = x.Id,
					Position = x.Position,
					QuestionText = x.QuestionText,
					ResponseType = x.ResponseType,
					AnswerOptions = x.AnswerOptions.Select(answer => new AnswerViewModel()
					{
						Id = answer.Id,
						QuestionId = answer.QuestionId,
						Position = answer.Position,
						Text = answer.Text,
					}).ToList(),
				}),
			};
			return View(test);
		}

		[HttpPost]
		public async Task<IActionResult> SendTestResults(TestResultViewModel testResultViewModel)
		{
			// TODO: По хорошему нужно создать в форме поле для имени, если чел не зареган

			var testResult = await ConvertViewModelToFiveMinuteResult(testResultViewModel);
			var currentUser = await userManager.GetUserAsync(User);

			testResult.UserId = currentUser?.Id;
			testResult.UserName = testResultViewModel.UserName;


			if (!await fiveMinuteTestRepository.AddResultToTest(testResultViewModel.FMTestId, testResult))
				return View("Error", new ErrorViewModel($"Something is wrong. Could not save your answers"));
			return RedirectToAction("Index", "Home");
		}

		public UserAnswer CheckUserAnswer(UserAnswerViewModel userAnswer, FiveMinuteTemplate fiveMinuteTemplate)
		{
			var question = fiveMinuteTemplate.Questions.FirstOrDefault(q => q.Position == userAnswer.QuestionPosition);
			var dbAnswer = question?.AnswerOptions.FirstOrDefault(x => x.Position == userAnswer.Position);
			if (dbAnswer == null)
			{
				// throw new Exception($"Вопрос ,на который указывает ответ юзера {userAnswer} не существует в ");
				// Для текстового ответа надо подумать что делать
			}
			return new UserAnswer
			{
				QuestionId = question.Id,
				Text = userAnswer.Text ?? "",
				Position = userAnswer.Position,
				IsCorrect = (dbAnswer?.IsCorrect ?? false) && userAnswer.Text == dbAnswer?.Text,
				QuestionPosition = userAnswer.QuestionPosition,
				QuestionText = question?.QuestionText ?? "",
			};
		}

		public async Task<FiveMinuteTestResult> ConvertViewModelToFiveMinuteResult(TestResultViewModel testResult)
		{
			var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testResult.FMTestId);
			return new FiveMinuteTestResult
			{
				Answers = testResult.UserAnswers.Select(ans => CheckUserAnswer(ans, fmTest.FiveMinuteTemplate)).ToList(),
				FiveMinuteTestId = testResult.FMTestId,
				PassTime = DateTime.UtcNow,
			};
		}
	}
}