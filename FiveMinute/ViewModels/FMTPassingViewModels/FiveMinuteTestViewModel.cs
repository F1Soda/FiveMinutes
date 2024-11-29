using FiveMinute.Models;

namespace FiveMinute.ViewModels;

public class FiveMinuteTestViewModel
{
    public string Name { get; set; }
    public int FMTestId;

    public IEnumerable<QuestionViewModel> Questions { get; set; }
}