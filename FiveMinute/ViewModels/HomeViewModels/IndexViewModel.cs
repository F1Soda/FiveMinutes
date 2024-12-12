using FiveMinute.Models;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.Interfaces;
using FiveMinute.Data;

namespace FiveMinute.ViewModels.HomeViewModels
{
	public class IndexViewModel:IInput<IndexViewModel,AppUser>
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string UserRole { get; set; }
		public ICollection<FMTemplateIndexViewModel> FMTemplates { get; set; }
		public ICollection<FMTestIndexViewModel> FMTests { get; set; }

		public ICollection<FMTestIndexViewModel> ActiveFMTests { get; set; }
		public ICollection<FMTestIndexViewModel> PlannedFMTests { get; set; }
		public ICollection<FMTestIndexViewModel> RequiresRecheckingFMTests { get; set; }

		public static IndexViewModel CreateByModel(AppUser user)
		{
			var rez = new IndexViewModel
			{
				UserName = user.UserName,
				Email = user.Email,
				FMTemplates = user.FMTTemplates.Select(x => FMTemplateIndexViewModel.CreateByModel(x))
					.OrderByDescending(x => x.lastModification).ToList(),
				FMTests = user.FMTests
					.OrderByDescending(x => x.CreationTime)
					.Select(x => FMTestIndexViewModel.CreateByModel(x)).ToList(),
				UserRole = user.UserRole
			};

			rez.ActiveFMTests = rez.FMTests.Where(x => x.Status == TestStatus.Started).ToList();
			rez.RequiresRecheckingFMTests = rez.FMTests.Where(x => x.Status == TestStatus.InRechekingProcess).ToList();
			rez.PlannedFMTests = rez.FMTests.Where(x => x.Status == TestStatus.Planned).ToList();
			return rez;
		}
	}
}
