using System.Net;
using FiveMinute.Models;
using FiveMinute.Repository.FiveMinuteTemplateRepository;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels;
using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiveMinute.Controllers;

//public class FiveMinuteStatisticsController : Controller
//{
//    private readonly IFiveMinuteTemplateRepository fiveMinuteTemplateRepository;
//    private readonly IFiveMinuteTestRepository fiveMinuteTestRepository;
//    private readonly UserManager<AppUser> userManager;

//    public FiveMinuteStatisticsController(UserManager<AppUser> userManager,
//        FiveMinuteTemplateRepository fmTemplateRepository,
//        FiveMinuteTestRepository fmTestRepository)
//    {
//        this.userManager = userManager;
//        fiveMinuteTemplateRepository = fmTemplateRepository;
//        fiveMinuteTestRepository = fmTestRepository;
//    }

//    public async Task<IActionResult> ShowResults(int fiveMinuteId)
//    {
//        var currentUser = await userManager.GetUserAsync(User);
//        var fmt = await fiveMinuteTemplateRepository.GetByIdAsync(fiveMinuteId);
//        if (fmt == null || currentUser is null || fmt.UserOwnerId != currentUser.Id)
//            return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));
        
//        var results = _fiveMinuteResultsRepository.GetByFMTIdAsync(fiveMinuteId).Result;
//        var fiveMinuteResults = new FiveMinuteResultsViewModel()
//        {
//            Results = results,
//        };
//        return View(fiveMinuteResults);

//    }

//    public async Task<IActionResult> FiveMinuteResult(int resultId)
//    {
//        var currentUser = await userManager.GetUserAsync(User);
//        var result = _fiveMinuteResultsRepository.GetById(resultId).Result;

//        if (result is null || currentUser is null || result.FiveMinuteTemplate.UserOwnerId != currentUser.Id)
//            return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));

//        return View(result);
//    }

//}