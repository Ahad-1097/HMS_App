using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Models.Identity;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace App.Interface
{
    public interface IAccountRepo
    {
        Task<IdentityResult> Registration(UserRegistrationModel _model, string userRole);
        Task<SignInResult> SignIn(UserLoginModel _model);
        Task SignOut();
        Task<List<UserWithRoleViewModel>> GetUsers();
        Task<List<UserWithRoleViewModel>> UserbyRole(string role);
        Task<bool> ApproveUser(string userId);
        Task<int> GetUserCountByRoleAsync(string roleName);

        Task<IdentityResult> UpdateUserAsync(UserWithRoleViewModel user);
        Task<IdentityResult> DeleteUserAsync(string userId);
        Task<UserWithRoleViewModel> FindByIdAsync(string userId);
    }
}
