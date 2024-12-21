using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class AnswerViewModel: IInput<AnswerViewModel,Answer>
{
	public int Id { get; set; }
	public int Position { get; set; }

	public string Text { get; set; }
	public int QuestionId { get; set; }
	public static AnswerViewModel CreateByModel(Answer model)
	{
		return new AnswerViewModel()
		{
			Id = model.Id,
			QuestionId = model.QuestionId,
			Position = model.Position,
			Text = model.Text,
		};
	}
}