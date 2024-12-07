using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository;
using FiveMinute.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FiveMinute.Controllers;

public class TestPassingController(UserManager<AppUser> userManager, ApplicationDbContext context)
    : Controller
{
    private readonly ApplicationDbContext context = context;
    private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
    private readonly IFiveMinuteResultsRepository fiveMinuteResultsRepository = new FiveMinuteResultRepository(context);


    public IActionResult Test(int testId)
    {
        var fmt = fiveMinuteTemplateRepository.GetByIdAsync(testId).Result;
        var test = new FiveMinuteTestViewModel
        {
            Name = fmt.Name,
            FMTestId = fmt.Id,
            Questions = fmt.Questions.Select(x => new QuestionViewModel
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
    public async Task<IActionResult> SendTestResults(TestResultViewModel testResult)
    {
        // TODO: По хорошему нужно создать в форме поле для имени, если чел не зареган
        
        var fiveMinuteResult = ConvertViewModelToFiveMinuteResult(testResult);
        var currentUser =  await userManager.GetUserAsync(User);
 
        fiveMinuteResult.UserId = currentUser?.Id;
        fiveMinuteResult.UserName = testResult.UserName;
        fiveMinuteResultsRepository.Add(fiveMinuteResult);
        return RedirectToAction("Index","Home");
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

    public FiveMinuteTestResult ConvertViewModelToFiveMinuteResult(TestResultViewModel testResult)
    {
        var fmt = fiveMinuteTemplateRepository.GetByIdAsync(testResult.FMTestId).Result;
        return new FiveMinuteTestResult
        {
            Answers = testResult.UserAnswers.Select(ans => CheckUserAnswer(ans, fmt)).ToList(),
            FiveMinuteTestId = testResult.FMTestId,
            PassTime = DateTime.UtcNow,
        };
    }
}