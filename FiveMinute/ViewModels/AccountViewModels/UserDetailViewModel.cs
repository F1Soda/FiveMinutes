﻿using FiveMinute.Data;
using FiveMinute.Models;

namespace FiveMinute.ViewModels.AccountViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public UserData UserData { get; set; }
        public string Email { get; set; }
        public bool IsOwner { get; set; }
        public string UserRole { get; set; }
        public ICollection<FiveMinuteTemplate> FMTs { get; set; }
        public ICollection<FiveMinuteTest> Tests { get; set; }
        public ICollection<FiveMinuteTestResult> PassedTestResults { get; set; }
    }
}
