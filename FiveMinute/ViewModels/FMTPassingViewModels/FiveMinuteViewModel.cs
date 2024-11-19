using FiveMinutes.Models;

namespace FiveMinutes.ViewModels;

public class FiveMinuteViewModel
{
    public string Name { get; set; }
    public int Id;

    public IEnumerable<QuestionViewModel> Questions { get; set; }
}