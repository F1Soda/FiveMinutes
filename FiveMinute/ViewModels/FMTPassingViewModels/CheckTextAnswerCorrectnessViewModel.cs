using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class CheckTextAnswerCorrectnessViewModel : IOutput<CheckTextAnswerCorrectnessViewModel, UserAnswer>
{
    public UserAnswerViewModel UserAnswerViewModel;
    public bool IsCorrect;
    //public int TestId;
    
    public static UserAnswer CreateByView(CheckTextAnswerCorrectnessViewModel model)
    {
        return new UserAnswer
        {
            Text = model.UserAnswerViewModel.Text ?? "",
            Position = model.UserAnswerViewModel.Position,
            QuestionPosition = model.UserAnswerViewModel.QuestionPosition,
            IsCorrect = model.IsCorrect
        };
    }
}