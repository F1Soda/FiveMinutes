using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FiveMinute.ViewModels;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Utils;
using System.Net;

namespace FiveMinute.Controllers
{
    public class FiveMinuteTestController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IFiveMinuteTestRepository _fiveMinuteTestRepository;
        private readonly IFiveMinuteResultsRepository _fiveMinuteResultsRepository;
        private readonly IChecker _fmtChecker;

        public FiveMinuteTestController(
            UserManager<AppUser> userManager,
            IUserRepository userRepository,
            IFiveMinuteTestRepository fiveMinuteTestRepository,
            IFiveMinuteResultsRepository fiveMinuteResultsRepository,
            IChecker fmtChecker)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _fiveMinuteTestRepository = fiveMinuteTestRepository;
            _fiveMinuteResultsRepository = fiveMinuteResultsRepository;
            _fmtChecker = fmtChecker;
        }

        public async Task<IActionResult> Edit(int testId)
        {
            var (currentUser, fmTest) = await GetCurrentUserAndTest(testId);
            if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

            if (fmTest == null) return NotFoundError();

            var viewModel = FiveMinuteTestEditViewModel.CreateByModel(fmTest);
            ViewData["TemplateId"] = fmTest.Id;
            return View(viewModel);
        }

        public IActionResult Passed() => View();

        [HttpPost]
        public async Task<IActionResult> Edit(FiveMinuteTestEditViewModel viewModel)
        {
            var (currentUser, existingFMTest) = await GetCurrentUserAndTest(viewModel.Id);
            if (currentUser == null) return UnauthorizedAccessError();

            var updatedTest = FiveMinuteTestEditViewModel.CreateByView(viewModel);
            updatedTest.Status = existingFMTest.Status;
            updatedTest.Results = existingFMTest.Results;

            await _fiveMinuteTestRepository.Update(updatedTest);
            return RedirectToAction("Detail", new { id = existingFMTest.Id });
        }

        public async Task<IActionResult> Detail(int testId)
        {
            var (currentUser, fmTest) = await GetCurrentUserAndTest(testId);
            if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

            if (fmTest == null) return NotFoundError();

            return View(FiveMinuteTestDetailViewModel.CreateByModel(fmTest));
        }

        public async Task<IActionResult> Create(int templateId)
        {
            var currentUser = await GetCurrentUser();
            if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

            ViewData["TemplateId"] = templateId;
            var user = await _userRepository.GetFullUserDataById(currentUser.Id);

            return View(user.FMTemplates);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FiveMinuteTestDetailViewModel viewModel)
        {
            var currentUser = await GetCurrentUser();
            if (currentUser == null || !currentUser.canCreate) return UnauthorizedAccessError();

            var user = await _userRepository.GetFullUserDataById(currentUser.Id);
            var attachedTemplate = user.FMTemplates.FirstOrDefault(x => x.Id == viewModel.AttachedFMTId);

            var newTest = CreateNewTest(viewModel, user, attachedTemplate);
            await _fiveMinuteTestRepository.Add(newTest);
            return RedirectToAction("Detail", new { testId = newTest.Id });
        }

        public async Task<IActionResult> Pass(string encryptedId)
        {
            var testId = UrlEncryptor.Decrypt(encryptedId);
            var fmTest = await _fiveMinuteTestRepository.GetByIdAsync(testId);
            if (fmTest == null) return NotFoundError();

            var currentUser = await _userManager.GetUserAsync(User);
            if (!fmTest.CanPass(currentUser)) return Forbid();

            var viewModel = FMTestPassingViewModel.CreateByModel(fmTest);
            if (currentUser != null && User.Identity.IsAuthenticated)
            {
                viewModel.UserData = currentUser.UserData;
                viewModel.userId = currentUser.Id;
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendTestResults(TestResultViewModel viewModel)
        {
            if (!await _fmtChecker.CheckAndSave(viewModel))
                return View("Error", new ErrorViewModel("Something is wrong. Could not save your answers"));

            return RedirectToAction("Passed");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTestSettings(FiveMinuteTestDetailViewModel viewModel)
        {
            var (currentUser, existingFMTest) = await GetCurrentUserAndTest(viewModel.Id);
            if (currentUser == null) return UnauthorizedAccessError();

            if (existingFMTest == null) return NotFoundError();

            var updatedTest = UpdateTestFromViewModel(viewModel, existingFMTest);
            if (!await _fiveMinuteTestRepository.Update(updatedTest)) return View("Error");

            return RedirectToAction("Detail", new { testId = updatedTest.Id });
        }

        public async Task<IActionResult> FiveMinuteResult(int resultId)
        {
            var currentUser = await GetCurrentUser();
            var result = await _fiveMinuteResultsRepository.GetById(resultId);
            var fmTest = await _fiveMinuteTestRepository.GetByIdAsync(result?.FiveMinuteTestId ?? 0);

            if (result == null || currentUser == null ||
                (fmTest?.UserOrganizerId != currentUser.Id && result.UserId != currentUser.Id))
            {
                return View("Error", new ErrorViewModel(HttpStatusCode.NotFound.ToString()));
            }

            var viewModel = FiveMinuteTestResultViewModel.CreateByModel(fmTest);
            viewModel.FiveMinuteTestResult = result;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAnswerCorrectness([FromBody] CheckTextAnswerCorrectnessViewModel model)
        {
            var test = await _fiveMinuteTestRepository.GetByIdAsync(model.TestId);
            var answers = test?.Results
                              .SelectMany(r => r.Answers)
                              .Where(a => a.Text == model.Text);

            if (answers != null)
            {
                foreach (var answer in answers)
                {
                    answer.IsCorrect = model.IsCorrect;
                }

                await _fiveMinuteTestRepository.Save();
            }

            return Json(new { success = true });
        }

        // Helper Methods
        private async Task<AppUser> GetCurrentUser()
        {
            return await _userManager.GetUserAsync(User);
        }

        private async Task<(AppUser currentUser, FiveMinuteTest fmTest)> GetCurrentUserAndTest(int testId)
        {
            var currentUser = await GetCurrentUser();
            var fmTest = await _fiveMinuteTestRepository.GetByIdAsync(testId);
            return (currentUser, fmTest);
        }

        private IActionResult UnauthorizedAccessError()
        {
            return View("Error", new ErrorViewModel("You don't have the rights to this action"));
        }

        private IActionResult NotFoundError()
        {
            return View("NotFound");
        }

        private FiveMinuteTest CreateNewTest(FiveMinuteTestDetailViewModel viewModel, AppUser user, FiveMinuteTemplate attachedTemplate)
        {
            return new FiveMinuteTest
            {
                Status = Data.TestStatus.Started,
                IdToUninclude = new List<int>(),
                UserOrganizerId = user.Id,
                UserOrganizer = user,
                FiveMinuteTemplate = attachedTemplate,
                FiveMinuteTemplateId = viewModel.AttachedFMTId,
                Results = new List<FiveMinuteTestResult>(),
                CreationTime = DateTime.UtcNow
            };
        }

        private FiveMinuteTest UpdateTestFromViewModel(FiveMinuteTestDetailViewModel viewModel, FiveMinuteTest existingFMTest)
        {
            var updatedTest = FiveMinuteTestDetailViewModel.CreateByView(viewModel);
            updatedTest.FiveMinuteTemplate = existingFMTest.FiveMinuteTemplate;
            updatedTest.FiveMinuteTemplateId = existingFMTest.FiveMinuteTemplate.Id;
            updatedTest.Results = existingFMTest.Results;
            return updatedTest;
        }
    }
}
