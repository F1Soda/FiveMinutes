using System.ComponentModel.DataAnnotations;
using FiveMinute.Models;
using FiveMinute.Data;
using FiveMinute.ViewModels.FiveMinuteTestViewModels;
using FiveMinute.ViewModels.FMTEditViewModels;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.FiveMinuteTestViewModels
{
	public class FiveMinuteTestDetailViewModel: IInput<FiveMinuteTestDetailViewModel,FiveMinuteTest>,IOutput<FiveMinuteTestDetailViewModel,FiveMinuteTest>,IValidatableObject
	{
	public int Id { get; set; }
	[Required]
	public string Name { get; set; }
	[Required]
	public int? AttachedFMTId { get; set; }
	[Required]
	public FiveMinuteTemplateEditViewModel AttachedFMT { get; set; }
	public TestStatus Status { get; set; }

	public bool StartPlanned { get; set; }
	
	public DateTime StartTime { get; set; }
	public bool EndPlanned  { get; set; }
	public DateTime EndTime { get; set; }
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
	
	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		List<ValidationResult> errors = new List<ValidationResult>();
 
		if (StartTime > EndTime)
		{
			errors.Add(new ValidationResult("Введите имя!", new List<string>() { "Name" }));
		}
 
		return errors;
	}
	}
}
