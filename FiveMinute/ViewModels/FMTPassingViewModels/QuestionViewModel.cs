using FiveMinutes.Models;

namespace FiveMinutes.ViewModels;

public class QuestionViewModel
{
    public int Id { get; set; }
    public int Position { get; set; }

    public string QuestionText { get; set; }
    public ICollection<AnswerViewModel> AnswerOptions { get; set; }
    public ResponseType ResponseType { get; set; }
}