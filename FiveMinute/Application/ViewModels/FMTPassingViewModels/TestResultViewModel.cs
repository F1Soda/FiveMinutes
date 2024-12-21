using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class TestResultViewModel:IOutput<TestResultViewModel, FiveMinuteTestResult>
{
    public int FMTestId { get; set; }
	public string UserId { get; set; }
	public UserData UserData { get; set; }
    public IEnumerable<UserAnswerViewModel> UserAnswers { get; set; }

    public static FiveMinuteTestResult CreateByView(TestResultViewModel model)
    {
	    return new FiveMinuteTestResult
	    {
		    FiveMinuteTestId = model.FMTestId,
		    PassTime = DateTime.UtcNow,
		    Status = ResultStatus.Accepted,
		    UserId = model.UserId,
		    UserData = model.UserData.GetCopy()
	    };
    }
}