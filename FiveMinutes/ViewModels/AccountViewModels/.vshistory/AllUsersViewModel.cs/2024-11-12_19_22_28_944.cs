using FiveMinutes.Models;

namespace FiveMinutes.ViewModels.AccountViewModels
{
    public class AllUsersViewModel
    {
        public List<AllUsersViewModel> Students { get; set; }
        public List<AllUsersViewModel> Teachers { get; set; }
        public List<AllUsersViewModel> Admins { get; set; }
    }
}
