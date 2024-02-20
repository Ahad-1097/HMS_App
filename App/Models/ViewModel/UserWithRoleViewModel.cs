using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace App.Models.ViewModel
{
    public class UserWithRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; } = string.Empty;   
        public string Roles { get; set; }
        public List<IdentityRole> RoleList { get; set; }
        public bool IsApproved { get; set; }
    }
}
