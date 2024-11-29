using FiveMinute.Models;

namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
	public class FiveMinuteTestEditViewModel
	{
		public int Id;
		public string Name { get; set; }
		public FiveMinuteTemplate AttachedFMT { get; set; }
		public bool StartPlanned = false;
		public DateTime StartTime { get; set; }
		public bool EndPlanned = false;
		public DateTime EndTime { get; set; }

		public static FiveMinuteTestEditViewModel CreateByModel(FiveMinuteTest fmTest)
		{
			return new FiveMinuteTestEditViewModel
			{
				Id = fmTest.Id,
				Name = fmTest.Name,
				AttachedFMT = fmTest.AttachedFMT,
				StartPlanned = fmTest.StartPlanned,
				StartTime = fmTest.StartTime,
				EndPlanned = fmTest.EndPlanned,
				EndTime = fmTest.EndTime
			};
		}
	}
}