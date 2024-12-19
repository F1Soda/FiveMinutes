using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FMResultViewModels
{
	public class FMTResultForIndexHomeViewModel : IInput<FMTResultForIndexHomeViewModel, FiveMinuteTestResult>
	{
		public int Id { get; set; }
		public int FiveMinuteTestId { get; set; }
		public float Result { get; set; }
		public string FMTestName {  get; set; }
		public string FMTestOrganizer {  get; set; }
		public DateTime PassTime { get; set; }

		public static FMTResultForIndexHomeViewModel CreateByModel(FiveMinuteTestResult model)
		{
			return new FMTResultForIndexHomeViewModel
			{
				Id = model.Id,
				FiveMinuteTestId = model.FiveMinuteTestId,
				Result = 0,
				FMTestName = "Empty",
				PassTime = model.PassTime,
				FMTestOrganizer = "model.UserName",
			};
		}
	}
}
