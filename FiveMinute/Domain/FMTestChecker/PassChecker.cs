using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;

namespace FiveMinute.Domain.FMTestChecker
{
	public static class PassChecker
	{
		public static bool CanPass(FiveMinuteTest FMTest)
		{
			var currentTime = DateTime.UtcNow.ToUniversalTime();
			var tooEarly = FMTest.StartPlanned && (currentTime < FMTest.StartTime);
			var tooLate = FMTest.EndPlanned && currentTime > FMTest.EndTime;
			if ((tooEarly || tooLate || FMTest.Status == TestStatus.Closed))
				return false;
			return true;
		}


		public static bool CanPass(FiveMinuteTestDetailViewModel FMTest)
		{
			var currentTime = DateTime.UtcNow.ToUniversalTime();
			var tooEarly = FMTest.StartPlanned && (currentTime < FMTest.StartTime);
			var tooLate = FMTest.EndPlanned && currentTime > FMTest.EndTime;
			if ((tooEarly || tooLate || FMTest.Status == TestStatus.Closed))
				return false;
			return true;
		}
	}
}
