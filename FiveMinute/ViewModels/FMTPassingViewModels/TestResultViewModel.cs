using FiveMinute.Data;
using FiveMinute.Models;

namespace FiveMinute.ViewModels;

public class TestResultViewModel
{
    public int FMTestId { get; set; }
	public string UserId { get; set; }
	public string UserName { get; set; }
	public StudentData StudentData { get; set; }
    public IEnumerable<UserAnswerViewModel> UserAnswers { get; set; }
}