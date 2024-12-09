using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class TestResultViewModel:IOutput<TestResultViewModel,FiveMinuteTestResult>
{
    public int FMTestId { get; set; }
	public string UserId { get; set; }
	public string UserName { get; set; }
	public UserData StudentData { get; set; }
    public IEnumerable<UserAnswerViewModel> UserAnswers { get; set; }
    public static FiveMinuteTestResult CreateByView(TestResultViewModel model)
    {
	    return new FiveMinuteTestResult
	    {
		    FiveMinuteTestId = model.FMTestId,
		    PassTime = DateTime.UtcNow,
		    // Тут нужна логика, чтобы обрабатывать, сразу ли ответы проверены или ещё что то сам препод долен чекнуть
		    Status = ResultStatus.Accepted,
		    UserId = model.UserId,
		    StudentData = model.StudentData??new UserData
		    {
			    FirstName = "UNKNOWN",
			    LastName = "UNKNOWN",
			    Group = "UNKNOWN",
		    },
	    };
    }
}