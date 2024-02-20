using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Interface;
using App.Models.DbContext;
using App.Models.Identity;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace App.Repo
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepo(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<bool> ApproveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                user.IsApproved = true; // Set the user as approved
                await _userManager.UpdateAsync(user);
            }

            return true;
        }

        public async Task<IdentityResult> Registration(UserRegistrationModel _model, string userRole)
        {
            var _user = new User()
            {
                Email = _model.Email,
                UserName = _model.Email,
                FirstName = _model.FirstName,
                LastName = _model.LastName,
                IsApproved = false,
                IsActive = true

            };

            var result = await _userManager.CreateAsync(_user, _model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, userRole);

            }
            return result;
        }


        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<SignInResult> SignIn(UserLoginModel _model)
        {
            var result = await _signInManager.PasswordSignInAsync(_model.Email, _model.Password, _model.RememberMe, false);

            return result;
        }

        public async Task<List<UserWithRoleViewModel>> GetUsers()
        {
            var usersWithRolesAndApproval = await _userManager.Users.Select(user => new UserWithRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = _userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                IsApproved = user.IsApproved // Assuming you have an IsApproved property in your ApplicationUser model
            }).ToListAsync();

            return usersWithRolesAndApproval;
        }

         public async Task<List<UserWithRoleViewModel>> UserbyRole(string role)
        {
            if (role == "All")
            {
                var usersWithRoles = await _userManager.Users
                    .Where(a => a.IsActive)
                    .Select(user => new UserWithRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.FirstName + " " + user.LastName,
                        IsApproved = user.IsApproved
                    }).ToListAsync();

                foreach (var user in usersWithRoles)
                {
                    var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.UserId));
                    string roleValue = roles?.FirstOrDefault() ?? "No Role"; // Handle null or empty roles
                    user.Roles = roleValue;
                }

                var userViewModels = usersWithRoles.Select(user => new UserWithRoleViewModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserPhone = user.UserPhone,
                    Roles = user.Roles.Normalize(),
                    IsApproved = user.IsApproved
                }).ToList();
                return userViewModels;
            }
            else
            {
                List<UserWithRoleViewModel> userWithRole = new List<UserWithRoleViewModel>();
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                foreach (var user in usersInRole)
                {
                    UserWithRoleViewModel viewModel = new UserWithRoleViewModel();
                    viewModel.UserId = user.Id;
                    viewModel.UserName = user.FirstName + " " + user.LastName;
                    viewModel.UserPhone = user.PhoneNumber;
                    viewModel.UserEmail = user.Email;

                    var roles = await _userManager.GetRolesAsync(user);
                    string roleValue = roles?.FirstOrDefault() ?? "No Role"; // Handle null or empty roles
                    viewModel.Roles = roleValue;

                    userWithRole.Add(viewModel);
                }

                return userWithRole;
            }
        }


        //public async Task<List<UserWithRoleViewModel>> UserbyRole(string role)
        //{
        //    if (role == "All")
        //    {
        //        var usersWithRoles = await _userManager.Users
        //            .Where(a=>a.IsActive)
        //         .Select(user => new UserWithRoleViewModel
        //         {
        //             UserId = user.Id,
        //             UserName = user.FirstName + " " + user.LastName,
        //             IsApproved = user.IsApproved
        //         }).ToListAsync();

        //        foreach (var user in usersWithRoles)
        //        {

        //                string rolel = (await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.UserId))).FirstOrDefault();


        //        }

        //        var userViewModels = usersWithRoles.Select(user => new UserWithRoleViewModel
        //        {
        //            UserId = user.UserId,
        //            UserName = user.UserName,
        //            UserPhone = user.UserPhone,
        //            Roles = user.Roles.Normalize(),
        //            IsApproved = user.IsApproved
        //        }).ToList();
        //        return userViewModels;

        //    }
        //    else
        //    {

        //        List<UserWithRoleViewModel> userWithRole = new List<UserWithRoleViewModel>();
        //        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        //        foreach (var user in usersInRole)
        //        {
        //            UserWithRoleViewModel viewModel = new UserWithRoleViewModel();
        //            var roles = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        //            viewModel.UserId = user.Id;
        //            viewModel.UserName = user.FirstName + " " + user.LastName;
        //            viewModel.UserPhone = user.PhoneNumber;
        //            viewModel.UserEmail = user.Email;
        //            viewModel.Roles = roles;
        //            userWithRole.Add(viewModel);

        //        }

        //        return userWithRole;
        //    }
        //}

        public async Task<int> GetUserCountByRoleAsync(string roleName)
        {
            // Check if the role exists
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return 0; // Role does not exist
            }
            if (roleName == "ALL")
            {

                var usersInRole =  _userManager.Users.Count();
                return usersInRole;
            }
            else {
                // Find users in the role
                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                return usersInRole.Count;
            }


            // Return the count of users in the role
            
        }


        public async Task<IdentityResult> UpdateUserAsync(UserWithRoleViewModel _model)
        {

            var UserData = await _userManager.FindByIdAsync(_model.UserId);

            var currentRoles = await _userManager.GetRolesAsync(UserData);
            if (!currentRoles.Contains(_model.Roles))
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(UserData, currentRoles);
                if (removeRolesResult.Succeeded)
                {
                    var newRoles = new List<string> { _model.Roles }; // Add the new role(s) as needed
                    await _userManager.AddToRolesAsync(UserData, newRoles);
                }
            }

            if (UserData == null) { return IdentityResult.Failed(); }
            if (!string.IsNullOrWhiteSpace(_model.FirstName))
            {
                UserData.FirstName = _model.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(_model.LastName))
            {
                UserData.LastName = _model.LastName;
            }
            if (!string.IsNullOrWhiteSpace(_model.UserName))
            {
                UserData.UserName = _model.UserName;
            }
            if (!string.IsNullOrWhiteSpace(_model.UserEmail))
            {
                UserData.Email = _model.UserEmail;
            }
            if (!string.IsNullOrWhiteSpace(_model.UserPhone))
            {
                UserData.PhoneNumber = _model.UserPhone;
            }
            UserData.IsApproved = _model.IsApproved;
            return await _userManager.UpdateAsync(UserData);
        }

        public async Task<IdentityResult> DeleteUserAsync(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) { return null; }
            user.IsActive = false;
            return await _userManager.UpdateAsync(user);

        }

        public async Task<UserWithRoleViewModel> FindByIdAsync(string userId)
        {
            try
            {

                // Get all roles
                var allRoles = _roleManager.Roles.ToList();
                string Userrole = "";
                var user = await _userManager.FindByIdAsync(userId);
                var role = await _userManager.GetRolesAsync(user);
                if (role != null && role.Count > 0) { Userrole = role.FirstOrDefault(); }
                UserWithRoleViewModel userData = new UserWithRoleViewModel()
                {
                    UserId = userId,
                    UserEmail = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserPhone = user.PhoneNumber,
                    IsApproved = user.IsApproved,
                    Roles = Userrole,
                    RoleList = allRoles,
                };
                return userData;
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                // You can also throw a custom exception or return null
                throw;
            }
        }
    }
}

