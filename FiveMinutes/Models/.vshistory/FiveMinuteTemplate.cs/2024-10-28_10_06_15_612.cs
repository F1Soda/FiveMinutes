using FiveMinutes.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FiveMinutes.Models
{
	public class FiveMinuteTemplate
	{
		[Key]
		public string Name { get; set; }
		public ICollection<QuestionEditViewModel> Questions { get; set; }
        public bool ShowInProfile = true;
	}
}
