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
        long AddPatient(PatientModel patient, CancellationToken cancellationToken);
        Task UpdatePatient(PatientModel patient, CancellationToken cancellationToken);
        DataTable addTempData(List<InvestigationModel> model);
        void addTempData_img(InvestigationImagesModel model);
        long AddInvestigationData(InvestigationModel InvestigationData, long PatientID);
        Task UpdateInvestigationData(InvestigationModel _model, CancellationToken cancellationToken);
        bool RemoveInvestigation(int InvestigationID);
        Task<bool> DeletePatient(long id, CancellationToken cancellationToken);
        bool AddInvestigation(InvestigationModel patient, CancellationToken cancellationToken, long _patientId);
        bool AddInvestigationImages(InvestigationImagesModel imagesModel, long investigationId, long _patientId);
        long AddCaseSheet(CaseSheetModel _model, long patientid);
        long AddOperationSheet(OperationModel _model, long patientid);
        long AddProgress(ProgressModel _model, long patientid);
        long AddDischarge(DischargeModel _model, long patientid);
        long AddDiagnosis(PatientModel _model, long patientID);
        Task UpdateInvestigationImages(InvestigationImagesModel _model, CancellationToken cancellationToken);
        bool UpdateProgress(ProgressModel _model);
        Task<bool> UpdateCaseSheet(CaseSheetModel _model);
        Task<bool> UpdateOperationSheet(OperationModel _model);
        bool UpdateDischarge(DischargeModel _model);
        bool UpdateOutCome(OutcomeModel _model);
        Task<PatientViewModel> PatientDetail(long? PatientID, CancellationToken cancellationToken);
        PatientViewModel InvestigationDetail(long PatientID);
        PatientViewModel PictureDetail(long PatientID, long imgId);
        PatientViewModel ProgressDetail(long PatientID);
        PatientViewModel CaseSheetDetail(long PatientID);
        PatientViewModel OperationDetail(long PatientID);
        PatientViewModel DischargeDetail(long PatientID);
        //PatientViewModel EditOutCome(long? PatientID);
        long AddOutCome(OutcomeModel _model, long patientid);
        public PatientViewModel OutComeDetail(long PatientID);
        void UpdateVital(ProgressModel _model,long PatientId);

    }
}
