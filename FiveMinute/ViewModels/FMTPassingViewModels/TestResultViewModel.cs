using FiveMinutes.Models;

namespace FiveMinutes.ViewModels;

public class TestResultViewModel
{
    public int FMTId { get; set; }
    public IEnumerable<UserAnswerViewModel> UserAnswers { get; set; }
}