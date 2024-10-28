namespace FiveMinutes.ViewModels
{
	public class FiveMinuteTemplateEditViewModel
	{
		public string Name { get; set; }
		public List<QuestionEditViewModel> Questions { get; set; }
		public bool ShowInProfile = true;
	}
}
