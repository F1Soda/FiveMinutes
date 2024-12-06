﻿using FiveMinute.Data;
using Microsoft.AspNetCore.Identity;

namespace FiveMinute.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<FiveMinuteTemplate> FMTTemplates { get; set; }
        public ICollection<FiveMinuteTest> FMTests { get; set; }
        public ICollection<FiveMinuteTestResult> PassedTestResults { get; set; }

        public string UserRole {  get; set; }
        public bool canCreate => UserRole is UserRoles.Admin or UserRoles.Teacher; 

        public void AddFMT(FiveMinuteTemplate fmt)
        {
            FMTTemplates.Add(fmt);
        }

        public void AddResult(FiveMinuteTestResult result)
        {
            PassedTestResults.Add(result);
        }
    }
}
