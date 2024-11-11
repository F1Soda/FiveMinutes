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
            {
                throw new ApplicationException("Хуйня какая-то, как ты без регистрации пытаешься создать пятиминутку?");
            }

            var newFMT = new FiveMinuteTemplate
            {
                CreationTime = DateTime.UtcNow,
                LastModificationTime = DateTime.UtcNow,
                UserOwnerId = currentUser.Id,
                UserOwner = currentUser,
                Questions = new List<Question>()
                {
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

                return RedirectToAction("Edit", new { newFMT.Id });
            }

            throw new ApplicationException("Неудачная попытка добавить в бд пятиминутку");
        }


        public async Task<IActionResult> Edit(int id)
        {
            var fmt = await fiveMinuteTemplateRepository.GetByIdAsync(id);

            if (fmt == null)
            {
                return View("Error");
            }



            var fmtViewModel = new FiveMinuteTemplateEditViewModel
            {
                Id = fmt.Id,
                Name = fmt.Name,
                ShowInProfile = fmt.ShowInProfile,
                Questions = fmt.Questions
                    .Select(x => new QuestionEditViewModel()
                    {
                        QuestionText = x.QuestionText,
                        Position = x.Position,
                        ResponseType = Models.ResponseType.SingleChoice,
                        Answers = x.Answers
                    })
            };
            HttpContext.Session.SetString("FmtViewModel", JsonConvert.SerializeObject(fmtViewModel));
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

        public async Task<IActionResult> Save(FiveMinuteTemplateEditViewModel fmt)
        {
            Console.WriteLine("Save was called!");


            var currentFmtJson = HttpContext.Session.GetString("FmtViewModel");
            if (currentFmtJson is null)
            {
                return Json(new
                {
                    success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            var currentFmt = JsonConvert.DeserializeObject<FiveMinuteTemplateEditViewModel>(currentFmtJson);
            if (currentFmt is null)
            {
                return Json(new
                {
                    success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                }); //тут какая-то другая ошибка должна быть
            }

            var id = currentFmt.Id; // Access the Id property
            // Retrieve the entity from the database
            var existingFmt = await fiveMinuteTemplateRepository.GetByIdAsyncNoTracking(id);
            if (existingFmt is null)
            {
                return Json(new
                {
                    success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
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
                    Answers = question.Answers
                }).ToList();
            // Mark the entity as modified
            context.Entry(existingFmt).State = EntityState.Modified;
            fiveMinuteTemplateRepository.Save();
            return Json(new { success = true });
        }

        private async Task<FiveMinuteTemplate> RecreateFMTByFMTAndViewModel(FiveMinuteTemplate fmt,
            FiveMinuteTemplateEditViewModel fmtViewModel)
        {
            var res = fmt.GetCopy();
            res.Questions = fmtViewModel.Questions
                .Select(question => new Question
                {
                    QuestionText = question.QuestionText,
                    FiveMinuteTemplate = fmt,
                    Position = question.Position,
                    ResponseType = question.ResponseType,
                    FiveMinuteTemplateId = fmt.Id,
                    Answers = question.Answers
                });
            questionRepository.Save();
            await questionRepository.DeleteByFMT(fmt);

            return res;
        }
    }
}
