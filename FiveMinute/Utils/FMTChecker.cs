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
            // throw new Exception($"Вопрос ,на который указывает ответ юзера {userAnswer} не существует в ");
            // Для текстового ответа надо подумать что делать
        }
        return new UserAnswer
        {
            QuestionId = question.Id,
            Text = userAnswer.Text ?? "",
            Position = userAnswer.Position,
            IsCorrect = (dbAnswer?.IsCorrect ?? false) && userAnswer.Text == dbAnswer?.Text,
            QuestionPosition = userAnswer.QuestionPosition,
            QuestionText = question?.QuestionText ?? "",
        };
    }

    public async Task<FiveMinuteTestResult> ConvertViewModelToFiveMinuteResult(TestResultViewModel testResult)
    {
        var fmTest = await fiveMinuteTestRepository.GetByIdAsync(testResult.FMTestId);
        return new FiveMinuteTestResult
        {
            Answers = testResult.UserAnswers.Select(ans => CheckUserAnswer(ans, fmTest.FiveMinuteTemplate)).ToList(),
            FiveMinuteTestId = testResult.FMTestId,
            PassTime = DateTime.UtcNow,
            // Тут нужна логика, чтобы обрабатывать, сразу ли ответы проверены или ещё что то сам препод долен чекнуть
            Status = ResultStatus.Accepted,
            UserId = testResult.UserName,
            StudentData = testResult.StudentData??new UserData
            {
                FirstName = "UNKNOWN",
                LastName = "UNKNOWN",
                Group = "UNKNOWN",
            },
        };
    }
}