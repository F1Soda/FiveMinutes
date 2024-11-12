using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteTemplateController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> userManager;

        // Удалить
        public readonly ApplicationDbContext context;

        private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;
        private readonly IQuestionRepository questionRepository;

        public FiveMinuteTemplateController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
            this.fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
            this.questionRepository = new QuestionRepository(context);
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var currentUser = await userManager.GetUserAsync(User);

            if (currentUser == null)
                return View("Error", new ErrorViewModel($"Attempt to create FMT by non register user"));

            var newFMT = FiveMinuteTemplate.CreateDefault(currentUser);
            if (fiveMinuteTemplateRepository.Add(newFMT))
            {
                currentUser.AddFMT(newFMT);

                // CHANGE!!!
                context.SaveChanges();

                return RedirectToAction("Edit", new { newFMT.Id });
            }
            return View("Error", new ErrorViewModel("Fail to add FMT to db"));
        }


        public async Task<IActionResult> Edit(int id)
        {
            var fmt = await fiveMinuteTemplateRepository.GetByIdAsync(id);

            if (fmt == null)
                return View("NotFound");

            var fmtViewModel = FiveMinuteTemplateEditViewModel.CreateByModel(fmt);

            // Change !!!
            HttpContext.Session.SetInt32("FmtViewModel", fmt.Id);
            return View(fmtViewModel);
        }

        public IActionResult TestCreation()
        {
            return View();
        }

        public IActionResult AllFiveMinuteTemplates()
        {
            return View();
        }

        public IActionResult FiveMinuteFolder()
        {
            return View();
        }

        public async Task<JsonResult> Save([FromBody] FiveMinuteTemplateEditViewModel fmt)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                });
            }

            var currentFMTId = HttpContext.Session.GetInt32("FmtViewModel");
            if (currentFMTId is null)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var existingFmt = await fiveMinuteTemplateRepository.GetByIdAsyncNoTracking(currentFMTId.Value);
            if (existingFmt is null)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                }); //тут какая-то другая ошибка должна быть
            }

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
                    Answers = question.Answers.Select(x => new Answer
                    {
                        IsCorrect = x.IsCorrect, 
                        Position = x.Position,
                        Text =x.Text,
                        // Вот эту залупу починить как то надо или не надо ???
                        QuestionId = 0
                    }).ToList()
                }).ToList();
            // Mark the entity as modified
            context.Entry(existingFmt).State = EntityState.Modified;
            fiveMinuteTemplateRepository.Save();
            return Json(new { success = true });
        }

        public async Task<IActionResult> Copy(int fiveMinuteId)
        {
			var currentUser = await userManager.GetUserAsync(User);
			if (currentUser == null)
			{
				return RedirectToAction("Login", "Account"); // Redirect to login if the user is not authenticated
			}

			var currentUserRoles = await userManager.GetRolesAsync(currentUser);

			bool isStudent = currentUserRoles.Contains(UserRoles.Student);

			if (isStudent)
			{
				// Redirect students trying to view other users' profiles
				return Forbid(); // or RedirectToAction("AccessDenied") if you have an Access Denied page
			}

            var fmt = await fiveMinuteTemplateRepository.GetByIdAsync(fiveMinuteId);

            if (fmt == null)
            {
				return RedirectToAction("NotFound");
			}

            var copyFMT = fmt.GetCopyToUser(); 
		}
    }
}
