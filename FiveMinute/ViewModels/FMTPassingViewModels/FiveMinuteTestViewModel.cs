using System.ComponentModel.DataAnnotations;
using FiveMinute.Data;
using FiveMinute.Models;

namespace FiveMinute.ViewModels;

public class FiveMinuteTestViewModel
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
}