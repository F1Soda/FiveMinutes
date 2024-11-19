namespace FiveMinutes.ViewModels
{
	public class FiveMinuteTemplateEditViewModel
	{
		public string Name { get; set; }
		public IEnumerable<QuestionEditViewModel> Questions { get; set; }
		public bool ShowInProfile = true;
	}
}
