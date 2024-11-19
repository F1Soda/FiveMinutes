using FiveMinutes.Models;

namespace FiveMinutes.ViewModels
{
	public class FiveMinuteTemplateEditViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<QuestionEditViewModel> Questions { get; set; }
		public bool ShowInProfile = true;
		

		public static FiveMinuteTemplateEditViewModel CreateByModel(FiveMinuteTemplate fmt)
		{
            return new FiveMinuteTemplateEditViewModel
            {
                Id = fmt.Id,
                Name = fmt.Name,
                ShowInProfile = fmt.ShowInProfile,
                Questions = fmt.Questions
                    .Select(x => new QuestionEditViewModel()
                    {
                        QuestionText = x.QuestionText,
                        Position = x.Position,
                        ResponseType = ResponseType.SingleChoice,
                        Answers = x.Answers.Select(x => new AnswerEditViewModel
                        {
                            Position = x.Position,
                            Text = x.Text,
                            IsCorrect = x.IsCorrect
                        })
                    })
            };
        }
	}
}
