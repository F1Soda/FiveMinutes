using System.ComponentModel.DataAnnotations;
using FiveMinute.Models;
using FiveMinute.Data;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
	public class FiveMinuteTestDetailViewModel: IInput<FiveMinuteTestDetailViewModel,FiveMinuteTest>,IOutput<FiveMinuteTestDetailViewModel,FiveMinuteTest>
	{
	public int Id { get; set; }
	public string Name { get; set; }
	public int? AttachedFMTId { get; set; }
	public FiveMinuteTemplateEditViewModel AttachedFMT { get; set; }
	public TestStatus Status { get; set; }

	public bool StartPlanned { get; set; }
	
	public DateTime StartTime { get; set; }
	public bool EndPlanned  { get; set; }
	public DateTime EndTime { get; set; }
	[DateRange(ErrorMessage = "Дата начала должна быть меньше даты конца.")]
	public bool IsValidDateRange => StartTime < EndTime;
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

	public static FiveMinuteTest CreateByView(FiveMinuteTestDetailViewModel FMTestView)
	{
		return new FiveMinuteTest
		{
			Id = FMTestView.Id,
			Name = FMTestView.Name,
			StartPlanned = FMTestView.StartPlanned,
			StartTime = FMTestView.StartTime.ToUniversalTime(),
			EndPlanned = FMTestView.EndPlanned,
			EndTime = FMTestView.EndTime.ToUniversalTime(),
			Status = FMTestView.Status,
			IdToUninclude = FMTestView.IdToUninclude,
		};
	}
	}
}

public class DateRangeAttribute : ValidationAttribute
{
	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		var model = (FiveMinuteTestDetailViewModel)validationContext.ObjectInstance;

		if (model.StartTime >= model.EndTime)
		{
			return new ValidationResult(ErrorMessage);
		}

		return ValidationResult.Success;
	}
}