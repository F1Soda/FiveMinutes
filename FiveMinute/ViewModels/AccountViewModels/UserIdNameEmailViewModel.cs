namespace FiveMinutes.ViewModels.AccountViewModels
{
    public class UserIdNameEmailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    
        
        public UserIdNameEmailViewModel(string id, string userName, string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }
    }
}
