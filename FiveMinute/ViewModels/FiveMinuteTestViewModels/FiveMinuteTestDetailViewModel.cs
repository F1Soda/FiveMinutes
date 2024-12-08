using FiveMinute.Models;
using FiveMinute.Data;
using FiveMinute.ViewModels.FMTEditViewModels;

namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
	public class FiveMinuteTestDetailViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? AttachedFMTId { get; set; }
		public FiveMinuteTemplateEditViewModel AttachedFMT { get; set; }
		public TestStatus Status { get; set; }

		public bool StartPlanned;
		public DateTime StartTime { get; set; }
		public bool EndPlanned;
		public DateTime EndTime { get; set; }
		public IEnumerable<FiveMinuteTestResult> Results { get; set; }
		public List<int> IdToUninclude { get; set; }

		public static FiveMinuteTestDetailViewModel CreateByModel(FiveMinuteTest fmTest)
		{
			return new FiveMinuteTestDetailViewModel
			{
				Id = fmTest.Id,
				Name = fmTest.Name,
				AttachedFMT = FiveMinuteTemplateEditViewModel.CreateByModel(fmTest.FiveMinuteTemplate),
				StartPlanned = fmTest.StartPlanned,
				StartTime = fmTest.StartTime,
				EndPlanned = fmTest.EndPlanned,
				EndTime = fmTest.EndTime,
				Results = fmTest.Results,
				Status = fmTest.Status,
				IdToUninclude = fmTest.IdToUninclude,
			};
		}
		public FiveMinuteTest CreateByView()//надо кому-то это дажать у меня сил не хватило но идея класс не получается null выдает т.к не хваитет ему элементов
		{
			return new FiveMinuteTest
			{
				Id = this.Id,
				Name = this.Name,
				FiveMinuteTemplate = this.AttachedFMT.CreateByView(),
				StartPlanned = this.StartPlanned,
				StartTime = this.StartTime,
				EndPlanned = this.EndPlanned,
				EndTime = this.EndTime,
				Results = this.Results.ToList(),
				Status = this.Status,
				IdToUninclude = this.IdToUninclude,
			};
		}
	}
}