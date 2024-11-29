using FiveMinute.Models;

namespace FiveMinute.ViewModels;

public class TestResultViewModel
{
    public int FMTId { get; set; }
    public string UserName { get; set; }
    public IEnumerable<UserAnswerViewModel> UserAnswers { get; set; }
}