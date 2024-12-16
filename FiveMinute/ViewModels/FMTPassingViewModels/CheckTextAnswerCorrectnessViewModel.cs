using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class CheckTextAnswerCorrectnessViewModel : IOutput<CheckTextAnswerCorrectnessViewModel, UserAnswer>
{
    public int Position { get; set; }
    public bool IsCorrect { get; set; }
    public int TestId { get; set; }

    
    public static UserAnswer CreateByView(CheckTextAnswerCorrectnessViewModel model)
    {
        return new UserAnswer
        {
            Position = model.Position,
            IsCorrect = model.IsCorrect
        };
    }
}