using System.ComponentModel.DataAnnotations;
using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels;

public class FiveMinuteTestViewModel: IInput<FiveMinuteTestViewModel,FiveMinuteTest>
{
    public string Name { get; set; }
    public int FMTestId;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    // [Display(Name = "Имя")]
    // public string? UserFirstName { get; set; }
    // [Display(Name = "Фамилия")]
    // public string? UserLastName { get; set; }
    // [Display(Name = "Группа")]
    
    public UserData StudentData { get; set; }

    public string? UserGroup { get; set; }

    public IEnumerable<QuestionViewModel> Questions { get; set; }
    public static FiveMinuteTestViewModel CreateByModel(FiveMinuteTest fmTest)
    {
        var fmTemplate = fmTest.FiveMinuteTemplate;
        return new FiveMinuteTestViewModel
        {
            Name = fmTemplate.Name,
            FMTestId = fmTest.Id,
            StartTime = fmTest.StartTime,
            EndTime = fmTest.EndTime,
            Questions = fmTemplate.Questions.Where(x=>!fmTest.IdToUninclude.Contains(x.Id)).Select(x => QuestionViewModel.CreateByModel(x)),
        };
    }
}