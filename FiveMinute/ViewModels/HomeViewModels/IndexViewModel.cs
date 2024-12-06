using FiveMinute.Models;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;

namespace FiveMinute.ViewModels.HomeViewModels
{
	public class IndexViewModel
	{
		public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string UserRole { get; set; }
		public ICollection<FMTemplateIndexViewModel> FMTemplates { get; set; }
		public ICollection<FMTestIndexViewModel> FMTests { get; set; }
	}
}
