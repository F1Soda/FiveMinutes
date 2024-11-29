using FiveMinutes.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinute.Models
{
	public class FiveMinuteTemplate
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }

		public DateTime? CreationTime { get; set; }
		public DateTime? LastModificationTime { get; set; }
		public IEnumerable<Question> Questions { get; set; }

		[ForeignKey("FiveMinuteTemplate")]
		public int? OriginId { get; set; }
		public FiveMinuteTemplate? Origin { get; set; }

		public bool ShowInProfile { get; set; }

		[ForeignKey("AppUser")]
		public string? UserOwnerId { get; set; }
		public AppUser? UserOwner { get; set; }

		public FiveMinuteTemplate GetCopyToUser(AppUser newUserOwner)
		{
			return new FiveMinuteTemplate
			{
				Name = $"{Name} (копия)",
				CreationTime = DateTime.UtcNow,
				LastModificationTime = DateTime.UtcNow,
				ShowInProfile = ShowInProfile,
				UserOwner = newUserOwner,
				UserOwnerId = newUserOwner.Id,
				Questions = Questions,
				Origin = this,
				OriginId = OriginId

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
						//ResponseType = Models.ResponseType.SingleChoice,
					}
				},
				ShowInProfile = true,
			};
			fmt.Name = $"Новая пятиминутка ({fmt.Id})";

			return fmt;
		}

	}
}
