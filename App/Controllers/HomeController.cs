
using App.Interface;
using App.Models;
using App.Models.DbContext;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace App.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepo _account;
        private readonly ApplicationContext _context;

        private readonly IDocterRepo _docterRepo;


        public HomeController(ApplicationContext context, IDocterRepo docterRepo,
            ILogger<HomeController> logger, IAccountRepo account)
        {

            _context = context;
            _docterRepo = docterRepo;
            _logger = logger;
            _account = account;
        }


        //[Authorize(Roles = "Consultant")]
        public async Task<IActionResult> Index()
        {

            var doctors = _docterRepo.GetDoterList();
            DashboardViewModel model = new DashboardViewModel();
            model.TotalPatient = _docterRepo.TotalPatient();
            model.TotalDocter = await _account.GetUserCountByRoleAsync("Docter");
            List<UserWithRoleViewModel> userList = await _account.UserbyRole("All");
            model.UserList = await _account.UserbyRole("Docter");
            model.TotalNewPatient = _docterRepo.NewPatient();
            model.TotalNewDicharge = _docterRepo.RecoverPatient();
            return View(model);
        }

      
       


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public class Item
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
        }
    }
}
