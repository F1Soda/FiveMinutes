﻿namespace FiveMinutes.ViewModels
{
	public class FiveMinuteTemplateEditViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public IEnumerable<QuestionEditViewModel> Questions { get; set; }
		public bool ShowInProfile = true;
	}
}
