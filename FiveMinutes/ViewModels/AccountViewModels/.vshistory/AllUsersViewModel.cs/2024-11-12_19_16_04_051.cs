using FiveMinutes.Models;

namespace FiveMinutes.ViewModels.AccountViewModels
{
    public class AllUsersViewModel
    {
        public List<AppUser> Students { get; set; }
        public List<AppUser> Teachers { get; set; }
        public List<AppUser> Admins { get; set; }
    }
}
