using App.Interface;
using App.Models;
using App.Models.DbContext;
using App.Models.EntityModels;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace App.Controllers
{
    public class ResidentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        private readonly IDocterRepo _docterRepo;


        public ResidentController(ApplicationContext context, IDocterRepo docterRepo, ILogger<HomeController> logger)
        {

            _context = context;
            _docterRepo = docterRepo;
            _logger = logger;
        }

        //[Authorize(Roles = "Consultant")]
        public IActionResult Index()
        {
            DashboardViewModel model = new DashboardViewModel();
            model.TotalPatient = _docterRepo.TotalPatient();
            model.TotalDocter = _docterRepo.TotalDotor();
            var doctors = _docterRepo.GetDoterList();
           //model.Doctors = doctors;

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
    }
}

