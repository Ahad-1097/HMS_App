using App.DtoModel;
using App.Interface;
using App.Models.DbContext;
using App.Models.EntityModels;
using App.Models.Identity;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Repo
{
    public class DocterRepo : IDocterRepo
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public DocterRepo(ApplicationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddDoctor(Doctor doctor, CancellationToken cancellationToken)
        {
            if (doctor == null) { return; }
            doctor.IsActive = true;
            await _context.Doctor.AddAsync(doctor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteDoctor(long Dr_Id, CancellationToken cancellationToken)
        {
            var data = await _context.Doctor.FirstOrDefaultAsync(a => a.Dr_ID == Dr_Id, cancellationToken);
            if (data == null) { return; }
            data.IsActive = false;
            await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<DoctorModel>> GetDoterList()
        {
            var drList = new List<DoctorModel>();
            var usersInRole = await _userManager.GetUsersInRoleAsync("Doctor");
            drList = usersInRole.Where(a => a.IsActive)
                  .Select(a => new DoctorModel
                  {
                      Dr_Name = a.FirstName + " " + a.LastName,
                      Dr_ID = a.Id
                  }).ToList();
            
            return drList;
        }

        public async Task<List<DropDrownModel>> getDropDownlist(string role)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            var DrList = usersInRole
                .Select(a => new DropDrownModel()
                {
                    ID = a.Id,
                    Name = a.FirstName + " " + a.LastName
                });
            return DrList.ToList();
        }

        public int TotalDotor()
        {
            return _context.Doctor.Count();
        }

        public int TotalPatient()
        {
            return _context.Patient.Where(a => a.IsActive).Count();
        }

        public int NewPatient()
        {
            return _context.Patient.Where(a => a.Status == "Admitted" && a.IsActive).Count();
        }

        public int RecoverPatient()
        {
            return _context.Patient.Where(a => a.Status == "Discharge" && a.IsActive).Count();
        }


    }
}
