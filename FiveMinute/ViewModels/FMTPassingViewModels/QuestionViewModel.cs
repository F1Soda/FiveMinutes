using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class QuestionViewModel: IInput<QuestionViewModel,Question>
{
    public int Id { get; set; }
    public int Position { get; set; }

    public string QuestionText { get; set; }
    public ICollection<AnswerViewModel> AnswerOptions { get; set; }
    public ResponseType ResponseType { get; set; }
    public static QuestionViewModel CreateByModel(Question model)
    {
        return new QuestionViewModel
        {
            Id = model.Id,
            Position = model.Position,
            QuestionText = model.QuestionText,
            ResponseType = model.ResponseType,
            AnswerOptions = model.AnswerOptions.Select(answer => AnswerViewModel.CreateByModel(answer)).ToList(),
        };
    }
}