using FiveMinutes.Models;

namespace FiveMinutes.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public ICollection<FiveMinuteTemplate> FMTs { get; set; }
        public ICollection<EducationTest> Tests { get; set; }
    }
}
