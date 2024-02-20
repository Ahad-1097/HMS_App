using App.Interface;
using App.Models.EntityModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using App.Models.DbContext;
using Microsoft.Extensions.Logging;

namespace App.Controllers
{
    public class DoctorController : Controller
    {
     
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;

        public DoctorController(ApplicationContext context, IDocterRepo docterRepo, ILogger<HomeController> logger)
        {

            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            //var data=_docterRepo.GetDoterList();
            //return View(data);
            return View();
        }

        public IActionResult AddDoctor()
        {
            return View();
        }

        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
        {
             //await _docterRepo.DeleteDoctor(id, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

    }
}
