using FiveMinutes.Models;

namespace FiveMinutes.ViewModels;

public class QuestionViewModel
{
    public int Id { get; set; }
    public int Position { get; set; }
    
    public string QuestionText { get; set; }
    public IEnumerable<AnswerViewModel> Answers { get; set; }
    public ResponseType ResponseType { get; set; }
}