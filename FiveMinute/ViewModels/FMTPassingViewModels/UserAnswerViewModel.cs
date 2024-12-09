using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class UserAnswerViewModel:IOutput<UserAnswerViewModel,UserAnswer>
{
    public string Text { get; set; }
    public int Position { get; set; }
    public int QuestionPosition { get; set; }
    public static UserAnswer CreateByView(UserAnswerViewModel model)
    {
        return new UserAnswer
        {
            Text = model.Text ?? "",
            Position = model.Position,
            QuestionPosition = model.QuestionPosition,
        };
    }
}