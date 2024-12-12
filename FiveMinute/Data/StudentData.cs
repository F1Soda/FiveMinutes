using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FiveMinute.Data;

[Owned]
public class StudentData
{
    [Display(Name = "Имя")]
    public string FirstName { get; set; }
    [Display(Name = "Фамилия")]
    public string LastName { get; set; }
    [Display(Name = "Группа")]
    public string Group{ get; set; }
}