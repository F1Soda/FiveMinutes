﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinutes.Models
{
	public class FiveMinuteTemplate
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public DateTime? CreationTime { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public IEnumerable<Question> Questions { get; set; }

		public FiveMinuteTemplate origin { get; set; }

        public bool ShowInProfile { get; set; }

        [ForeignKey("AppUser")]
        public string? UserOwnerId { get; set; }
        public AppUser? UserOwner { get; set; }

        public FiveMinuteTemplate GetCopy()
        {
	        return new FiveMinuteTemplate
	        {
		        Name = $"{this.Name} (копия)",
		        CreationTime = this.CreationTime,
		        LastModificationTime = this.LastModificationTime,
		        ShowInProfile = this.ShowInProfile,
		        UserOwner = this.UserOwner,
		        UserOwnerId = this.UserOwnerId,
		        Questions = this.Questions
	        };
        }

		public static FiveMinuteTemplate CreateDefault(AppUser user)
		{
			var fmt = new FiveMinuteTemplate
			{
				CreationTime = DateTime.UtcNow,
				LastModificationTime = DateTime.UtcNow,
				UserOwnerId = user.Id,
				UserOwner = user,
				Questions = new List<Question>()
				{
					new Question
					{
						QuestionText = "Вопрос 1",
						Position = 0,
						ResponseType = Models.ResponseType.SingleChoice,
					}
				},
				ShowInProfile = true,
			};
			fmt.Name = $"Новая пятиминутка ({fmt.Id})";

			return fmt;
		}

	}
}
