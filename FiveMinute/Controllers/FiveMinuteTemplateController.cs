using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels.FMTEditViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FiveMinutes.Controllers
{
    public class FiveMinuteTemplateController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> userManager;


        private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;
        private readonly IUserRepository userRepository;
        // private readonly IQuestionRepository questionRepository;

        public FiveMinuteTemplateController(UserManager<AppUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            userRepository=new UserRepository(context);
            this.fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
            // this.questionRepository = new QuestionRepository(context);
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
            if (fiveMinuteTemplateRepository.Add(newFMT).Result)
            {
                await userRepository.AddFMTtoUser(newFMT, currentUser);

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
                    id = fmt.Id
                });
            }

            var currentFMTId = HttpContext.Session.GetInt32("FmtViewModel");
            if (currentFMTId is null)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                    id = fmt.Id
                });
            }

            var existingFmt = await fiveMinuteTemplateRepository.GetByIdAsyncNoTracking(currentFMTId.Value);
            if (existingFmt is null)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage),
                    id = fmt.Id
                }); //тут какая-то другая ошибка должна быть
            }
            var template = new FiveMinuteTemplate
            {
                Id = fmt.Id,
                Name = fmt.Name,
                ShowInProfile = fmt.ShowInProfile,
                LastModificationTime = DateTime.UtcNow,
                Questions = GetQuestionsByFMTViewModel(fmt, existingFmt),
            };
            await fiveMinuteTemplateRepository.Update(existingFmt, template);
            return Json(new { success = true, id = fmt.Id });
        }

        public List<Question> GetQuestionsByFMTViewModel(FiveMinuteTemplateEditViewModel fmt,FiveMinuteTemplate existingFmt)
        {
            if(fmt.Questions.Any(x=>x.QuestionText==""))
            {
                Console.Write("Поступил Пустой вопрос");
            }
            if (fmt.Questions.Select(x => x.Answers)
                .Any(x => x.Any(x => x.Text == null)))
            {
                Console.Write("Поступил Пустой ответ");
            }
            return fmt.Questions.Select(question =>new Question
            {
                QuestionText = question.QuestionText,
                FiveMinuteTemplate = existingFmt,
                Position = question.Position,
                ResponseType = question.ResponseType,
                FiveMinuteTemplateId = fmt.Id,
                AnswerOptions = question.Answers.Select(x => new Answer
                {
                    IsCorrect = x.IsCorrect, 
                    Position = x.Position,
                    Text =x.Text,
                    // Вот эту залупу починить как то надо или не надо ???
                    QuestionId = 0
                }).ToList()
            }).ToList();
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

            var copyFMT = fmt.GetCopyToUser(currentUser);
            
            if (fiveMinuteTemplateRepository.Add(copyFMT).Result)
            {
                await userRepository.AddFMTtoUser(copyFMT, currentUser);
                return RedirectToAction("Edit", new { copyFMT.Id });
			}
			return View("Error", new ErrorViewModel("Fail to add FMT to db"));
		}
    }
}
