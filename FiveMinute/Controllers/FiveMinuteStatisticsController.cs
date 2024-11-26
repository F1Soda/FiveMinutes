using FiveMinutes.Data;
using FiveMinutes.Interfaces;
using FiveMinutes.Models;
using FiveMinutes.Repository;
using FiveMinutes.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinutes.Controllers;

public class FiveMinuteStatisticsController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IFiveMinuteResultsRepository _fiveMinuteResultsRepository;

    public FiveMinuteStatisticsController(ApplicationDbContext context)
    {
        this.context = context;
        _fiveMinuteResultsRepository = new FiveMinuteResultRepository(context);
    }

    public IActionResult ShowResults(int fiveMinuteId)
    {
        var results = _fiveMinuteResultsRepository.GetByFMTIdAsync(fiveMinuteId).Result;
        var fiveMinuteResults = new FiveMinuteResultsViewModel()
        {
            Results = results,
        };
        return View(fiveMinuteResults);

    }

    public IActionResult FiveMinuteResult(int resultId)
    {
        var result = _fiveMinuteResultsRepository.GetById(resultId).Result;
        // var a = new FiveMinuteResultViewModel
        // {
        //     Id = result.Id,
        //     Answers = result.Answers,
        //     FiveMinuteTemplate = result.FiveMinuteTemplate,
        //     FiveMinuteTemplateId = result.FiveMinuteTemplateId,
        //     UserId = result.UserId,
        //     UserName = result.UserName,
        //     PassTime = result.PassTime,
        // };
        return View(result);
    }

}