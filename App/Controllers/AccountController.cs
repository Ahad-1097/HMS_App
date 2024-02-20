using App.Interface;
using App.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace App.Controllers
{


    public class AccountController : Controller
    {
        private readonly IAccountRepo _account;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
       
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IAccountRepo account)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _account = account;
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]      
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegistrationModel userModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _account.Registration(userModel, userModel.UserRole);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return View(userModel);
                }
                ModelState.Clear();
                TempData["Registerd"] = "Success";
                return RedirectToAction("Login");
            }

            return View(userModel);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel _model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(_model.Email, _model.Password, _model.RememberMe, false);

                if (result.Succeeded)
                {
                    // Retrieve the user object
                    var user = await _userManager.FindByEmailAsync(_model.Email);
                    if (user != null && user.IsActive == true)
                    {

                        // Get the user's roles
                        var roles = await _userManager.GetRolesAsync(user);

                        // Determine the redirection based on user roles
                        if (roles.Contains("Admin"))
                        {
                            return new RedirectToActionResult("Index", "Admin", null);
                        }

                        else if (roles.Contains("Docter"))
                        {
                            return new RedirectToActionResult("Index", "Admin", null);
                        }

                        else if (roles.Contains("Sr"))
                        {
                            return new RedirectToActionResult("Index", "Home", null);
                        }

                        else if (roles.Contains("Jr"))
                        {
                            return new RedirectToActionResult("Index", "Home", null);
                        }

                        else if (roles.Contains("User"))
                        {
                            return new RedirectToActionResult("Index", "Home", null);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Account Not Approved By Admin");
                        return View(_model);
                    }
                }

                // Handle invalid login
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(_model);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log them, or perform any necessary actions
                // You might want to display a user-friendly error message or log the error for debugging
                // For example, you can use a logging framework like Serilog or log to a file or database
                // Log the exception
                //_logger.LogError(ex, "An error occurred during user login.");

                // Redirect to an error page or display a generic error message
               // return RedirectToAction("Login", "Account"); // Customize this based on your application

                ModelState.AddModelError(string.Empty, ex.Message);
                return View(_model);
            }
        }


        
        public async Task<IActionResult> Logout()
        {
            await _account.SignOut();

            //return RedirectToAction(nameof(HomeController.Index), "Home");
            return RedirectToAction("Login");
        }

        //private IActionResult RedirectToLocal(string returnUrl)
        //{
        //    if (Url.IsLocalUrl(returnUrl))
        //        return Redirect(returnUrl);
        //    else
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    // User successfully deleted
                    return RedirectToAction("Index"); // Redirect to a success page or action
                }
                else
                {
                    // Handle delete failure, display errors, etc.
                    // You can access errors using result.Errors
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("ErrorView"); // Display an error view
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions that may occur during the delete operation
                // Log the exception or take appropriate action
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the user.");
                return View("ErrorView"); // Display an error view
            }
        }
    }

}

