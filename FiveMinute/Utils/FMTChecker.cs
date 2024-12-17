using FiveMinute.Data;
using FiveMinute.Interfaces;
using FiveMinute.Models;
using FiveMinute.Repository.FiveMinuteTestRepository;
using FiveMinute.ViewModels;

namespace FiveMinute.Utils;

public class FmtChecker(
    IFiveMinuteResultsRepository resultsRepository,
    IFiveMinuteTestRepository fiveMinuteTestRepository)
    : IChecker
{
    public async Task<bool> CheckAndSave(AppUser? currentUser, TestResultViewModel testResultViewModel)
    {
        var testResult = await ConvertViewModelToFiveMinuteResult(testResultViewModel);
        
        testResult.UserId = currentUser?.Id;
        testResult.UserName = currentUser?.UserName;

        if (!await fiveMinuteTestRepository.AddResultToTest(testResultViewModel.FMTestId, testResult))
            return false;
        currentUser?.AddResult(testResult);

        return true;
    }
    
    public UserAnswer CheckUserAnswer(UserAnswerViewModel userAnswer, FiveMinuteTemplate fiveMinuteTemplate)
    {
        var question = fiveMinuteTemplate.Questions.FirstOrDefault(q => q.Position == userAnswer.QuestionPosition);
        var dbAnswer = question?.AnswerOptions.FirstOrDefault(x => x.Position == userAnswer.Position);
        if (dbAnswer == null)
        {
            throw new Exception($"Вопрос ,на который указывает ответ юзера {userAnswer} не существует в БД");
        }

        var rez =UserAnswerViewModel.CreateByView(userAnswer);
        rez.QuestionId = question.Id;
        rez.IsCorrect = (dbAnswer?.IsCorrect ?? false) && rez.Text == dbAnswer?.Text;
        rez.QuestionText = question?.QuestionText ?? "";
        return rez;
    }

    public async Task<FiveMinuteTestResult> ConvertViewModelToFiveMinuteResult(TestResultViewModel testResult)
    {
        var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testResult.FMTestId);
        var rez=TestResultViewModel.CreateByView(testResult);
        rez.Answers = testResult.UserAnswers.Select(ans => CheckUserAnswer(ans, fmTest.FiveMinuteTemplate)).ToList();
        return rez;
    }
}