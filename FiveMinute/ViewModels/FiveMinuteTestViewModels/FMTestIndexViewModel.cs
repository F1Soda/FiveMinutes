using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
	public class FMTestIndexViewModel: IInput<FMTestIndexViewModel,FiveMinuteTest>
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? AttachedFMTemplateId { get; set; }
		public string AttachedFMTemplateName { get; set; }
		public TestStatus Status { get; set; }
		public bool StartPlanned = false;
		public DateTime StartTime { get; set; }
		public bool EndPlanned = false;
		public DateTime EndTime { get; set; }
	
		public static FMTestIndexViewModel CreateByModel(FiveMinuteTest model)
		{
			return new FMTestIndexViewModel
			{
				Id = model.Id,
				Name = model.Name,
				AttachedFMTemplateId = model.FiveMinuteTemplateId,
				AttachedFMTemplateName = model.FiveMinuteTemplate.Name,
				StartPlanned = model.StartPlanned,
				StartTime = model.StartTime,
				EndPlanned = model.EndPlanned,
				EndTime = model.EndTime,
				Status = model.Status,
			};
		}
	}
}
