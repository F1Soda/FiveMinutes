using FiveMinute.Data;
using FiveMinute.Models;
using FiveMinute.ViewModels.FMResultViewModels;
using FiveMinute.ViewModels.Interfaces;

namespace FiveMinute.ViewModels.AccountViewModels
{
    public class UserDetailViewModel: IInput<UserDetailViewModel,AppUser>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public UserData UserData { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
        public string UserRole { get; set; }

        public ICollection<FMTResultForAccountDetailViewModel> PassedTestResults { get; set; }
        public static UserDetailViewModel CreateByModel(AppUser user)
        {
            return new UserDetailViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                IsOwner = true,
                UserData = user.UserData,
                PassedTestResults = user.PassedTestResults.Select(FMTResultForAccountDetailViewModel.CreateByModel).ToList()
            };
        }
    }
}
