using FiveMinute.Models;

namespace FiveMinute.ViewModels.AccountViewModels
{
    public class AllUsersViewModel
    {
        public List<UserIdNameEmailViewModel> Students { get; set; }
        public List<UserIdNameEmailViewModel> Teachers { get; set; }
        public List<UserIdNameEmailViewModel> Admins { get; set; }
    }
}
