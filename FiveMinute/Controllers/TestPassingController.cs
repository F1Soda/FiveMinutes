using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository;
using FiveMinute.Repository.FiveMinuteTestRepository;
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
        var rez =UserAnswerViewModel.CreateByView(userAnswer);
        rez.QuestionId = question.Id;
        rez.IsCorrect = (dbAnswer?.IsCorrect ?? false) && rez.Text == dbAnswer?.Text;
        rez.QuestionText = question?.QuestionText ?? "";
        return rez;
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