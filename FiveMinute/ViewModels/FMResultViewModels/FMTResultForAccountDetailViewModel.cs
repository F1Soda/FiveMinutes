using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FMResultViewModels
{
	public class FMTResultForAccountDetailViewModel : IInput<FMTResultForAccountDetailViewModel, FiveMinuteTestResult>
	{
		public int Id { get; set; }
		public int FiveMinuteTestId { get; set; }
		public float Result { get; set; }
		public string FMTestName {  get; set; }
		public DateTime PassTime { get; set; }

		public static FMTResultForAccountDetailViewModel CreateByModel(FiveMinuteTestResult model)
		{
			return new FMTResultForAccountDetailViewModel
			{
				Id = model.Id,
				FiveMinuteTestId = model.FiveMinuteTestId,
				Result = 0,
				FMTestName = "ERROR",
				PassTime = model.PassTime
			};
		}
	}
}
