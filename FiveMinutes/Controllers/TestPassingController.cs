using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace FiveMinutes.Controllers;

public class TestPassingController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IFiveMinuteResultsRepository _fiveMinuteResultsRepository;

    public TestPassingController(ApplicationDbContext context)
    {
        this.context = context;
        this.fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
        _questionRepository = new QuestionRepository(context);
        _fiveMinuteResultsRepository = new FiveMinuteResultRepository(context);
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
                Answers = x.Answers.Select(answer => new AnswerViewModel()
                {
                    Id = answer.Id,
                    QuestionId = answer.QuestionId,
                    Position = answer.Position,
                    Text = answer.Text,
                })
            }),
        };
        return View(test);
    }

    [HttpPost]
    public string SendTestResults(Dictionary<int, string[]> userAnswers, int fiveMinuteId)
    {
        // TODO: По хорошему нужно создать в форме поле для имени, если чел не зареган
            //.SelectMany(question => question.Answers)
            //.Where(answer => answer.IsCorrect)
            //.ToList();
            //.ToDictionary(id => id, id => _questionRepository.GetByIdAsyncNoTracking(id).Result.Answers);
        
        var answers = userAnswers.Keys
            .SelectMany(questionId => userAnswers[questionId]
                .Select(answerText => new UserAnswer
                {
                    // TODO: Тут хуйня, переделать
                    IsCorrect = correctAnswers
                        .Any(correctAnswer => correctAnswer.ToString() == answerText),
                    QuestionId = questionId,
                    Text = answerText,
                }))
            .ToList();
        
        var fiveMinuteResult = new FiveMinuteResult()
        {
            Answers = answers,
            PassTime = DateTime.UtcNow,
            // TODO: запоминать айди пользователя и пятиминутки
            UserId = -1,
            UserName = "User",
            FiveMinuteTemplateId = fiveMinuteId,
        };

        _fiveMinuteResultsRepository.Add(fiveMinuteResult);
        context.SaveChanges();
        return "succes";
    }
}