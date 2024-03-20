using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using App.DtoModel;
using App.Factory.Helper;
using App.Interface;
using App.Models.DbContext;
using App.Models.DtoModel;
using App.Models.EntityModels;
using App.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace App.Repo
{
    public class PatientRepo : IPatientRepo
    {
        private readonly ApplicationContext _context;
        IWebHostEnvironment _hostEnvironment;
        MyDataTable dataTable = new MyDataTable();
        static List<InvestigationImagesModel> imageFileList = new List<InvestigationImagesModel>();

        public PatientRepo(ApplicationContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<List<PatientModel>> GetPatientList(CancellationToken cancellationToken, string Role)
        {
            if (Role == "Admin")
            {

                var data = await _context.Patient
                  //await _context.Patient.Join(_context.Users,pt=>pt.UserId,us=>us.Id,(pt,user) =>new {patien=pt,User=user})
                  .Where(a => a.IsActive == true)
                  .OrderByDescending(a => a.PatientID)
                    .Select(a => new PatientModel
                    {
                        PatientID = a.PatientID,
                        SerialNumber = a.SerialNumber,
                        Name = a.Name,
                        Gender = a.Gender,
                        DOA = a.DOA,
                        PhoneNumber = a.PhoneNumber,
                        CADSNumber = a.CADSNumber,
                        OPDNumber = a.OPDNumber,
                        SeniorResident = a.SeniorResident,
                        OtherO = a.OtherO,
                        OtherT = a.OtherT,
                        OtherTh = a.OtherTh,
                        Street = a.Address.Street,
                        City = a.Address.City,
                        State = a.Address.State,
                        ZipCode = a.Address.ZipCode,
                        // Dr_Name = "Dr."+ a.User.FirstName +" "+ a.User.LastName,
                        Daignosis = a.Daignosis,
                        Side = a.Side,
                        CoMorbity = a.CoMorbity,
                        CreatedOn = a.CreatedOn
                        //SubCategoryTitle = a.SubCategory.SubCategoryTitle
                    }).ToListAsync(cancellationToken);
                return data;
            }
            else
            {
                var data = await _context.Patient.Join(_context.Users, pt => pt.DrId, us => us.Id, (pt, user) => new { patien = pt, User = user })
                    .Where(a => a.patien.IsActive)
                   .OrderByDescending(a => a.patien.CreatedOn)
                   .Select(a => new PatientModel
                   {
                       PatientID = a.patien.PatientID,
                       SerialNumber = a.patien.SerialNumber,
                       Name = a.patien.Name,
                       Gender = a.patien.Gender,
                       DOA = a.patien.DOA,
                       PhoneNumber = a.patien.PhoneNumber,
                       CADSNumber = a.patien.CADSNumber,
                       OPDNumber = a.patien.OPDNumber,
                       SeniorResident = a.patien.SeniorResident,
                       OtherO = a.patien.OtherO,
                       OtherT = a.patien.OtherT,
                       OtherTh = a.patien.OtherTh,
                       Street = a.patien.Address.Street,
                       City = a.patien.Address.City,
                       State = a.patien.Address.State,
                       ZipCode = a.patien.Address.ZipCode,
                       Dr_Name = "Dr." + a.User.FirstName + " " + a.User.LastName,
                       Daignosis = a.patien.Daignosis,
                       Side = a.patien.Side,
                       CoMorbity = a.patien.CoMorbity,
                       //SubCategoryTitle = a.SubCategory.SubCategoryTitle
                   }).ToListAsync(cancellationToken);
                return data;
            }
        }

        public PatientViewModel GetAllDetail(long? PatientID)
        {

            PatientViewModel pvm = new PatientViewModel();

            pvm.PatientModel = _context.Patient.Join(_context.Users, pt => pt.DrId, us => us.Id, (pt, user) => new { patien = pt, User = user })
                    .Where(a => a.patien.IsActive && a.patien.PatientID == PatientID)
                    .Select(a => new PatientModel
                    {
                        PatientID = a.patien.PatientID,
                        SerialNumber = a.patien.SerialNumber,
                        Name = a.patien.Name,
                        Gender = a.patien.Gender,
                        DOA = a.patien.DOA,
                        PhoneNumber = a.patien.PhoneNumber,
                        CADSNumber = a.patien.CADSNumber,
                        OPDNumber = a.patien.OPDNumber,
                        SeniorResident = a.patien.SeniorResident,
                        JuniorResident = a.patien.JuniorResident,
                        OtherO = a.patien.OtherO,
                        OtherT = a.patien.OtherT,
                        OtherTh = a.patien.OtherTh,
                        Street = a.patien.Address.Street,
                        City = a.patien.Address.City,
                        State = a.patien.Address.State,
                        ZipCode = a.patien.Address.ZipCode,
                        Dr_Name = "Dr." + a.User.FirstName + " " + a.User.LastName,
                        Daignosis = a.patien.Daignosis,
                        Side = a.patien.Side,
                        CoMorbity = a.patien.CoMorbity,
                        //SubCategoryTitle = a.SubCategory.SubCategoryTitle
                    }).FirstOrDefault();


            // var _Patient = _context.Patient.Include(p => p.Address).Include(d => d.Consultant).Where(a => a.PatientID == PatientID && a.IsActive).FirstOrDefault();
            var _Investigation = _context.Investigation.Include(a => a.Patient).Where(a => a.PatientID == PatientID).ToList();
            var _InvestigationImages = _context.InvestigationImages.Where(a => a.PatientId == PatientID).FirstOrDefault();
            var _Progress = _context.Progress.Where(a => a.PatientID == PatientID).FirstOrDefault();
            var _Operation = _context.Operation.Where(a => a.PatientID == PatientID).FirstOrDefault();
            var _Discharge = _context.Discharge.Where(a => a.PatientID == PatientID).FirstOrDefault();
            var _CaseSheet = _context.CaseSheet.Where(a => a.PatientID == PatientID).FirstOrDefault();
            pvm.InvestigationList = _Investigation;
            pvm.InvestigationImages = _InvestigationImages;
            // pvm.Patient = _Patient;
            pvm.Progress = _Progress;
            pvm.Operation = _Operation;
            pvm.Discharge = _Discharge;
            pvm.CaseSheet = _CaseSheet;

            return pvm;
        }

        public async Task<PatientViewModel> PatientDetail(long? PatientID, CancellationToken cancellationToken)
        {

            PatientViewModel pvm = new PatientViewModel();
            pvm.PatientModel = await _context.Users
                .Join(_context.Patient,
                      user => user.Id,
                      patient => patient.DrId,
                      (user, patient) => new { User = user, Patient = patient })
                .Where(a => a.Patient.IsActive && a.Patient.PatientID == PatientID)
                .Select(a => new PatientModel
                {
                    PatientID = a.Patient.PatientID,
                    SerialNumber = a.Patient.SerialNumber,
                    Name = a.Patient.Name,
                    Gender = a.Patient.Gender,
                    Age = a.Patient.Age,
                    DOA = a.Patient.DOA,
                    PhoneNumber = a.Patient.PhoneNumber,
                    AlternateNumber = a.Patient.AlternateNumber,
                    CADSNumber = a.Patient.CADSNumber,
                    OPDNumber = a.Patient.OPDNumber,
                    SeniorResident = a.Patient.SeniorResident,
                    JuniorResident = a.Patient.JuniorResident,
                    OtherO = a.Patient.OtherO,
                    OtherT = a.Patient.OtherT,
                    OtherTh = a.Patient.OtherTh,
                    Address_ID = a.Patient.Address.ID,
                    Street = a.Patient.Address.Street,
                    City = a.Patient.Address.City,
                    State = a.Patient.Address.State,
                    ZipCode = a.Patient.Address.ZipCode,
                    Dr_ID = a.Patient.DrId,
                    Dr_Name = !a.User.FirstName.Contains("Dr")? "Dr." + a.User.FirstName + " " + a.User.LastName : a.User.FirstName + " " + a.User.LastName,
                    Daignosis = a.Patient.Daignosis,
                    Side = a.Patient.Side,
                    CoMorbity = a.Patient.CoMorbity,
                    //SubCategoryTitle = a.SubCategory.SubCategoryTitle
                }).FirstOrDefaultAsync(cancellationToken);
            return pvm;
        }

        public PatientViewModel InvestigationDetail(long PatientID)
        {

            PatientViewModel pvm = new PatientViewModel();
            //pvm.InvestigationList = _context.Investigation.Include(a => a.Patient).Where(a => a.PatientID == PatientID).ToList();
            pvm.InvestigationList = _context.Investigation.Where(a => a.PatientID == PatientID).ToList();
            if (pvm.InvestigationList == null || pvm.InvestigationList.Count==0)
            {
                var inv = new Investigation()
                {
                    PatientID=PatientID
                };
                pvm.InvestigationModel = new InvestigationModel() { PatientID=PatientID};
                pvm.InvestigationList.Add(inv);
            }
            return pvm;
        }

        public PatientViewModel PictureDetail(long PatientID, long imgId ,string ViewName)
        {
            try
            {
                PatientViewModel pvm = new PatientViewModel
                {
                    InvestigationImagesList = _context.InvestigationImages.Where(a => a.PatientId == PatientID).ToList()
                };
                if (ViewName == "Detail")
                {
                    return pvm;
                }
                if (imgId == 0)
                {
                    pvm.InvestigationImages = pvm.InvestigationImagesList.FirstOrDefault();
                }
                else if (imgId > 0)
                {
                    pvm.InvestigationImages = pvm.InvestigationImagesList.Where(a => a.Id == imgId).FirstOrDefault();
                }
                pvm.InvestigationImages ??= new InvestigationImages
                {
                    PatientId = (int)PatientID
                };
                return pvm;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public PatientViewModel ProgressDetail(long PatientID)
        {

            PatientViewModel pvm = new PatientViewModel();
            pvm.Progress = _context.Progress.Where(a => a.PatientID == PatientID).FirstOrDefault();

            if (pvm.Progress == null)
            {
                pvm.Progress = new Progress();
                pvm.Progress.PatientID = PatientID;
            }
            return pvm;
        }

        public PatientViewModel CaseSheetDetail(long PatientID)
        {

            PatientViewModel pvm = new PatientViewModel();
            pvm.CaseSheet = _context.CaseSheet.Where(a => a.PatientID == PatientID).FirstOrDefault();

            if (pvm.CaseSheet == null)
            {
                pvm.CaseSheet = new CaseSheet();
                pvm.CaseSheet.PatientID = PatientID;
            }
            return pvm;
        }

        public PatientViewModel OperationDetail(long PatientID)
        {

            PatientViewModel pvm = new PatientViewModel();

            pvm.Operation = _context.Operation.Where(a => a.PatientID == PatientID).FirstOrDefault();

            if (pvm.Operation == null)
            {
                pvm.Operation = new Operation();
                pvm.Operation.PatientID = PatientID;
            }
            return pvm;
        }

        public PatientViewModel DischargeDetail(long PatientID)
        {

            PatientViewModel pvm = new PatientViewModel();

            //pvm.Patient = _context.Patient.Include(p => p.Address).Include(d => d.Consultant).Where(a => a.PatientID == PatientID && a.IsActive).FirstOrDefault();
            pvm.Patient = _context.Patient.Include(p => p.Address).Where(a => a.PatientID == PatientID && a.IsActive).FirstOrDefault();
            pvm.Discharge = _context.Discharge.Where(a => a.PatientID == PatientID).FirstOrDefault();

            if (pvm.Discharge == null)
            {
                pvm.Discharge = new Discharge();
                pvm.Discharge.PatientID = PatientID;
                pvm.Discharge.JuniorResident = pvm.Patient.JuniorResident;
                pvm.Discharge.SeniorResident = pvm.Patient.SeniorResident;
            }
            return pvm;
        }

        public DischargePrintModel DischargePrintDetail(long PatientID)
        {

            //pvm.Patient = _context.Patient.Include(p => p.Address).Include(d => d.Consultant).Where(a => a.PatientID == PatientID && a.IsActive).FirstOrDefault();
            //pvm.Patient = _context.Patient.Include(p => p.Address).Where(a => a.PatientID == PatientID && a.IsActive).FirstOrDefault();
            var PatientData = _context.Users
                                .Join(_context.Patient,
                                      user => user.Id,
                                      patient => patient.DrId,
                                      (user, patient) => new { User = user, Patient = patient })
                                .Join(_context.Address,
                                      data => data.Patient.Address_ID,
                                      address => address.ID,
                                      (data, address) => new { data.User, data.Patient, Address = address })
                                .Where(a => a.Patient.IsActive && a.Patient.PatientID == PatientID)
                                .FirstOrDefault();

            var Dischargedata = _context.Discharge.Where(a => a.PatientID == PatientID).FirstOrDefault();

            if (PatientData != null)
            {
                DischargePrintModel DischargePrintdata = new DischargePrintModel()
                {

                    Name = PatientData.Patient.Name,
                    Gender = PatientData.Patient.Gender,
                    Age = PatientData.Patient.Age,
                    DOA = PatientData.Patient.DOA.ToString("dd/MM/yyyy"),
                    DOD=Dischargedata==null?"": Dischargedata.DOD.ToString(),
                    PhoneNumber = PatientData.Patient.PhoneNumber,
                    //AlternateNumber = PatientData.Patient.AlternateNumber,
                    CADSNumber = PatientData.Patient.CADSNumber,
                    OPDNumber = PatientData.Patient.OPDNumber,
                    SeniorResident = PatientData.Patient.SeniorResident,
                    JuniorResident = PatientData.Patient.JuniorResident,
                    Address = PatientData.Patient.Address.Street+" " + PatientData.Patient.Address.City + " " + PatientData.Patient.Address.State + " " + PatientData.Patient.Address.ZipCode,
                    
                    Dr_Name = !PatientData.User.FirstName.Contains("Dr",StringComparison.OrdinalIgnoreCase)? "Dr." + PatientData.User.FirstName + " " + PatientData.User.LastName: PatientData.User.FirstName + " " + PatientData.User.LastName,
                    Side = PatientData.Patient.Side,
                    CoMorbity = PatientData.Patient.CoMorbity,
                    Diagnosis = PatientData.Patient.Daignosis,
                    CaseSummary = Dischargedata == null ? "" : Dischargedata.CaseSummary,
                    Investigations = Dischargedata == null ? "" : Dischargedata.Investigations,
                    TreatmentGiven = Dischargedata == null ? "" : Dischargedata.TreatmentGiven,
                    AdviceOndischarge = Dischargedata == null ? "" : Dischargedata.AdviceOndischarge
                };
                return DischargePrintdata;

            }
            return null;
        }

        public long AddPatient(PatientModel _model, CancellationToken cancellationToken)
        {
            var _addressId = AddAddress(_model);
            var _patientId = AddPatient(_model, _addressId);
            return _patientId;
        }

        public long AddDiagnosis(PatientModel _model, long patientID)
        {

            var data = _context.Patient.FirstOrDefault(a => a.PatientID == patientID);
            if (data != null)
            {
                data.Daignosis = _model.Daignosis;
                data.Side = _model.Side;
                data.CoMorbity = _model.CoMorbity;
                _context.SaveChanges();
            }

            return data.PatientID;
        }

        public bool AddInvestigation(InvestigationModel _model, CancellationToken cancellationToken, long _patientId)
        {
            // var tempData = MyDataTable.tempTable;
            var patientId = AddInvestigationData(_model, _patientId);
            if (patientId > 0) { return true; }
            return false;
        }

        public bool AddInvestigationImages(InvestigationImagesModel _model, long investigationId, long _patientId)
        {
            var result = AddInvestigationImage(_model, investigationId, _patientId);
            return true;
        }

        public async Task UpdatePatient(PatientModel _model, CancellationToken cancellationToken)
        {
            var tempData = MyDataTable.tempTable;
            PatientModel patient1 = new PatientModel();
            var _patientExists = _context.Patient.FirstOrDefault(a => a.PatientID == _model.PatientID);
            if (_patientExists != null)
            {
                if (!string.IsNullOrEmpty(_model.Name))
                {
                    _patientExists.Name = _model.Name;
                }
                if (!string.IsNullOrEmpty(_model.SerialNumber))
                {
                    _patientExists.SerialNumber = _model.SerialNumber;
                }
                if (!string.IsNullOrEmpty(_model.Gender))
                {
                    _patientExists.Gender = _model.Gender;
                }
                if (!string.IsNullOrEmpty(_model.Age))
                {
                    _patientExists.Age = _model.Age;
                }
                if (_model.DOA != null)
                {
                    _patientExists.DOA = Convert.ToDateTime(_model.DOA);
                }
                if (!string.IsNullOrEmpty(_model.PhoneNumber))
                {
                    _patientExists.PhoneNumber = _model.PhoneNumber;
                }
                if (!string.IsNullOrEmpty(_model.AlternateNumber))
                {
                    _patientExists.AlternateNumber = _model.AlternateNumber;
                }
                if (!string.IsNullOrEmpty(_model.CADSNumber))
                {
                    _patientExists.CADSNumber = _model.CADSNumber;
                }
                if (!string.IsNullOrEmpty(_model.OPDNumber))
                {
                    _patientExists.OPDNumber = _model.OPDNumber;
                }

                if (!string.IsNullOrEmpty(_model.SeniorResident))
                {
                    _patientExists.SeniorResident = _model.SeniorResident;
                }

                if (!string.IsNullOrEmpty(_model.JuniorResident))
                {
                    _patientExists.JuniorResident = _model.JuniorResident;
                }
                if (!string.IsNullOrEmpty(_model.OtherO))
                {
                    _patientExists.OtherO = _model.OtherO;
                }
                if (!string.IsNullOrEmpty(_model.OtherT))
                {
                    _patientExists.OtherT = _model.OtherT;
                }
                if (!string.IsNullOrEmpty(_model.OtherTh))
                {
                    _patientExists.OtherTh = _model.OtherTh;
                }
                if (!string.IsNullOrEmpty(_model.Daignosis))
                {
                    _patientExists.Daignosis = _model.Daignosis;
                }
                if (!string.IsNullOrEmpty(_model.Side))
                {
                    _patientExists.Side = _model.Side;
                }
                if (!string.IsNullOrEmpty(_model.CoMorbity))
                {
                    _patientExists.CoMorbity = _model.CoMorbity;
                }
                if (!string.IsNullOrWhiteSpace(_model.Dr_ID))
                {
                    _patientExists.DrId = _model.Dr_ID;
                }
                _patientExists.UpdateBy = _patientExists.Name;
                _patientExists.UpdatedOn = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);
                //address...
                await UpdateAddress(_model, cancellationToken);
                // await UpdateInvestigationImages(_model, cancellationToken);

            }
        }

        public async Task UpdateInvestigationData(InvestigationModel _model, CancellationToken cancellationToken)
        {
            if (_model != null)
            {
                try
                {
                    //Investigation Model = new Investigation();
                    var Model = await _context.Investigation.FirstOrDefaultAsync(a => a.Id == _model.Id);
                    if (Model == null) { return; }
                    Model.Id = _model.Id;
                    Model.PatientID = _model.PatientID;
                    if (!string.IsNullOrEmpty(_model.Day))
                    {
                        Model.Day = _model.Day;
                    }
                    //CBC
                    if (!string.IsNullOrEmpty(_model.HB))
                    {
                        Model.HB = _model.HB;
                    }
                    if (!string.IsNullOrEmpty(_model.TLC))
                    {
                        Model.TLC = _model.TLC;
                    }
                    if (!string.IsNullOrEmpty(_model.PLT))
                    {
                        Model.PLT = _model.PLT;
                    }

                    //RFT
                    if (!string.IsNullOrEmpty(_model.SGeat))
                    {
                        Model.SGeat = _model.SGeat;
                    }
                    if (!string.IsNullOrEmpty(_model.BUN))
                    {
                        Model.BUN = _model.BUN;
                    }

                    //BLOOD SUGAR
                    if (!string.IsNullOrEmpty(_model.Fasting))
                    {
                        Model.Fasting = _model.Fasting;
                    }
                    if (!string.IsNullOrEmpty(_model.PP))
                    {
                        Model.PP = _model.PP;
                    }
                    if (!string.IsNullOrEmpty(_model.Random))
                    {
                        Model.Random = _model.Random;
                    }

                    //LFT

                    if (!string.IsNullOrEmpty(_model.TotalBil))
                    {
                        Model.TotalBil = _model.TotalBil;
                    }
                    if (!string.IsNullOrEmpty(_model.DirectBil))
                    {
                        Model.DirectBil = _model.DirectBil;
                    }
                    if (!string.IsNullOrEmpty(_model.AlkPhosphate))
                    {
                        Model.AlkPhosphate = _model.AlkPhosphate;
                    }
                    if (!string.IsNullOrEmpty(_model.SGDT))
                    {
                        Model.SGDT = _model.SGDT;
                    }
                    if (!string.IsNullOrEmpty(_model.SGPT))
                    {
                        Model.SGPT = _model.SGPT;
                    }

                    //TFT
                    if (!string.IsNullOrEmpty(_model.T3))
                    {
                        Model.T3 = _model.T3;
                    }
                    if (!string.IsNullOrEmpty(_model.T4))
                    {
                        Model.T4 = _model.T4;
                    }
                    if (!string.IsNullOrEmpty(_model.TSH))
                    {
                        Model.TSH = _model.TSH;
                    }
                    if (!string.IsNullOrEmpty(_model.FT3))
                    {
                        Model.FT3 = _model.FT3;
                    }
                    if (!string.IsNullOrEmpty(_model.FT4))
                    {
                        Model.FT4 = _model.FT4;
                    }

                    //SERUM ELECTROLYTES
                    if (!string.IsNullOrEmpty(_model.Sodium))
                    {
                        Model.Sodium = _model.Sodium;
                    }
                    if (!string.IsNullOrEmpty(_model.Potassium))
                    {
                        Model.Potassium = _model.Potassium;
                    }
                    if (!string.IsNullOrEmpty(_model.Calcium))
                    {
                        Model.Calcium = _model.Calcium;
                    }

                    //PT-INR IMG
                    if (!string.IsNullOrEmpty(_model.PT))
                    {
                        Model.PT = _model.PT;
                    }
                    if (!string.IsNullOrEmpty(_model.INR))
                    {
                        Model.INR = _model.INR;
                    }

                    //LIPID PROFILE
                    if (!string.IsNullOrEmpty(_model.Cholesterol))
                    {
                        Model.Cholesterol = _model.Cholesterol;
                    }
                    if (!string.IsNullOrEmpty(_model.Triglyceride))
                    {
                        Model.Triglyceride = _model.Triglyceride;
                    }
                    if (!string.IsNullOrEmpty(_model.HDL))
                    {
                        Model.HDL = _model.HDL;
                    }
                    if (!string.IsNullOrEmpty(_model.LDL))
                    {
                        Model.LDL = _model.LDL;
                    }

                    //URINE RM IMG
                    if (!string.IsNullOrEmpty(_model.Blood))
                    {
                        Model.Blood = _model.Blood;
                    }
                    if (!string.IsNullOrEmpty(_model.PusCell))
                    {
                        Model.PusCell = _model.PusCell;
                    }
                    if (!string.IsNullOrEmpty(_model.EpithelialCell))
                    {
                        Model.EpithelialCell = _model.EpithelialCell;
                    }
                    if (!string.IsNullOrEmpty(_model.Crystals))
                    {
                        Model.Crystals = _model.Crystals;
                    }
                    if (!string.IsNullOrEmpty(_model.Sugar))
                    {
                        Model.Sugar = _model.Sugar;
                    }
                    if (!string.IsNullOrEmpty(_model.Color))
                    {
                        Model.Color = _model.Color;
                    }
                    if (!string.IsNullOrEmpty(_model.Appearance))
                    {
                        Model.Appearance = _model.Appearance;
                    }
                    if (!string.IsNullOrEmpty(_model.Albumin))
                    {
                        Model.Albumin = _model.Albumin;
                    }

                    if (!string.IsNullOrEmpty(_model.ABG))
                    {
                        Model.ABG = _model.ABG;
                    }

                    if (!string.IsNullOrEmpty(_model.USG))
                    {
                        Model.USG = _model.USG;
                    }
                    if (!string.IsNullOrEmpty(_model.SONOMMOGRAPHY))
                    {
                        Model.SONOMMOGRAPHY = _model.SONOMMOGRAPHY;
                    }
                    if (!string.IsNullOrEmpty(_model.CECT))
                    {
                        Model.CECT = _model.CECT;
                    }
                    if (!string.IsNullOrEmpty(_model.MRI))
                    {
                        Model.MRI = _model.MRI;
                    }
                    if (!string.IsNullOrEmpty(_model.FNAC))
                    {
                        Model.FNAC = _model.FNAC;
                    }
                    if (!string.IsNullOrEmpty(_model.TrucutBiopsy))
                    {
                        Model.TrucutBiopsy = _model.TrucutBiopsy;
                    }
                    if (!string.IsNullOrEmpty(_model.ReceptorStatus))
                    {
                        Model.ReceptorStatus = _model.ReceptorStatus;
                    }
                    if (!string.IsNullOrEmpty(_model.MRCP))
                    {
                        Model.MRCP = _model.MRCP;
                    }

                    if (!string.IsNullOrEmpty(_model.ERCP))
                    {
                        Model.ERCP = _model.ERCP;
                    }

                    if (!string.IsNullOrEmpty(_model.EndoscopyUpperGI))
                    {
                        Model.EndoscopyUpperGI = _model.EndoscopyUpperGI;
                    }
                    if (!string.IsNullOrEmpty(_model.EndoscopyLowerGI))
                    {
                        Model.EndoscopyLowerGI = _model.EndoscopyLowerGI;
                    }
                    if (!string.IsNullOrEmpty(_model.PETCT))
                    {
                        Model.PETCT = _model.PETCT;
                    }
                    if (!string.IsNullOrEmpty(_model.TumorMarkers))
                    {
                        Model.TumorMarkers = _model.TumorMarkers;
                    }
                    if (!string.IsNullOrEmpty(_model.IVP))
                    {
                        Model.IVP = _model.IVP;
                    }
                    if (!string.IsNullOrEmpty(_model.MCU))
                    {
                        Model.MCU = _model.MCU;
                    }
                    if (!string.IsNullOrEmpty(_model.RGU))
                    {
                        Model.RGU = _model.RGU;
                    }

                    if (!string.IsNullOrEmpty(_model.OtherO))
                    {
                        Model.OtherO = _model.OtherO;
                    }

                    if (!string.IsNullOrEmpty(_model.OtherT))
                    {
                        Model.OtherT = _model.OtherT; // Serum Amylase 
                    }

                    if (!string.IsNullOrEmpty(_model.OtherTh))
                    {
                        Model.OtherTh = _model.OtherTh; // Serum lipase
                    }

                    Model.UpdatedOn = DateTime.Now;
                    Model.UpdateBy = _model.PatientID.ToString();

                    _context.Investigation.Update(Model);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception e)
                {

                    throw;
                }


            };
            // return ModelList[0].Id;
        }

        private long AddAddress(PatientModel _address)
        {
            if (_address != null)
            {
                var address = new Address
                {
                    Street = _address.Street,
                    City = _address.City,
                    State = _address.State,
                    ZipCode = _address.ZipCode,
                };
                _context.Address.Add(address);
                _context.SaveChanges();
                return address.ID;
            }
            return 0;

        }

        private async Task UpdateAddress(PatientModel _model, CancellationToken cancellationToken)
        {
            var _addressExists = _context.Address.FirstOrDefault(a => a.ID == _model.Address_ID);
            if (!string.IsNullOrEmpty(_model.Street))
            {
                _addressExists.Street = _model.Street;
            }
            if (!string.IsNullOrEmpty(_model.City))
            {
                _addressExists.City = _model.City;
            }
            if (!string.IsNullOrEmpty(_model.State))
            {
                _addressExists.State = _model.State;
            }
            if (!string.IsNullOrEmpty(_model.ZipCode))
            {
                _addressExists.ZipCode = _model.ZipCode;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        private long AddPatient(PatientModel _patient, long _addressId)
        {
            if (_patient != null)
            {
                var patient = new Patient
                {
                    SerialNumber = _patient.SerialNumber,
                    DrId = _patient.Dr_ID,
                    CADSNumber = _patient.CADSNumber,
                    OPDNumber = _patient.OPDNumber,
                    Name = _patient.Name,
                    Age = _patient.Age,
                    Gender = _patient.Gender,
                    DOA = _patient.DOA ?? DateTime.UtcNow,
                    Address_ID = _addressId,
                    PhoneNumber = _patient.PhoneNumber,
                    AlternateNumber = _patient.AlternateNumber,
                    SeniorResident = _patient.SeniorResident,
                    JuniorResident = _patient.JuniorResident,

                    Daignosis = _patient.Daignosis,
                    Side = _patient.Side,
                    CoMorbity = _patient.CoMorbity,

                    OtherO = _patient.OtherO,
                    OtherT = _patient.OtherT,
                    OtherTh = _patient.OtherTh,
                    CreatedBy = _patient.Name,
                    UpdateBy = _patient.Name,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = true,
                    Status = "Admitted"
                };
                _context.Patient.Add(patient);
                _context.SaveChanges();
                return patient.PatientID;
            }
            return 0;
        }

        public long AddInvestigationData(InvestigationModel investigationData, long PatientID)
        {
            if (investigationData != null && PatientID > 0)
            {
                Investigation Model = new Investigation()
                {
                    PatientID = PatientID,
                    Day = investigationData.Day,

                    //CBC
                    HB = investigationData.HB,
                    TLC = investigationData.TLC,
                    PLT = investigationData.PLT,

                    //RFT
                    SGeat = investigationData.SGeat,
                    BUN = investigationData.BUN,

                    //BLOOD SUGAR
                    Fasting = investigationData.Fasting,
                    PP = investigationData.PP,
                    Random = investigationData.Random,

                    //LFT
                    TotalBil = investigationData.TotalBil,
                    DirectBil = investigationData.DirectBil,
                    AlkPhosphate = investigationData.AlkPhosphate,
                    SGDT = investigationData.SGDT,
                    SGPT = investigationData.SGPT,
                    OtherT=investigationData.OtherT, // Serum Amylase 
                    OtherTh =investigationData.OtherTh, // Serum lipase

                    //TFT
                    T3 = investigationData.T3,
                    T4 = investigationData.T4,
                    TSH = investigationData.TSH,
                    FT3 = investigationData.FT3,
                    FT4 = investigationData.FT4,

                    //SERUM ELECTROLYTES
                    Sodium = investigationData.Sodium,
                    Potassium = investigationData.Potassium,
                    Calcium = investigationData.Calcium,

                    //PT-INR IMG
                    PT = investigationData.PT,
                    INR = investigationData.INR,

                    //LIPID PROFILE
                    Cholesterol = investigationData.Cholesterol,
                    Triglyceride = investigationData.Triglyceride,
                    HDL = investigationData.HDL,
                    LDL = investigationData.LDL,

                    //URINE RM IMG
                    Blood = investigationData.Blood,
                    PusCell = investigationData.PusCell,
                    EpithelialCell = investigationData.EpithelialCell,
                    Crystals = investigationData.Crystals,
                    Sugar = investigationData.Sugar,
                    Color = investigationData.Color,
                    Appearance = investigationData.Appearance,
                    Albumin = investigationData.Albumin,

                    ABG = investigationData.ABG,

                    USG = investigationData.USG,
                    SONOMMOGRAPHY = investigationData.SONOMMOGRAPHY,
                    CECT = investigationData.CECT,
                    MRI = investigationData.MRI,
                    FNAC = investigationData.FNAC,
                    TrucutBiopsy = investigationData.TrucutBiopsy,
                    ReceptorStatus = investigationData.ReceptorStatus,
                    MRCP = investigationData.MRCP,
                    ERCP = investigationData.ERCP,
                    EndoscopyUpperGI = investigationData.EndoscopyUpperGI,
                    EndoscopyLowerGI = investigationData.EndoscopyLowerGI,
                    PETCT = investigationData.PETCT,
                    TumorMarkers = investigationData.TumorMarkers,
                    IVP = investigationData.IVP,
                    MCU = investigationData.MCU,
                    RGU = investigationData.RGU,
                    OtherO = investigationData.OtherO,
                    

                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                    CreatedBy = PatientID.ToString(),
                    UpdateBy = "",
                    IsActive = true
                };

                _context.Investigation.AddAsync(Model);
                _context.SaveChanges();
                return Model.Id;
            }
            return 0;
        }

        private bool AddInvestigationImage(InvestigationImagesModel imgData, long InvestigationID, long PatientID)
        {
            if (imgData != null)
            {
                var fileNames = SaveImages(imgData);
                int maxImagesToInsert = 0;
                foreach (var property in typeof(InvestigationImages).GetProperties())
                {
                    if (fileNames.ContainsKey(property.Name))
                    {
                        var fileList = fileNames[property.Name];
                        maxImagesToInsert = Math.Min(fileList.Count, 5);
                    }
                }

                for (int i = 0; i < maxImagesToInsert; i++)
                {
                    InvestigationImages InvImgTbl = new InvestigationImages();
                    InvImgTbl.InvestigationID = InvestigationID;
                    InvImgTbl.PatientId = Convert.ToInt32(PatientID);

                    if (fileNames.ContainsKey("BloodSugar_img") && fileNames["BloodSugar_img"].Count > i)
                    {
                        InvImgTbl.BloodSugar_img = fileNames["BloodSugar_img"][i];
                    }
                    if (fileNames.ContainsKey("TFT_img") && fileNames["TFT_img"].Count > i)
                    {
                        InvImgTbl.TFT_img = fileNames["TFT_img"][i];
                    }
                    if (fileNames.ContainsKey("USG_img") && fileNames["USG_img"].Count > i)
                    {
                        InvImgTbl.USG_img = fileNames["USG_img"][i];
                    }
                    if (fileNames.ContainsKey("SONOMMOGRAPHY_img") && fileNames["SONOMMOGRAPHY_img"].Count > i)
                    {
                        InvImgTbl.SONOMMOGRAPHY_img = fileNames["SONOMMOGRAPHY_img"][i];
                    }
                    if (fileNames.ContainsKey("CECT_img") && fileNames["CECT_img"].Count > i)
                    {
                        InvImgTbl.CECT_img = fileNames["CECT_img"][i];
                    }
                    if (fileNames.ContainsKey("MRI_img") && fileNames["MRI_img"].Count > i)
                    {
                        InvImgTbl.MRI_img = fileNames["MRI_img"][i];
                    }
                    if (fileNames.ContainsKey("FNAC_img") && fileNames["FNAC_img"].Count > i)
                    {
                        InvImgTbl.FNAC_img = fileNames["FNAC_img"][i];
                    }
                    if (fileNames.ContainsKey("TrucutBiopsy_img") && fileNames["TrucutBiopsy_img"].Count > i)
                    {
                        InvImgTbl.TrucutBiopsy_img = fileNames["TrucutBiopsy_img"][i];
                    }
                    if (fileNames.ContainsKey("ReceptorStatus_img") && fileNames["ReceptorStatus_img"].Count > i)
                    {
                        InvImgTbl.ReceptorStatus_img = fileNames["ReceptorStatus_img"][i];
                    }
                    if (fileNames.ContainsKey("MRCP_img") && fileNames["MRCP_img"].Count > i)
                    {
                        InvImgTbl.MRCP_img = fileNames["MRCP_img"][i];
                    }

                    if (fileNames.ContainsKey("ERCP_img") && fileNames["ERCP_img"].Count > i)
                    {
                        InvImgTbl.ERCP_img = fileNames["ERCP_img"][i];
                    }
                    if (fileNames.ContainsKey("EndoscopyUpperGI_img") && fileNames["EndoscopyUpperGI_img"].Count > i)
                    {
                        InvImgTbl.EndoscopyUpperGI_img = fileNames["EndoscopyUpperGI_img"][i];
                    }
                    if (fileNames.ContainsKey("EndoscopyLowerGI_img") && fileNames["EndoscopyLowerGI_img"].Count > i)
                    {
                        InvImgTbl.EndoscopyLowerGI_img = fileNames["EndoscopyLowerGI_img"][i];
                    }
                    if (fileNames.ContainsKey("PETCT_img") && fileNames["PETCT_img"].Count > i)
                    {
                        InvImgTbl.PETCT_img = fileNames["PETCT_img"][i];
                    }
                    if (fileNames.ContainsKey("TumorMarkers_img") && fileNames["TumorMarkers_img"].Count > i)
                    {
                        InvImgTbl.TumorMarkers_img = fileNames["TumorMarkers_img"][i];
                    }
                    if (fileNames.ContainsKey("IVP_img") && fileNames["IVP_img"].Count > i)
                    {
                        InvImgTbl.IVP_img = fileNames["IVP_img"][i];
                    }
                    if (fileNames.ContainsKey("MCU_img") && fileNames["MCU_img"].Count > i)
                    {
                        InvImgTbl.MCU_img = fileNames["MCU_img"][i];
                    }
                    if (fileNames.ContainsKey("RGU_img") && fileNames["RGU_img"].Count > i)
                    {
                        InvImgTbl.RGU_img = fileNames["RGU_img"][i]; /*Cystoscopy*/
                    }
                    if (fileNames.ContainsKey("ABG_img") && fileNames["ABG_img"].Count > i)
                    {
                        InvImgTbl.ABG_img = fileNames["ABG_img"][i];
                    }
                    if (fileNames.ContainsKey("OtherO") && fileNames["OtherO"].Count > i)
                    {
                        InvImgTbl.OtherO = fileNames["OtherO"][i];
                    }
                    if (fileNames.ContainsKey("CBC_img") && fileNames["CBC_img"].Count > i)
                    {
                        InvImgTbl.CBC_img = fileNames["CBC_img"][i];
                    }
                    if (fileNames.ContainsKey("RFT_img") && fileNames["RFT_img"].Count > i)
                    {
                        InvImgTbl.RFT_img = fileNames["RFT_img"][i];
                    }

                    if (fileNames.ContainsKey("PTINR_img") && fileNames["PTINR_img"].Count > i)
                    {
                        InvImgTbl.PTINR_img = fileNames["PTINR_img"][i];
                    }
                    if (fileNames.ContainsKey("LFT_img") && fileNames["LFT_img"].Count > i)
                    {
                        InvImgTbl.LFT_img = fileNames["LFT_img"][i];
                    }

                    if (fileNames.ContainsKey("LIPIDPROFILE_img") && fileNames["LIPIDPROFILE_img"].Count > i)
                    {
                        InvImgTbl.LIPIDPROFILE_img = fileNames["LIPIDPROFILE_img"][i];
                    }

                    if (fileNames.ContainsKey("UrineRM_img") && fileNames["UrineRM_img"].Count > i)
                    {
                        InvImgTbl.UrineRM_img = fileNames["UrineRM_img"][i];
                    }

                    _context.InvestigationImages.Add(InvImgTbl);
                    _context.SaveChanges();
                }

                // return InvImgTbl.Id;
                return true;
            }
            return false;
        }

        public async Task UpdateInvestigationImages(InvestigationImagesModel _model, CancellationToken cancellationToken)
        {
            var _imgExists = await _context.InvestigationImages.Where(a => a.PatientId == _model.PatientId && a.Id == _model.Id).FirstOrDefaultAsync(cancellationToken);
            if (_model != null)
            {

                var fileNames = SaveImagesforUpdate(_model);
                if (fileNames.ContainsKey("BloodSugar_img"))
                {
                    _imgExists.BloodSugar_img = fileNames["BloodSugar_img"];
                }
                if (fileNames.ContainsKey("TFT_img"))
                {
                    _imgExists.TFT_img = fileNames["TFT_img"];
                }
                if (fileNames.ContainsKey("USG_img"))
                {
                    _imgExists.USG_img = fileNames["USG_img"];
                }
                if (fileNames.ContainsKey("SONOMMOGRAPHY_img"))
                {
                    _imgExists.SONOMMOGRAPHY_img = fileNames["SONOMMOGRAPHY_img"];
                }
                if (fileNames.ContainsKey("CECT_img"))
                {
                    _imgExists.CECT_img = fileNames["CECT_img"];
                }

                if (fileNames.ContainsKey("MRI_img"))
                {
                    _imgExists.MRI_img = fileNames["MRI_img"];
                }
                if (fileNames.ContainsKey("FNAC_img"))
                {
                    _imgExists.FNAC_img = fileNames["FNAC_img"];
                }
                if (fileNames.ContainsKey("TrucutBiopsy_img"))
                {
                    _imgExists.TrucutBiopsy_img = fileNames["TrucutBiopsy_img"];
                }
                if (fileNames.ContainsKey("ReceptorStatus_img"))
                {
                    _imgExists.ReceptorStatus_img = fileNames["ReceptorStatus_img"];
                }
                if (fileNames.ContainsKey("MRCP_img"))
                {
                    _imgExists.MRCP_img = fileNames["MRCP_img"];
                }
                if (fileNames.ContainsKey("ERCP_img"))
                {
                    _imgExists.ERCP_img = fileNames["ERCP_img"];
                }
                if (fileNames.ContainsKey("EndoscopyUpperGI_img"))
                {
                    _imgExists.EndoscopyUpperGI_img = fileNames["EndoscopyUpperGI_img"];
                }
                if (fileNames.ContainsKey("EndoscopyLowerGI_img"))
                {
                    _imgExists.EndoscopyLowerGI_img = fileNames["EndoscopyLowerGI_img"];
                }
                if (fileNames.ContainsKey("PETCT_img"))
                {
                    _imgExists.PETCT_img = fileNames["PETCT_img"];
                }
                if (fileNames.ContainsKey("TumorMarkers_img"))
                {
                    _imgExists.TumorMarkers_img = fileNames["TumorMarkers_img"];
                }
                if (fileNames.ContainsKey("IVP_img"))
                {
                    _imgExists.IVP_img = fileNames["IVP_img"];
                }
                if (fileNames.ContainsKey("MCU_img"))
                {
                    _imgExists.MCU_img = fileNames["MCU_img"];
                }
                if (fileNames.ContainsKey("RGU_img"))
                {
                    _imgExists.RGU_img = fileNames["RGU_img"]; /*Cystoscopy*/
                }

                if (fileNames.ContainsKey("ABG_img"))
                {
                    _imgExists.ABG_img = fileNames["ABG_img"];
                }
                if (fileNames.ContainsKey("OtherO"))
                {
                    _imgExists.OtherO = fileNames["OtherO"];
                }
                if (fileNames.ContainsKey("CBC_img"))
                {
                    _imgExists.CBC_img = fileNames["CBC_img"];
                }
                if (fileNames.ContainsKey("RFT_img"))
                {
                    _imgExists.RFT_img = fileNames["RFT_img"];
                }
                if (fileNames.ContainsKey("PTINR_img"))
                {
                    _imgExists.PTINR_img = fileNames["PTINR_img"];
                }
                if (fileNames.ContainsKey("LFT_img"))
                {
                    _imgExists.LFT_img = fileNames["LFT_img"];
                }
                if (fileNames.ContainsKey("PETCT_img"))
                {
                    _imgExists.PETCT_img = fileNames["PETCT_img"];
                }

                if (fileNames.ContainsKey("LIPIDPROFILE_img"))
                {
                    _imgExists.LIPIDPROFILE_img = fileNames["LIPIDPROFILE_img"];
                }
                if (fileNames.ContainsKey("UrineRM_img"))
                {
                    _imgExists.UrineRM_img = fileNames["UrineRM_img"];
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }

        public Dictionary<string, string> SaveImagesforUpdate(InvestigationImagesModel imgData)
        {

            //var images = new List<IFormFile> { imgData.CBC_img, imgData.RFT_img, imgData.BloodSugar_img, imgData.SerumElectrolytes_img, imgData.LipidProfile_img, imgData.USG_img, imgData.CECT_img, imgData.MRI_img, imgData.FNAC_img, imgData.TrucutBiopsy_img, imgData.ReceptorStatus_img, imgData.MRCP_img, imgData.ERCP_img, imgData.EndoscopyUpperGI_img, imgData.EndoscopyLowerGI_img, imgData.PETCT_img, imgData.TumorMarkers_img, imgData.SONOMMOGRAPHY_img, imgData.LFT_img, imgData.TSPAG_img, imgData.TFT_img, imgData.IVP_img, imgData.MCU_img, imgData.RGU_img };

            var images = new List<IFormFile>();

            var properties = typeof(InvestigationImagesModel).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(List<IFormFile>))
                {
                    var propertyValue = (List<IFormFile>)property.GetValue(imgData);
                    if (propertyValue != null)
                        images.AddRange(propertyValue);
                }
            }

            var folderName = "Images_Data"; // Change this to the desired folder name

            List<string> fileNames1 = new List<string>();
            Dictionary<string, string> fileNames = new Dictionary<string, string>();
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            try
            {

                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {

                        IFormFile img = image;
                        var fileName = Path.GetFileName(image.FileName);
                        fileName = Guid.NewGuid() + "_" + fileName;
                        var filePath = Path.Combine(directoryPath, fileName);
                        fileNames.Add(image.Name, fileName);

                        using FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate);
                        img.CopyTo(stream);
                        stream.Close();
                    }
                }
            }
            catch (Exception e)
            {

                return fileNames;
            }

            return fileNames;
        }

        public Dictionary<string, List<string>> SaveImages(InvestigationImagesModel imgData)
        {

            //var images = new List<IFormFile> { imgData.CBC_img, imgData.RFT_img, imgData.BloodSugar_img, imgData.SerumElectrolytes_img, imgData.LipidProfile_img, imgData.USG_img, imgData.CECT_img, imgData.MRI_img, imgData.FNAC_img, imgData.TrucutBiopsy_img, imgData.ReceptorStatus_img, imgData.MRCP_img, imgData.ERCP_img, imgData.EndoscopyUpperGI_img, imgData.EndoscopyLowerGI_img, imgData.PETCT_img, imgData.TumorMarkers_img, imgData.SONOMMOGRAPHY_img, imgData.LFT_img, imgData.TSPAG_img, imgData.TFT_img, imgData.IVP_img, imgData.MCU_img, imgData.RGU_img };

            var images = new List<IFormFile>();

            var properties = typeof(InvestigationImagesModel).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(List<IFormFile>))
                {
                    var propertyValue = (List<IFormFile>)property.GetValue(imgData);
                    if (propertyValue != null)
                        images.AddRange(propertyValue);
                }
            }

            var folderName = "Images_Data"; // Change this to the desired folder name

            List<string> fileNames1 = new List<string>();
            Dictionary<string, List<string>> fileNames = new Dictionary<string, List<string>>();
            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            try
            {
                int ct = 0;
                foreach (var image in images)
                {
                    ct++;
                    if (image != null && image.Length > 0)
                    {

                        IFormFile img = image;
                        var fileName = Path.GetFileName(image.FileName);
                        fileName = Guid.NewGuid() + "_" + fileName;
                        var filePath = Path.Combine(directoryPath, fileName);
                        //fileNames.Add(image.Name+"_"+ ct, fileName);
                        if (!fileNames.ContainsKey(image.Name))
                        {
                            fileNames.Add(image.Name, new List<string>());
                        }
                        // Add the file name to the list
                        fileNames[image.Name].Add(fileName);
                        using FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate);
                        img.CopyTo(stream);
                        stream.Close();
                    }
                }
            }
            catch (Exception e)
            {

                return fileNames;
            }

            return fileNames;
        }

        public DataTable addTempData(List<InvestigationModel> model)
        {
            return dataTable.GetTempTable(model);
        }

        public bool RemoveInvestigation(int InvestigationID)
        {
            var data = _context.Investigation.FirstOrDefault(a => a.Id == InvestigationID);
            //// If the row exists, remove it
            if (data != null)
            {
                _context.Investigation.Remove(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void addTempData_img(InvestigationImagesModel model)
        {
            imageFileList.Add(model);

        }

        public async Task<bool> DeletePatient(long id, CancellationToken cancellationToken)
        {
            if (id > 0)
            {
                var data = await _context.Patient.FirstOrDefaultAsync(a => a.PatientID == id, cancellationToken);
                if (data != null)
                {
                    // 1=Delete and 0 = Active
                    data.IsActive = false;
                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            return false;
        }

        public long AddInvestigationImages(PatientViewModel patient, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public long AddCaseSheet(CaseSheetModel _model, long patientid)
        {
            if (patientid > 0)
            {
                var caseSheet = new CaseSheet
                {
                    PatientID = patientid,
                    PRESENTING_COMPLAINTS = _model.PRESENTING_COMPLAINTS,
                    HistoryOfPresentingIllness = _model.HistoryOfPresentingIllness,
                    PastHistory = _model.PastHistory,
                    PersonalHistory = _model.PersonalHistory,
                    Diet = _model.Diet,
                    Appetite = _model.Appetite,
                    Sleep = _model.Sleep,
                    Bowel = _model.Bowel,
                    Bladder = _model.Bladder,
                    Addiction = _model.Addiction,
                    FamilyHistory = _model.FamilyHistory,
                    // Vitals 
                    BP = _model.BP,
                    PR = _model.PR,
                    RR = _model.RR,
                    Temp = _model.Temp,
                    SpO2 = _model.SpO2,
                    //GENERAL EXAMINATION
                    Pallor = _model.Pallor,
                    Icterus = _model.Icterus,
                    Cyanosis = _model.Cyanosis,
                    Clubbing = _model.Clubbing,
                    Edema = _model.Edema,
                    Lymphadenopathy = _model.Lymphadenopathy,
                    //SYSTEMIC EXAMINATION
                    RespiratorySystem = _model.RespiratorySystem,
                    CNS = _model.CNS,
                    CVS = _model.CVS,
                    PerAbdomen = _model.PerAbdomen,
                    LocoregionalExam = _model.LocoregionalExam,

                    Remark = _model.Remark,
                    Value1 = _model.AddImage, //this used for addimges
                    Value2 = _model.Value2,
                    Value3 = _model.Value3,
                };
                _context.CaseSheet.Add(caseSheet);
                _context.SaveChanges();
                return caseSheet.Id;
            }
            return 0;
        }

        public async Task<bool> UpdateCaseSheet(CaseSheetModel _model)
        {
            if (_model.Id > 0)
            {
                var data = _context.CaseSheet.Where(a => a.Id == _model.Id && a.PatientID == _model.PatientID).FirstOrDefault();
                data.Id = _model.Id;
                data.PatientID = _model.PatientID;
                if (!string.IsNullOrEmpty(_model.PRESENTING_COMPLAINTS))
                {
                    data.PRESENTING_COMPLAINTS = _model.PRESENTING_COMPLAINTS;
                }

                if (!string.IsNullOrEmpty(_model.HistoryOfPresentingIllness))
                {
                    data.HistoryOfPresentingIllness = _model.HistoryOfPresentingIllness;
                }

                if (!string.IsNullOrEmpty(_model.PastHistory))
                {
                    data.PastHistory = _model.PastHistory;
                }

                if (!string.IsNullOrEmpty(_model.PersonalHistory))
                {
                    data.PersonalHistory = _model.PersonalHistory;
                }

                if (!string.IsNullOrEmpty(_model.Diet))
                {
                    data.Diet = _model.Diet;
                }

                if (!string.IsNullOrEmpty(_model.Appetite))
                {
                    data.Appetite = _model.Appetite;
                }

                if (!string.IsNullOrEmpty(_model.Sleep))
                {
                    data.Sleep = _model.Sleep;
                }

                if (!string.IsNullOrEmpty(_model.Bowel))
                {
                    data.Bowel = _model.Bowel;
                }

                if (!string.IsNullOrEmpty(_model.Bladder))
                {
                    data.Bladder = _model.Bladder;
                }

                if (!string.IsNullOrEmpty(_model.Addiction))
                {
                    data.Addiction = _model.Addiction;
                }

                if (!string.IsNullOrEmpty(_model.FamilyHistory))
                {
                    data.FamilyHistory = _model.FamilyHistory;
                }

                //Vitals

                if (!string.IsNullOrEmpty(_model.BP))
                {
                    data.BP = _model.BP;
                }

                if (!string.IsNullOrEmpty(_model.PR))
                {
                    data.PR = _model.PR;
                }

                if (!string.IsNullOrEmpty(_model.RR))
                {
                    data.RR = _model.RR;
                }

                if (!string.IsNullOrEmpty(_model.Temp))
                {
                    data.Temp = _model.Temp;
                }

                if (!string.IsNullOrEmpty(_model.SpO2))
                {
                    data.SpO2 = _model.SpO2;
                }

                //GENERAL EXAMINATION

                if (!string.IsNullOrEmpty(_model.Pallor))
                {
                    data.Pallor = _model.Pallor;
                }
                if (!string.IsNullOrEmpty(_model.Icterus))
                {
                    data.Icterus = _model.Icterus;
                }
                if (!string.IsNullOrEmpty(_model.Cyanosis))
                {
                    data.Cyanosis = _model.Cyanosis;
                }
                if (!string.IsNullOrEmpty(_model.Clubbing))
                {
                    data.Clubbing = _model.Clubbing;
                }
                if (!string.IsNullOrEmpty(_model.Edema))
                {
                    data.Edema = _model.Edema;
                }
                if (!string.IsNullOrEmpty(_model.Lymphadenopathy))
                {
                    data.Lymphadenopathy = _model.Lymphadenopathy;
                }

                //SYSTEMIC EXAMINATION
                if (!string.IsNullOrEmpty(_model.RespiratorySystem))
                {
                    data.RespiratorySystem = _model.RespiratorySystem;
                }
                if (!string.IsNullOrEmpty(_model.CNS))
                {
                    data.CNS = _model.CNS;
                }
                if (!string.IsNullOrEmpty(_model.CVS))
                {
                    data.CVS = _model.CVS;
                }
                if (!string.IsNullOrEmpty(_model.PerAbdomen))
                {
                    data.PerAbdomen = _model.PerAbdomen;
                }
                if (!string.IsNullOrEmpty(_model.LocoregionalExam))
                {
                    data.LocoregionalExam = _model.LocoregionalExam;
                }


                if (!string.IsNullOrEmpty(_model.Remark))
                {
                    data.Remark = _model.Remark;
                }

                if (!string.IsNullOrEmpty(_model.AddImage))
                {
                    data.Value1 = _model.AddImage;
                }

                if (!string.IsNullOrEmpty(_model.Value2))
                {
                    data.Value2 = _model.Value2;
                }

                if (!string.IsNullOrEmpty(_model.Value3))
                {
                    data.Value3 = _model.Value3;
                }
                //_context.CaseSheet.Update(data);
                await _context.SaveChangesAsync();
                return true;
            };



            return false;
        }

        public long AddOperationSheet(OperationModel _model, long patientid)
        {
            if (patientid > 0)
            {
                var _operation = new Operation
                {
                    PatientID = patientid,
                    Dr_ID = _model.Dr_ID,
                    Date = _model.Date,
                    Indication = _model.Indication,
                    Anaesthetist = _model.Anaesthetist,
                    OpertingSurgeon = _model.OpertingSurgeon,
                    Position = _model.Position,
                    Anaesthesia = _model.Anaesthesia,
                    PreoperativeDiagnosis = _model.PreoperativeDiagnosis,
                    OperationTitle = _model.OperationTitle,
                    Findings = _model.Findings,
                    Duration = _model.Duration,
                    StepsOfOperation = _model.StepsOfOperation,
                    Antibiotics = _model.Antibiotics,
                    SpecimensSentFor = _model.SpecimensSentFor,
                    PostOperativeInstructions = _model.PostOperativeInstructions,
                    PerOPImage = _model.PerOPImage,
                    Value1 = _model.PerOPImage2,
                    Value2 = _model.PerOPImage3,
                    Value3 = _model.PerOPImage4,
                    Value4 = _model.PerOPImage5,

                };
                _context.Operation.Add(_operation);
                _context.SaveChanges();
                return _operation.PatientID;
            }
            return 0;
        }

        public async Task<bool> UpdateOperationSheet(OperationModel _model)
        {
            if (_model.Id > 0)
            {

                var data = await _context.Operation.Where(a => a.Id == _model.Id && a.PatientID == _model.PatientID).FirstOrDefaultAsync();
                data.PatientID = _model.PatientID;
                if (!string.IsNullOrEmpty(_model.Dr_ID))
                {
                    data.Dr_ID = _model.Dr_ID;
                }
                if (!string.IsNullOrEmpty(_model.Date))
                {
                    data.Date = _model.Date;
                }
                if (!string.IsNullOrEmpty(_model.Indication))
                {
                    data.Indication = _model.Indication;
                }

                if (!string.IsNullOrEmpty(_model.Anaesthetist))
                {
                    data.Anaesthetist = _model.Anaesthetist;
                }

                if (!string.IsNullOrEmpty(_model.OpertingSurgeon))
                {
                    data.OpertingSurgeon = _model.OpertingSurgeon;
                }
                if (!string.IsNullOrEmpty(_model.Position))
                {
                    data.Position = _model.Position;
                }
                if (!string.IsNullOrEmpty(_model.Anaesthesia))
                {
                    data.Anaesthesia = _model.Anaesthesia;
                }
                if (!string.IsNullOrEmpty(_model.PreoperativeDiagnosis))
                {
                    data.PreoperativeDiagnosis = _model.PreoperativeDiagnosis;
                }
                if (!string.IsNullOrEmpty(_model.OperationTitle))
                {
                    data.OperationTitle = _model.OperationTitle;
                }
                if (!string.IsNullOrEmpty(_model.Findings))
                {
                    data.Findings = _model.Findings;
                }
                if (!string.IsNullOrEmpty(_model.Duration))
                {
                    data.Duration = _model.Duration;
                }
                if (!string.IsNullOrEmpty(_model.StepsOfOperation))
                {
                    data.StepsOfOperation = _model.StepsOfOperation;
                }
                if (!string.IsNullOrEmpty(_model.Antibiotics))
                {
                    data.Antibiotics = _model.Antibiotics;
                }
                if (!string.IsNullOrEmpty(_model.SpecimensSentFor))
                {
                    data.SpecimensSentFor = _model.SpecimensSentFor;
                }
                if (!string.IsNullOrEmpty(_model.PostOperativeInstructions))
                {
                    data.PostOperativeInstructions = _model.PostOperativeInstructions;
                }
                if (!string.IsNullOrEmpty(_model.PerOPImage))
                {
                    data.PerOPImage = _model.PerOPImage;
                }
                if (!string.IsNullOrEmpty(_model.PerOPImage2))
                {
                    data.Value1 = _model.PerOPImage2;
                }
                if (!string.IsNullOrEmpty(_model.PerOPImage3))
                {
                    data.Value2 = _model.PerOPImage3;
                }
                if (!string.IsNullOrEmpty(_model.PerOPImage4))
                {
                    data.Value3 = _model.PerOPImage4;
                }
                if (!string.IsNullOrEmpty(_model.PerOPImage5))
                {
                    data.Value4 = _model.PerOPImage5;
                }
                // _context.Operation.Update(data);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public long AddProgress(ProgressModel _model, long patientid)
        {
            if (patientid > 0)
            {
                var _progress = new Progress
                {
                    PatientID = patientid,
                    Date = _model.Date,
                    Cc = _model.Cc,
                    GeneralCondition = _model.GeneralCondition,
                    Vitals = _model.Vitals,
                    PR = _model.PR,
                    BP = _model.BP,
                    RR = _model.RR,
                    SpO2 = _model.SpO2,
                    Temp = _model.Temp,
                    GeneralExamination = _model.GeneralExamination,
                    Urine = _model.Urine,
                    CNS = _model.CNS,
                    CVS = _model.CVS,
                    RS = _model.RS,
                    PA = _model.PA,
                    LocalExamination = _model.LocalExamination,
                    Drains = _model.Drains,
                    Advice = _model.Advice,
                    Remark = _model.Remark,
                };
                _context.Progress.Add(_progress);
                _context.SaveChanges();
                return _progress.PatientID;
            }
            return 0;
        }

        public void UpdateVital(ProgressModel _model, long PatientId)
        {

            var data = _context.Progress.Where(a => a.PatientID == PatientId).FirstOrDefault();
            if (data != null)
            {
                _model.Id = data.Id;
                _model.PatientID = PatientId;
                UpdateProgress(_model);
            }
            else
            {
                _model.PatientID = PatientId;
                AddProgress(_model, PatientId);
            }
        }

        public bool UpdateProgress(ProgressModel _model)
        {
            if (_model.Id > 0)
            {
                var data = _context.Progress.Where(a => a.Id == _model.Id && a.PatientID == _model.PatientID).FirstOrDefault();

                if (!string.IsNullOrEmpty(_model.Date))
                {
                    data.Date = _model.Date;
                }

                data.PatientID = _model.PatientID;
                if (!string.IsNullOrEmpty(_model.Date))
                {
                    data.Date = _model.Date;
                }

                if (!string.IsNullOrEmpty(_model.Cc))
                {
                    data.Cc = _model.Cc;
                }

                if (!string.IsNullOrEmpty(_model.GeneralCondition))
                {
                    data.GeneralCondition = _model.GeneralCondition;
                }

                if (!string.IsNullOrEmpty(_model.Vitals))
                {
                    data.Vitals = _model.Vitals;
                }

                if (!string.IsNullOrEmpty(_model.PR))
                {
                    data.PR = _model.PR;
                }

                if (!string.IsNullOrEmpty(_model.BP))
                {
                    data.BP = _model.BP;
                }

                if (!string.IsNullOrEmpty(_model.RR))
                {
                    data.RR = _model.RR;
                }

                if (!string.IsNullOrEmpty(_model.SpO2))
                {
                    data.SpO2 = _model.SpO2;
                }

                if (!string.IsNullOrEmpty(_model.Temp))
                {
                    data.Temp = _model.Temp;
                }

                if (!string.IsNullOrEmpty(_model.GeneralExamination))
                {
                    data.GeneralExamination = _model.GeneralExamination;
                }

                if (!string.IsNullOrEmpty(_model.Urine))
                {
                    data.Urine = _model.Urine;
                }

                if (!string.IsNullOrEmpty(_model.CNS))
                {
                    data.CNS = _model.CNS;
                }

                if (!string.IsNullOrEmpty(_model.CVS))
                {
                    data.CVS = _model.CVS;
                }

                if (!string.IsNullOrEmpty(_model.RS))
                {
                    data.RS = _model.RS;
                }

                if (!string.IsNullOrEmpty(_model.PA))
                {
                    data.PA = _model.PA;
                }

                if (!string.IsNullOrEmpty(_model.LocalExamination))
                {
                    data.LocalExamination = _model.LocalExamination;
                }

                if (!string.IsNullOrEmpty(_model.Drains))
                {
                    data.Drains = _model.Drains;
                }

                if (!string.IsNullOrEmpty(_model.Advice))
                {
                    data.Advice = _model.Advice;
                }

                if (!string.IsNullOrEmpty(_model.Remark))
                {
                    data.Remark = _model.Remark;
                }

                _context.Progress.Update(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public long AddDischarge(DischargeModel _model, long patientid)
        {
            if (_model.PatientID > 0)
            {
                var _discharge = new Discharge
                {
                    PatientID = _model.PatientID,
                    DOA = _model.DOA,
                    DOD = _model.DOD,
                    Diagnosis = _model.Diagnosis,
                    CaseSummary = _model.CaseSummary,
                    Investigations = _model.Investigations,
                    TreatmentGiven = _model.TreatmentGiven,
                    AdviceOndischarge = _model.AdviceOndischarge,
                    SeniorResident = _model.SeniorResident,
                    JuniorResident = _model.JuniorResident,
                };
                _context.Discharge.Add(_discharge);
                _context.SaveChanges();


                var data = _context.Patient.FirstOrDefault(a => a.PatientID == _model.PatientID);
                if (data != null)
                {
                    data.Status = "Discharge";
                    _context.SaveChanges();
                }

                return _discharge.PatientID;
            }
            return 0;
        }

        public bool UpdateDischarge(DischargeModel _model)
        {
            if (_model.Id > 0)
            {

                var data = _context.Discharge.Where(a => a.Id == _model.Id && a.PatientID == _model.PatientID).FirstOrDefault();

                data.PatientID = _model.PatientID;
                if (!string.IsNullOrEmpty(_model.DOA))
                {
                    data.DOA = _model.DOA;
                }

                if (!string.IsNullOrEmpty(_model.DOD))
                {
                    data.DOD = _model.DOD;
                }

                if (!string.IsNullOrEmpty(_model.Diagnosis))
                {
                    data.Diagnosis = _model.Diagnosis;
                }

                if (!string.IsNullOrEmpty(_model.CaseSummary))
                {
                    data.CaseSummary = _model.CaseSummary;
                }

                if (!string.IsNullOrEmpty(_model.Investigations))
                {
                    data.Investigations = _model.Investigations;
                }

                if (!string.IsNullOrEmpty(_model.TreatmentGiven))
                {
                    data.TreatmentGiven = _model.TreatmentGiven;
                }

                if (!string.IsNullOrEmpty(_model.AdviceOndischarge))
                {
                    data.AdviceOndischarge = _model.AdviceOndischarge;
                }

                if (!string.IsNullOrEmpty(_model.SeniorResident))
                {
                    data.SeniorResident = _model.SeniorResident;
                }
                if (!string.IsNullOrEmpty(_model.JuniorResident))
                {
                    data.JuniorResident = _model.JuniorResident;
                }


                _context.Discharge.Update(data);
                _context.SaveChanges();

                var Patientdata = _context.Patient.FirstOrDefault(a => a.PatientID == data.PatientID);
                if (Patientdata != null)
                {
                    Patientdata.Status = "Discharge";
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }

        private void ResizeAndCompressImage(string sourcePath, string destinationPath, int maxWidth, int maxHeight, int quality)
        {
            //using (var image = Image.Load(sourcePath))
            //{
            //    image.Mutate(x => x.Resize(new ResizeOptions
            //    {
            //        Size = new Size(maxWidth, maxHeight),
            //        Mode = ResizeMode.Max
            //    }));

            //    var encoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
            //    {
            //        Quality = quality
            //    };

            //    image.Save(destinationPath, encoder);
            //}
        }

        //public bool UpdateOutCome(OutcomeModel _model)
        //{
        //    if (_model.PatientID > 0)
        //    {
        //        var data = _context.Outcome.Where(a => a.Id == _model.Id && a.PatientID == _model.PatientID).FirstOrDefault();

        //        data.Date = _model.Date;
        //        data.outcomeType = _model.Outcome;

        //        _context.Outcome.Update(data);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    return false;
        //}

        public PatientViewModel OutComeDetail(long PatientID)
        {
            PatientViewModel pvm = new PatientViewModel();
            pvm.Outcome = _context.Outcome.Where(a => a.PatientID == PatientID).FirstOrDefault();
            if (pvm.Outcome == null)
            {
                pvm.Outcome = new Outcome();
                pvm.Outcome.PatientID = PatientID;
            }
            return pvm;
        }

        public long AddOutCome(OutcomeModel _model, long patientid)
        {
            try
            {
                if (patientid > 0)
                {
                    var outcomeExits = _context.Outcome.Where(a => a.PatientID == patientid).FirstOrDefault();
                    if (outcomeExits != null)
                    {
                        _model.PatientID = patientid;
                        UpdateOutCome(_model);
                        return patientid;
                    }
                    else
                    {
                        var outcome = new Outcome
                        {
                            PatientID = patientid,
                            Date = _model.Date,
                            outcomeType = _model.Outcome

                        };

                        _context.Outcome.Add(outcome);
                        _context.SaveChanges();
                        return outcome.PatientID;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public PatientViewModel EditOutCome(long? PatientID)
        //{

        //    PatientViewModel pvm = new PatientViewModel();

        //    pvm.Outcome = _context.Outcome.Where(a => a.PatientID == PatientID).FirstOrDefault();
        //    return pvm;
        //}

        public bool UpdateOutCome(OutcomeModel _model)
        {
            if (_model.Id > 0)
            {
                var data = _context.Outcome.Where(a => a.Id == _model.Id).FirstOrDefault();
                data.Date = _model.Date;
                data.outcomeType = _model.Outcome;
                _context.Outcome.Update(data);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<string> Addimge(IFormFile img)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
            var filePath = Path.Combine("wwwroot/Images_Data", uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await img.CopyToAsync(fileStream);
            }
            return uniqueFileName;
        }
    }
}

