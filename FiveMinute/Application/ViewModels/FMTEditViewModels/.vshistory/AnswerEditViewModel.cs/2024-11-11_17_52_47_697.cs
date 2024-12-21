using System.ComponentModel.DataAnnotations;

namespace FiveMinutes.ViewModels
{
	public class AnswerEditViewModel
	{
		public int Position { get; set; }
		[Required(ErrorMessage = "Текст ответа обязателен")]
		public string Text { get; set; }
		public bool IsCorrect { get; set; }
	}
}
