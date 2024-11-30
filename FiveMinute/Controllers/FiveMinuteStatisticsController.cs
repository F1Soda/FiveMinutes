using System.Net;
using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers;

public class FiveMinuteStatisticsController : Controller
{
    private readonly ApplicationDbContext context;
    private readonly IFiveMinuteResultsRepository _fiveMinuteResultsRepository;
    private readonly FiveMinuteTestRepository _fiveMinuteTestRepository;
    private readonly UserManager<AppUser> _userManager;

    public FiveMinuteStatisticsController(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        this.context = context;
        _userManager = userManager;
        _fiveMinuteResultsRepository = new FiveMinuteResultRepository(context);
        _fiveMinuteTestRepository = new FiveMinuteTestRepository(context);
    }

    public async Task<IActionResult> ShowResults(int testId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var test = await _fiveMinuteTestRepository.GetByIdAsync(testId);
        if (test == null || currentUser is null || test.UserOrganizerId != currentUser.Id)
            return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));
        
        var results = _fiveMinuteResultsRepository.GetByTestIdAsync(testId).Result;
        var fiveMinuteResults = new FiveMinuteResultsViewModel
        {
            Results = results,
        };
        return View(fiveMinuteResults);
    }

    public async Task<IActionResult> FiveMinuteResult(int resultId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var result = _fiveMinuteResultsRepository.GetById(resultId).Result;
        var FMTest = _fiveMinuteTestRepository.GetByIdAsync(result.FiveMinuteTestId).Result;
        var fiveMinuteTestResultViewModel = new FiveMinuteTestResultViewModel
        {
            FiveMinuteTestName = FMTest.Name,
            FiveMinuteTestResult = result,
            Questions = FMTest.FiveMinuteTemplate.Questions.ToList()
        };

        if (result is null || currentUser is null || FMTest.UserOrganizerId != currentUser.Id)
            return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));

        return View(fiveMinuteTestResultViewModel);
    }
}