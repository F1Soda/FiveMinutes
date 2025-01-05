using FiveMinute.Data;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;

namespace FiveMinute.Application.ViewModels.HomeViewModels
{
	public class CardRowTestInfoViewModel
	{
		public ICollection<FMTestIndexViewModel> ActiveFMTests;
		public ICollection<FMTestIndexViewModel> RequiresRecheckingFMTests;
		public ICollection<FMTestIndexViewModel> PlannedFMTests;
	
		public CardRowTestInfoViewModel(ICollection<FMTestIndexViewModel> activeFMTests,
			ICollection<FMTestIndexViewModel> requiresRecheckingFMTests,
			 ICollection<FMTestIndexViewModel> plannedFMTests)
		{
			ActiveFMTests = activeFMTests;
			RequiresRecheckingFMTests = requiresRecheckingFMTests;
			PlannedFMTests = plannedFMTests;
		}

		public CardRowTestInfoViewModel(List<FMTestIndexViewModel> tests)
		{
			ActiveFMTests = tests.Where(x => x.Status == TestStatus.Open).ToList();
			RequiresRecheckingFMTests = tests.Where(x => x.HasUncheckedAnswers).ToList();
			PlannedFMTests = tests.Where(x => x.EndPlanned || x.StartPlanned).ToList();
		}
	}
}
