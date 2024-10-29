using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace FiveMinutes.Controllers;

public class TestPassingController : Controller
{
    public readonly ApplicationDbContext context;
    private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;

    public TestPassingController(ApplicationDbContext context)
    {
        this.context = context;
        this.fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
    }
    public IActionResult Test(int id)
    {
        var fmt = fiveMinuteTemplateRepository.GetByIdAsync(id).Result;
        var test = new FiveMinuteViewModel
        {
            Name = fmt.Name,
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

    public string SendTestResults(FiveMinuteUserAnswers fiveMinuteUserAnswers)
    {
        throw new NotImplementedException();
    }
}