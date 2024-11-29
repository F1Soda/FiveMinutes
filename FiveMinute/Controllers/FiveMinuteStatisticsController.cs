using System.Net;
using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository;
using FiveMinute.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers;

public class FiveMinuteStatisticsController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IFiveMinuteResultsRepository _fiveMinuteResultsRepository;
    private readonly FiveMinuteTemplateRepository _fiveMinuteTemplateRepository;
    private readonly UserManager<AppUser> _userManager;

    public FiveMinuteStatisticsController(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        this.context = context;
        _userManager = userManager;
        _fiveMinuteResultsRepository = new FiveMinuteResultRepository(context);
        _fiveMinuteTemplateRepository = new FiveMinuteTemplateRepository(context);
    }

    public async Task<IActionResult> ShowResults(int fiveMinuteId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var fmt = await _fiveMinuteTemplateRepository.GetByIdAsync(fiveMinuteId);
        if (fmt == null || currentUser is null || fmt.UserOwnerId != currentUser.Id)
            return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));
        
        var results = _fiveMinuteResultsRepository.GetByFMTIdAsync(fiveMinuteId).Result;
        var fiveMinuteResults = new FiveMinuteResultsViewModel()
        {
            Results = results,
        };
        return View(fiveMinuteResults);

    }

    public async Task<IActionResult> FiveMinuteResult(int resultId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var result = _fiveMinuteResultsRepository.GetById(resultId).Result;

        if (result is null || currentUser is null || result.FiveMinuteTemplate.UserOwnerId != currentUser.Id)
            return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));

        return View(result);
    }

}