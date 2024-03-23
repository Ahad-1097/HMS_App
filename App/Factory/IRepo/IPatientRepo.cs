using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.DtoModel;
using App.Models.DtoModel;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Interface
{
    public interface IPatientRepo
    {
        Task<List<PatientModel>> GetPatientList(CancellationToken cancellationToken, string role);
        PatientViewModel GetAllDetail(long? PatientID);
        Task<long> AddPatient(PatientModel _model, CancellationToken cancellationToken);
        Task UpdatePatient(PatientModel patient, CancellationToken cancellationToken);
        DataTable addTempData(List<InvestigationModel> model);
        void addTempData_img(InvestigationImagesModel model);
        Task<long> AddInvestigationData(InvestigationModel investigationData, long PatientID, CancellationToken cancellationToken);
        Task UpdateInvestigationData(InvestigationModel _model, CancellationToken cancellationToken);
        Task<bool> RemoveInvestigation(int InvestigationID,CancellationToken cancellationToken);
        Task<bool> DeletePatient(long id, CancellationToken cancellationToken);
         Task<bool> AddInvestigation(InvestigationModel _model, CancellationToken cancellationToken, long _patientId);
        Task<bool> AddInvestigationImages(InvestigationImagesModel _model, long investigationId, long _patientId, CancellationToken cancellationToken);
       Task<long> AddCaseSheet(CaseSheetModel _model, long patientid,CancellationToken cancellationToken);
        Task<long> AddOperationSheet(OperationModel _model, long patientid,CancellationToken cancellationToken);
       Task<long> AddProgress(ProgressModel _model, long patientid,CancellationToken cancellationToken);
        Task<long> AddDischarge(DischargeModel _model, long patientid,CancellationToken cancellationToken);
       Task<long> AddDiagnosis(PatientModel _model, long patientID,CancellationToken cancellationToken);
        Task UpdateInvestigationImages(InvestigationImagesModel _model, CancellationToken cancellationToken);
       Task<bool> UpdateProgress(ProgressModel _model,CancellationToken cancellationToken);
        Task<bool> UpdateCaseSheet(CaseSheetModel _model);
        Task<bool> UpdateOperationSheet(OperationModel _model);
        Task<bool> UpdateDischarge(DischargeModel _model,CancellationToken cancellationToken);
        Task<bool> UpdateOutCome(OutcomeModel _model,CancellationToken cancellationToken);
        Task<PatientViewModel> PatientDetail(long? PatientID, CancellationToken cancellationToken);
        Task<PatientViewModel> InvestigationDetail(long PatientID,CancellationToken cancellationToken);
        PatientViewModel PictureDetail(long PatientID, long imgId,string ViewName);
       Task<PatientViewModel> ProgressDetail(long PatientID,CancellationToken cancellationToken);
        PatientViewModel CaseSheetDetail(long PatientID);
        PatientViewModel OperationDetail(long PatientID);
        PatientViewModel DischargeDetail(long PatientID);
        //PatientViewModel EditOutCome(long? PatientID);
       Task<long> AddOutCome(OutcomeModel _model, long patientid,CancellationToken cancellationToken);
        public PatientViewModel OutComeDetail(long PatientID);
        Task UpdateVital(ProgressModel _model,long PatientId,CancellationToken cancellationToken);
        DischargePrintModel DischargePrintDetail(long PatientID);
        Task<string> Addimge(IFormFile img);

    }
}
