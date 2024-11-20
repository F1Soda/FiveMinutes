using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FiveMinutes.Controllers;

public class TestPassingController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;
    // private readonly IQuestionRepository _questionRepository;
    private readonly IFiveMinuteResultsRepository _fiveMinuteResultsRepository;
    
    private readonly UserManager<AppUser> _userManager;


    public TestPassingController(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        this.context = context;
        this.fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
        // _questionRepository = new QuestionRepository(context);
        _fiveMinuteResultsRepository = new FiveMinuteResultRepository(context);
        _userManager = userManager;
    }
    public IActionResult Test(int fiveMinuteId)
    {
        var fmt = fiveMinuteTemplateRepository.GetByIdAsync(fiveMinuteId).Result;
        var test = new FiveMinuteViewModel
        {
            Name = fmt.Name,
            Id = fmt.Id,
            Questions = fmt.Questions.Select(x => new QuestionViewModel
            {
                Id = x.Id,
                Position = x.Position,
                QuestionText = x.QuestionText,
                ResponseType = x.ResponseType,
                AnswerOptions = x.Answers.Select(answer => new AnswerViewModel()
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
    public async Task<JsonResult> SendTestResults(TestResultViewModel testResult)
    {
        // TODO: По хорошему нужно создать в форме поле для имени, если чел не зареган
            //.SelectMany(question => question.Answers)
            //.Where(answer => answer.IsCorrect)
            //.ToList();
            //.ToDictionary(id => id, id => _questionRepository.GetByIdAsyncNoTracking(id).Result.Answers);
        
        var fiveMinuteResult = CheckFiveMinuteResult(testResult);
        
        var currentUser =  await _userManager.GetUserAsync(User);
 
        fiveMinuteResult.UserId = currentUser?.Id;
        var a = _fiveMinuteResultsRepository.Add(fiveMinuteResult);
        context.SaveChangesAsync();
        return Json("success");
    }

    public UserAnswer CheckUserAnswer(UserAnswerViewModel userAnswer, FiveMinuteTemplate fiveMinuteTemplate)
    {
        var question = fiveMinuteTemplate.Questions.FirstOrDefault(q => q.Position == userAnswer.QuestionPosition);
        var dbAnswer = question?.Answers.FirstOrDefault(x => x.Position == userAnswer.Position);
        if (dbAnswer == null)
        {
            var lalala = "lalala";
            // throw new Exception("Че то не то, нет такого вопроса ептыть");
        }
        return new UserAnswer
        {
            QuestionId = question.Id,
            Text = userAnswer.Text ?? "",
            IsCorrect = dbAnswer?.IsCorrect ?? userAnswer.Text == dbAnswer?.Text,
            QuestionPosition = userAnswer.QuestionPosition,
            QuestionText = question?.QuestionText ?? "",
        };
    }

    public FiveMinuteResult CheckFiveMinuteResult(TestResultViewModel testResult)
    {
        var fmt = fiveMinuteTemplateRepository.GetByIdAsync(testResult.FMTId).Result;
        return new FiveMinuteResult
        {
            Answers = testResult.UserAnswers.Select(ans => CheckUserAnswer(ans, fmt)).ToList(),
            UserName = "User",
            FiveMinuteTemplateId = testResult.FMTId,
            PassTime = DateTime.UtcNow,
        };
    }
}