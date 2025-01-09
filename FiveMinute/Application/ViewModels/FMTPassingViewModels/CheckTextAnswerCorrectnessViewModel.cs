using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class CheckTextAnswerCorrectnessViewModel : IOutput<CheckTextAnswerCorrectnessViewModel, UserAnswer>
{
    public int Position { get; set; }
    public int QuestionId { get; set; }
    public bool IsCorrect { get; set; }
    public int TestId { get; set; }
    public string Text { get; set; }
    public int resultId { get; set; }

    
    public static UserAnswer CreateByView(CheckTextAnswerCorrectnessViewModel model)
    {
        return new UserAnswer
        {
            Position = model.Position,
            IsCorrect = model.IsCorrect,
            Text = model.Text,
            QuestionId = model.QuestionId,
            Id = model.TestId
        };
    }
}