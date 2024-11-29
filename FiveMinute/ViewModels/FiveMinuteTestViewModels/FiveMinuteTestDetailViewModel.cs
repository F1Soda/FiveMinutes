using FiveMinute.Models;
using FiveMinutes.Data;
using FiveMinutes.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
	public class FiveMinuteTestDetailViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? AttachedFMTId { get; set; }
		public FiveMinuteTemplate AttachedFMT { get; set; }
		public TestStatus Status { get; set; }

		public bool StartPlanned = false;
		public DateTime StartTime { get; set; }
		public bool EndPlanned = false;
		public DateTime EndTime { get; set; }
		public IEnumerable<FiveMinuteResult> Results { get; set; }

		public static FiveMinuteTestDetailViewModel CreateByModel(FiveMinuteTest fmTest)
		{
			return new FiveMinuteTestDetailViewModel
			{
				Id = fmTest.Id,
				Name = fmTest.Name,
				AttachedFMT = fmTest.AttachedFMT,
				StartPlanned = fmTest.StartPlanned,
				StartTime = fmTest.StartTime,
				EndPlanned = fmTest.EndPlanned,
				EndTime = fmTest.EndTime,
				Results = fmTest.Results,
				Status = fmTest.Status
			};
		}
	}
}
