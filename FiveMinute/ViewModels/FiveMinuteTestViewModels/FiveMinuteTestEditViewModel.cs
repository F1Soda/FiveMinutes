using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
	public class FiveMinuteTestEditViewModel : IInput<FiveMinuteTestEditViewModel,FiveMinuteTest> , IOutput<FiveMinuteTestEditViewModel,FiveMinuteTest>
	{
		public int Id;
		public string Name { get; set; }
		public FiveMinuteTemplate AttachedFMT { get; set; }
		public int AttachedFMTId { get; set; }
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
				AttachedFMT = fmTest.FiveMinuteTemplate,
				StartPlanned = fmTest.StartPlanned,
				StartTime = fmTest.StartTime,
				EndPlanned = fmTest.EndPlanned,
				EndTime = fmTest.EndTime
			};
		}

		public static FiveMinuteTest CreateByView(FiveMinuteTestEditViewModel model)
		{
			return new FiveMinuteTest
			{
				Id = model.Id,
				Name = model.Name,
				FiveMinuteTemplate = model.AttachedFMT,
				FiveMinuteTemplateId = model.AttachedFMT.Id,
				StartPlanned = model.StartPlanned,
				StartTime = model.StartTime,
				EndPlanned = model.EndPlanned,
				EndTime = model.EndTime,
			};
		}
	}
}