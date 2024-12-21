using FiveMinutes.Models;

namespace FiveMinutes.ViewModels
{
	public class FiveMinuteTemplateEditViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<QuestionEditViewModel> Questions { get; set; }
		public bool ShowInProfile = true;
		

		public static FiveMinuteResultsViewModel CreateByModel(FiveMinuteTemplate fmt)
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
                        ResponseType = Models.ResponseType.SingleChoice,
                        Answers = x.Answers
                    })
            };
        }
	}
}
