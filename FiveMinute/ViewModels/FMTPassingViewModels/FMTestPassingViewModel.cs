using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class FMTestPassingViewModel : IInput<FMTestPassingViewModel, FiveMinuteTest>
{
	public string Name { get; set; }
	public int FMTestId;
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public UserData UserData { get; set; }
	public string userId { get; set; }
	public IEnumerable<QuestionViewModel> Questions { get; set; }
	public static FMTestPassingViewModel CreateByModel(FiveMinuteTest fmTest)
	{
		var fmTemplate = fmTest.FiveMinuteTemplate;
		return new FMTestPassingViewModel
		{
			Name = fmTemplate.Name,
			FMTestId = fmTest.Id,
			StartTime = fmTest.StartTime,
			EndTime = fmTest.EndTime,
			Questions = fmTemplate.Questions.Where(x => !fmTest.IdToUninclude.Contains(x.Id)).Select(x => QuestionViewModel.CreateByModel(x)),
		};
	}
}