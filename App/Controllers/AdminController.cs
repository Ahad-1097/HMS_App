using App.Interface;
using App.Models.DbContext;
using App.Models;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using App.Models.Identity;

namespace App.Controllers
{

    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;
        private readonly IDocterRepo _docterRepo;
        private readonly IAccountRepo _account;
        private readonly IPatientRepo _patient;

        public AdminController(ApplicationContext context, IDocterRepo docterRepo, ILogger<HomeController> logger, IAccountRepo account, IPatientRepo patient)
        {

            _context = context;
            _docterRepo = docterRepo;
            _logger = logger;
            _account = account;
            _patient = patient;
        }


       // [Authorize(Roles = "Docter")]
       // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            DashboardViewModel model = new DashboardViewModel
            {
                TotalPatient = _docterRepo.TotalPatient(),
                TotalDocter = await _account.GetUserCountByRoleAsync("Doctor"),
                UserList = await _account.UserbyRole("All"),
                DocterList = await _account.UserbyRole("Doctor"),
                TotalNewPatient = _docterRepo.NewPatient(),
                TotalNewDicharge = _docterRepo.RecoverPatient()
            };
            return View(model);

        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            DashboardViewModel model = new DashboardViewModel
            {
                UserList = await _account.UserbyRole("All")
            };
            return View(model);
        }

        //public async Task<IActionResult> Patient(CancellationToken cancellationToken)
        //{
        //    var result = await _patient.GetPatientList(cancellationToken);
        //    return View(result);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet("/EditUser")]
        public async Task<IActionResult> EditUser(string userId)
        {

            var user = await _account.FindByIdAsync(userId);
            //var rolList = user.RoleList;
            //ViewBag.RoleList = new SelectList(rolList, "Name", "Name");
            if (user == null) { return NotFound(); }
            else
            {
                return View(user);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserWithRoleViewModel user)
        {
            if (!ModelState.IsValid)
            {
                // Handle invalid input
                return View(user);
            }

            var result = await _account.UpdateUserAsync(user);

            if (result.Succeeded)
            {
                // User successfully updated
                return RedirectToAction("Users"); // Redirect to a success page or action
            }
            else
            {
                // Handle update failure, display errors, etc.
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(user); // Display the update view with errors
            }
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(string userId, CancellationToken cancellationToken)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var user = await _account.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }


        [HttpGet("/DeleteUser")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest();
            }

            var result = await _account.DeleteUserAsync(userId);

            if (result.Succeeded)
            {
                return RedirectToAction("Users"); // Redirect to a success page or action
            }
            else
            {
                // Handle delete failure, display errors, etc.
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("ErrorView"); // Display an error view
            }
        }

        
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRegistrationModel user)
        {
            if (!ModelState.IsValid)
            {
                // Handle invalid input
                return View(user);
            }

            var result = await _account.Registration(user, user.UserRole);

            if (result.Succeeded)
            {
                // User successfully updated
                return RedirectToAction("Users"); // Redirect to a success page or action
            }
            else
            {
                // Handle update failure, display errors, etc.
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(user); // Display the update view with errors
            }
        }

        [HttpPost("RegisterUser")]
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
                return RedirectToAction("Users");
            }

            return View(userModel);
        }

    }
}

