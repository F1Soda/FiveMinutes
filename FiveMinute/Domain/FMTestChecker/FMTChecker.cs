using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository.FMTestRepository;
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

		if (testResult.Answers.All(x => x.ResultStatus == ResultStatus.Verified))	
			testResult.Status =  ResultStatus.Verified;
		testResult.UserData = testResultViewModel.UserData.GetCopy();
		testResult.UserId = testResultViewModel.UserId;
		
		if (testResultViewModel.UserId != "")
		{
			var currentUser = await userRepository.GetFullUserDataById(testResultViewModel.UserId);
			if (currentUser != null)
				currentUser.AddResult(testResult);
		}
		if (!await fiveMinuteTestRepository.AddResultToTest(testResultViewModel.FMTestId, testResult))
			return false;

		await userRepository.Save();
		return true;
	}

	public UserAnswer CheckUserAnswer(UserAnswerViewModel userAnswer, FiveMinuteTemplate fiveMinuteTemplate)
	{
		var question = fiveMinuteTemplate.Questions.FirstOrDefault(q => q.Id == userAnswer.QuestionId);
		var dbAnswer = question?.AnswerOptions.FirstOrDefault(x => x.Position == userAnswer.Position);

		var rez = UserAnswerViewModel.CreateByView(userAnswer);
		rez.QuestionId = question.Id;
		rez.IsCorrect = (dbAnswer?.IsCorrect ?? false) && rez.Text == dbAnswer?.Text;
		rez.ResultStatus = question.ResponseType == ResponseType.Text 
			? ResultStatus.Accepted 
			: ResultStatus.Verified;
		rez.QuestionText = question?.QuestionText ?? "";
		if (rez.IsCorrect)
		{
			var count = question.AnswerOptions.Count(x => x.IsCorrect);
			rez.Score = question.QuestionScore / count;
		}
		return rez;
	}

	public async Task<FiveMinuteTestResult> ConvertViewModelToFiveMinuteResult(TestResultViewModel testResult)
	{
		var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testResult.FMTestId);
		var rez = TestResultViewModel.CreateByView(testResult);
		rez.Answers = testResult.UserAnswers.Select(ans => CheckUserAnswer(ans, fmTest.FiveMinuteTemplate)).ToList();
		rez = await UpdateScore(rez);
		return rez;
	}

	public async Task<FiveMinuteTestResult> UpdateScore(FiveMinuteTestResult testResult)
	{
		testResult.Score = testResult.Answers.Sum(answer=>answer.Score);
		return testResult;
	}
}