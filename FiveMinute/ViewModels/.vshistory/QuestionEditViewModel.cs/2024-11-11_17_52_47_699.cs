using FiveMinutes.Models;

namespace FiveMinutes.ViewModels
{
	public class QuestionEditViewModel
	{
		public int Position { get; set; }
		public string QuestionText { get; set; }
		public ResponseType ResponseType { get; set; }
		public ICollection<Answer> Answers { get; set; } = new List<Answer>();
	}
}
