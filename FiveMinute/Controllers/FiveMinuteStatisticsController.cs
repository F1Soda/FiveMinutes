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
        var results = _fiveMinuteResultsRepository.GetByIdAsync(fiveMinuteId).Result;
        var fiveMinuteResults = new FiveMinuteResultsViewModel()
        {
            Results = results,
        };
        return View(fiveMinuteResults);

    }

}