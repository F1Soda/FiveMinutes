using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels;

namespace FiveMinute.Utils;

public class FmtChecker(
	IFiveMinuteResultsRepository resultsRepository,
	IFiveMinuteTestRepository fiveMinuteTestRepository,
	IUserRepository userRepository)
	: IChecker
{
	public async Task<bool> CheckAndSave(TestResultViewModel testResultViewModel)
	{	
		var testResult = await ConvertViewModelToFiveMinuteResult(testResultViewModel);

		testResult.UserData = testResultViewModel.UserData.GetCopy();
		testResult.UserId = testResultViewModel.UserId;
		
		if (testResultViewModel.UserId != "")
		{
			var currentUser = await userRepository.GetUserById(testResultViewModel.UserId);
			currentUser.AddResult(testResult);
		}
		if (!await fiveMinuteTestRepository.AddResultToTest(testResultViewModel.FMTestId, testResult))
			return false;

		await userRepository.Save();
		return true;
	}

	public UserAnswer CheckUserAnswer(UserAnswerViewModel userAnswer, FiveMinuteTemplate fiveMinuteTemplate)
	{
		var question = fiveMinuteTemplate.Questions.FirstOrDefault(q => q.Position == userAnswer.QuestionPosition);
		var dbAnswer = question?.AnswerOptions.FirstOrDefault(x => x.Position == userAnswer.Position);

		var rez = UserAnswerViewModel.CreateByView(userAnswer);
		rez.QuestionId = question.Id;
		rez.IsCorrect = (dbAnswer?.IsCorrect ?? false) && rez.Text == dbAnswer?.Text;
		rez.QuestionText = question?.QuestionText ?? "";
		return rez;
	}

	public async Task<FiveMinuteTestResult> ConvertViewModelToFiveMinuteResult(TestResultViewModel testResult)
	{
		var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testResult.FMTestId);
		var rez = TestResultViewModel.CreateByView(testResult);
		rez.Answers = testResult.UserAnswers.Select(ans => CheckUserAnswer(ans, fmTest.FiveMinuteTemplate)).ToList();
		return rez;
	}
}