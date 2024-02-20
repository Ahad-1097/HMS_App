
using App.DtoModel;
using App.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace App.Interface
{
    public interface IDocterRepo
    {
        List<DoctorModel> GetDoterList();
        int TotalPatient();
        int TotalDotor();
        Task DeleteDoctor(long Dr_Id, CancellationToken cancellationToken);
        Task AddDoctor(Doctor doctor, CancellationToken cancellationToken);
        Task<List<DropDrownModel>> getDropDownlist(string role);
        int NewPatient();
        int RecoverPatient();

    }


}
