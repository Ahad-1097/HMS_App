using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using App.Models.DbContext;
using App.Interface;
using System.Threading;
using App.DtoModel;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using App.Models.ViewModel;
using Newtonsoft.Json;
using App.Models.DtoModel;
using System.Data;
using System.Security.Claims;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using App.Models.EntityModels;

namespace App.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IPatientRepo _patient;
        private readonly IDocterRepo _docterRepo;
        public static long _PatientId = 0;
        public static long _InvestigationId = 0;
        static int _catId = 0;
        static bool addimgsuccess = false;

        List<InvestigationModel> TabledataList = new List<InvestigationModel>();

        public PatientController(ApplicationContext context, IPatientRepo patient, IDocterRepo docterRepo)
        {
            _context = context;
            _patient = patient;
            _docterRepo = docterRepo;
        }

        [HttpGet("Index")]
        // GET: Patient
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            // Get the user's roles from the ClaimsPrincipal
            var userRoles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

            string Role = "";
            if (userRoles.Contains("Admin"))
            {
                Role = "Admin";
            }
            var result = await _patient.GetPatientList(cancellationToken, Role);
            result = result.OrderByDescending(patient => patient.PatientID).ToList();
            return View(result);

            // var result = await _patient.GetPatientList(cancellationToken, Role);

            // return View(result);
        }

        // GET: Patient/Details/5
        public IActionResult Details(long id)
        {
            if (id == 0) { return NotFound(); }
            PatientModel model = new PatientModel();
            model.PatientID = id;
            return View(model);
        }

        // GET: Patient/Create
        public async Task<IActionResult> Create()
        {
            var doctors = _docterRepo.GetDoterList();
            ViewBag.DoctorList = new SelectList(doctors, "Dr_ID", "Dr_Name");

            var JRlist = await _docterRepo.getDropDownlist("jr");
            ViewBag.JRlist = new SelectList(JRlist, "Name", "Name");

            var SRlist = await _docterRepo.getDropDownlist("sr");
            ViewBag.SRlist = new SelectList(SRlist, "Name", "Name");
            List<DropDrownModel> OutcomeType = new List<DropDrownModel>
        {
            new DropDrownModel { ID = "Discharged", Name = "Discharged" },
            new DropDrownModel { ID = "LAMA", Name = "LAMA" },
            new DropDrownModel { ID = "Absconded", Name = "Absconded" },
            new DropDrownModel { ID = "Transferred", Name = "Transferred" },
            new DropDrownModel { ID = "Death", Name = "Death" }
        };

            ViewBag.OutcomeType = new SelectList(OutcomeType, "ID", "Name");
            if (addimgsuccess)
            {
                ViewBag.addImagesSuccess = "Data added successfully";
            }
            addimgsuccess = false;

            return View();
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(long? id, CancellationToken cancellationToken)
        {
            if (id == null) { return NotFound(); }

            var doctors = _docterRepo.GetDoterList();
            ViewBag.DoctorList = new SelectList(doctors, "Dr_ID", "Dr_Name");

            List<DropDrownModel> OutcomeType = new List<DropDrownModel>
        {
            new DropDrownModel { ID = "Discharged", Name = "Discharged" },
            new DropDrownModel { ID = "LAMA", Name = "LAMA" },
            new DropDrownModel { ID = "Absconded", Name = "Absconded" },
            new DropDrownModel { ID = "Transferred", Name = "Transferred" },
            new DropDrownModel { ID = "Death", Name = "Death" }
        };

            ViewBag.OutcomeType = new SelectList(OutcomeType, "ID", "Name");

            var patient = await _patient.PatientDetail(id, cancellationToken);
            _PatientId = Convert.ToInt32(id);
            if (patient == null) { return NotFound(); }
            return View(patient);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPatient(string model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                string msg;
                try
                {
                    var _model = JsonConvert.DeserializeObject<List<PatientModel>>(model).FirstOrDefault();
                    if (_model.PatientID > 0)
                    {
                        await _patient.UpdatePatient(_model, cancellationToken);
                        msg = "Data updated successfully";
                    }
                    else
                    {
                        _PatientId = _patient.AddPatient(_model, cancellationToken);
                        msg = "Data added successfully";
                    }

                    var doctors = _docterRepo.GetDoterList();
                    ViewBag.DoctorList = new SelectList(doctors, "Dr_ID", "Dr_Name");

                    return Json(msg);
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return Json("something went wrong");
        }

        [HttpGet]
        public async Task<IActionResult> Patientdata(long PatientID, string ViewName, CancellationToken cancellationToken)
        {
            var doctors = _docterRepo.GetDoterList();
            ViewBag.DoctorList = new SelectList(doctors, "Dr_ID", "Dr_Name");

            //var JRlist = await _docterRepo.getDropDownlist("jr");
            //ViewBag.JRlist = new SelectList(JRlist, "Name", "Name");

            //var SRlist = await _docterRepo.getDropDownlist("sr");
            //ViewBag.SRlist = new SelectList(SRlist, "Name", "Name");


            PatientViewModel data = new PatientViewModel();
            if (PatientID > 0 && ViewName == "Edit")
            {
                data = await _patient.PatientDetail(PatientID, cancellationToken);
                return PartialView("AddPatient", data);
            }
            else if (PatientID > 0 && ViewName == "Detail")
            {

                data = await _patient.PatientDetail(PatientID, cancellationToken);

                return PartialView("_ViewPatient", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                data = await _patient.PatientDetail(PatientID, cancellationToken);

                return GeneratePdf(data.PatientModel);
            }
            return PartialView("AddPatient", data);

        }

        //Add Investigation
        [HttpPost]
        public async Task<IActionResult> AddInvestigation(string data, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                var doctors = _docterRepo.GetDoterList();
                ViewBag.DoctorList = new SelectList(doctors, "Dr_ID", "Dr_Name");

                var _model = JsonConvert.DeserializeObject<List<InvestigationModel>>(data).FirstOrDefault();
                if (_model == null) { return Json(null); }
                if (_model.Id > 0)
                {
                    await _patient.UpdateInvestigationData(_model, cancellationToken);
                    var investigationList = _context.Investigation.Where(a => a.PatientID == _model.PatientID).ToList();
                    string jsonData = JsonConvert.SerializeObject(investigationList);
                    return Json(jsonData);
                }
                else
                {
                    if (_PatientId > 0)
                    {
                        _InvestigationId = _patient.AddInvestigationData(_model, _PatientId);
                    }

                    var investigationList = _context.Investigation.Where(a => a.PatientID == _PatientId).ToList();
                    string jsonData = JsonConvert.SerializeObject(investigationList);
                    return Json(jsonData);
                }

            }
            return Json(null);
        }

        [HttpGet]
        public IActionResult AddInvestigation(long PatientID, string ViewName)
        {
            PatientViewModel data = new PatientViewModel();

            if (PatientID > 0 && ViewName == "Edit")
            {
                _PatientId = PatientID;
                data = _patient.InvestigationDetail(PatientID);
                if (data.InvestigationList.Count() == 0)
                {
                    return PartialView("_AddInvestigation", data);
                }
                else
                {
                    return PartialView("_EditInvestigation", data);
                }
            }

            else if (PatientID > 0 && ViewName == "Detail")
            {

                data = _patient.InvestigationDetail(PatientID);
                ViewBag.PId = PatientID;
                return PartialView("_ViewInvestigation", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                data = _patient.InvestigationDetail(PatientID);

                // return GeneratePdf(data.InvestigationList.ToList());
                return GenerateListofPdf(data.InvestigationList.ToList());
            }
            return PartialView("_EditInvestigation", data);

        }

        [HttpPost]
        public async Task<IActionResult> AddUpadateInvestigationDay(string data, CancellationToken cancellationToken)
        {

            PatientViewModel viewModel = new PatientViewModel();

            var _model = JsonConvert.DeserializeObject<List<InvestigationModel>>(data).FirstOrDefault();
            if (_model == null) { return Json(null); }
            if (_model.Id > 0)
            {
                await _patient.UpdateInvestigationData(_model, cancellationToken);
            }

            viewModel.InvestigationList = _context.Investigation.Where(a => a.PatientID == _model.PatientID).ToList();
            string jsonData = JsonConvert.SerializeObject(viewModel.InvestigationList);
            return Json(jsonData);

        }

        [HttpGet]
        public IActionResult AddPicture(long PatientID, string ViewName, long ImgId = 0)
        {
            PatientViewModel data = new PatientViewModel();
           

            if (PatientID > 0 && ViewName == "Edit")
            {
                // Pass the value to the view

                data = _patient.PictureDetail(PatientID, ImgId);
                if (data.InvestigationImages == null)
                {
                    ViewBag.Method = "save";
                    ViewBag.PatientId = PatientID;
                }
                //  data.PatientModel.PatientID = PatientID;
                //if (data.InvestigationImages != null && ImgId > 0)
                //{

                //    //data.InvestigationImages = data.InvestigationImagesList.Where(a => a.Id == ImgId).FirstOrDefault();
                //    //data.InvestigationImages.PatientId = (int)PatientID;
                //    return PartialView("_EditPicture", data);
                //}
                else
                {
                    //data = _patient.PictureDetail(PatientID, ImgId);
                    //data.InvestigationImages.PatientId = (int)PatientID;
                    return PartialView("_EditPicture", data);
                }
            }

            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = _patient.PictureDetail(PatientID, ImgId);
                return PartialView("_ViewPicture", data);
            }

            InvestigationImagesModel investigationImagesModel = new InvestigationImagesModel();
            ViewBag.PatientId = PatientID;
            return PartialView("_AddPicture", investigationImagesModel);


        }

        [HttpPost]
        public async Task<IActionResult> AddPicture([FromForm] InvestigationImagesModel imageFiles, CancellationToken cancellationToken)
        {
            if (imageFiles != null)
            {
                string msg;
                if (imageFiles.PatientId > 0 && imageFiles.Id > 0)
                {
                    try
                    {
                        await _patient.UpdateInvestigationImages(imageFiles, cancellationToken);
                        msg = "Data updated successfully";
                        return Json(msg);
                    }
                    catch (Exception ex)
                    {
                        return Json($"Error updating data: {ex.Message}");
                    }
                }
                else if (imageFiles.PatientId > 0 && imageFiles.Id == 0)
                {
                    try
                    {
                        _patient.AddInvestigationImages(imageFiles, _InvestigationId, _PatientId);
                        msg = "Data added successfully";
                        //return RedirectToAction("Create");
                        msg = "Data added successfully";
                        return Json(msg);

                    }
                    catch (Exception ex)
                    {
                        return Json($"Error adding data: {ex.Message}");
                    }
                }
                else if (_PatientId > 0)
                {

                    _patient.AddInvestigationImages(imageFiles, _InvestigationId, _PatientId);
                    msg = "Data added successfully";
                    addimgsuccess = true;

                    return RedirectToAction("Create", true);
                    //InvestigationImagesModel investigationImagesModel = new InvestigationImagesModel();
                    //return PartialView("_AddPicture", investigationImagesModel);
                }

                //if (imageFiles.Msg == "save")
                //{
                //    try
                //    {
                //        await _patient.UpdateInvestigationImages(imageFiles, cancellationToken);
                //        msg = "Data updated successfully";
                //        return Json(msg);
                //    }
                //    catch (Exception ex)
                //    {
                //        return Json($"Error updating data: {ex.Message}");
                //    }
                //}
                //else if (imageFiles.PatientId > 0 && imageFiles.Id == 0)
                //{
                //    try
                //    {
                //        _patient.AddInvestigationImages(imageFiles, _InvestigationId, _PatientId);
                //        msg = "Data added successfully";
                //        //return RedirectToAction("Create");
                //        msg = "Data added successfully";
                //        return Json(msg);

                //    }
                //    catch (Exception ex)
                //    {
                //        return Json($"Error adding data: {ex.Message}");
                //    }
                //}
                //else if (_PatientId > 0)
                //{

                //    _patient.AddInvestigationImages(imageFiles, _InvestigationId, _PatientId);
                //    msg = "Data added successfully";
                //    addimgsuccess=true;

                //    return RedirectToAction("Create");
                //    //InvestigationImagesModel investigationImagesModel = new InvestigationImagesModel();
                //    //return PartialView("_AddPicture", investigationImagesModel);


                //}


                return Json("Invalid parameters or conditions");
            }

            return Json("Invalid input");
        }


        [HttpPost]
        public IActionResult Progress(string model)
        {
            if (ModelState.IsValid)
            {
                string msg;
                var _model = JsonConvert.DeserializeObject<List<ProgressModel>>(model).FirstOrDefault();
                if (_model.Id > 0)
                {
                    var result = _patient.UpdateProgress(_model);
                    msg = "Data updated successfully";
                    return Json(msg);
                }

                if (_PatientId > 0)
                {
                    _patient.AddProgress(_model, _PatientId);
                    msg = "Data added successfully";
                    return Json(msg);
                }
                return Json("something went wrong");
            }
            return Json("something went wrong");
        }

        [HttpGet]
        public IActionResult Progress(long PatientID, string ViewName)
        {
            PatientViewModel data = new PatientViewModel();

            if (PatientID > 0 && ViewName == "Edit")
            {
                data = _patient.ProgressDetail(PatientID);
                return PartialView("AddProgress", data);
            }
            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = _patient.ProgressDetail(PatientID);
                return PartialView("_ViewProgress", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                data = _patient.ProgressDetail(PatientID);

                return GeneratePdf(data.Progress);
            }
            return PartialView("AddProgress", data);

        }


        [HttpPost]
        public IActionResult Vitals(string model)
        {
            if (ModelState.IsValid)
            {
                string msg;
                var _model = JsonConvert.DeserializeObject<List<ProgressModel>>(model).FirstOrDefault();
                if (_PatientId > 0)
                {
                    _patient.UpdateVital(_model, _PatientId);
                    msg = "Successfull";
                    return Json(msg);
                }
                else if (_model.PatientID > 0)
                {
                    _patient.UpdateVital(_model, _model.PatientID);
                    msg = "Successfull";
                    return Json(msg);
                }

                return Json("something went wrong");
            }
            return Json("something went wrong");
        }

        [HttpGet]
        public IActionResult Vitals(long PatientID, string ViewName)
        {
            PatientViewModel data = new PatientViewModel();

            if (PatientID > 0 && ViewName == "Edit")
            {
                data = _patient.ProgressDetail(PatientID);
                return PartialView("AddVitals", data);
            }
            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = _patient.ProgressDetail(PatientID);
                return PartialView("_ViewVitals", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                data = _patient.ProgressDetail(PatientID);

                return GeneratePdf(data.Progress);
            }
            data.Progress.PatientID = _PatientId;
            return PartialView("AddVitals", data);

        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Diagnosis(string model)
        {
            if (ModelState.IsValid)
            {
                var _model = JsonConvert.DeserializeObject<List<PatientModel>>(model);
                string msg;
                if (_PatientId > 0)
                {
                    _patient.AddDiagnosis(_model[0], _PatientId);
                    msg = "Successfull";
                    return Json(msg);
                }
                if (_model[0].PatientID > 0)
                {
                    _patient.AddDiagnosis(_model[0], _model[0].PatientID);
                    msg = "Data updated successfully";
                    return Json(msg);
                }
                return Json("something went wrong");
            }
            return Json("something went wrong");
        }

        [HttpGet]
        public async Task<IActionResult> DiagnosisEdit(long PatientID, string ViewName, CancellationToken cancellationToken)
        {
            PatientViewModel data = new PatientViewModel();

            if (PatientID > 0 && ViewName == "Edit")
            {
                data = await _patient.PatientDetail(PatientID, cancellationToken);
                return PartialView("_EditDiagnosis", data);
            }
            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = await _patient.PatientDetail(PatientID, cancellationToken); ;
                return PartialView("_ViewDiagnosis", data);
            }
            return PartialView("_ViewDiagnosis", data);

        }

        [HttpPost]
        public async Task<IActionResult> CaseSheet(string model)
        {
            try
            {
                if (model != null)
                {
                    string msg;
                    var _model = JsonConvert.DeserializeObject<List<CaseSheetModel>>(model).FirstOrDefault();
                    if (_model.Id > 0)
                    {
                        //_model.AddImage = await Uploadimg(AddImageFile);
                        await _patient.UpdateCaseSheet(_model);
                        msg = "Data updated successfully";
                        return Json(msg);
                    }
                    if (_PatientId > 0)
                    {
                        //_model.AddImage = await Uploadimg(AddImageFile);
                        _patient.AddCaseSheet(_model, _PatientId);
                        msg = "Data added successfully";
                        return Json(msg);
                    }

                    return Json("something went wrong");
                }
            }
            catch (Exception e)
            {

                throw;
            }

            return Json("something went wrong");

        }

        [HttpGet]
        public IActionResult CaseSheet(long PatientID, string ViewName)
        {
            PatientViewModel data = new PatientViewModel();

            if (PatientID > 0 && ViewName == "Edit")
            {
                data = _patient.CaseSheetDetail(PatientID);
                return PartialView("CaseSheet", data);
            }
            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = _patient.CaseSheetDetail(PatientID);
                return PartialView("_ViewCaseSheet", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                data = _patient.CaseSheetDetail(PatientID);

                return GeneratePdf(data.CaseSheet);
            }
            return PartialView("CaseSheet", data);

        }

        [HttpGet]
        public IActionResult Operation(long PatientID, string ViewName)
        {
            PatientViewModel data = new PatientViewModel();
            var doctors = _docterRepo.GetDoterList();
            ViewBag.DoctorList = new SelectList(doctors, "Dr_ID", "Dr_Name");
            if (PatientID > 0)
            {
                data = _patient.OperationDetail(PatientID);
                if (!string.IsNullOrWhiteSpace(data.Operation.Dr_ID))
                {
                    ViewBag.SelectedDoctor = doctors.FirstOrDefault(a => a.Dr_ID == data.Operation.Dr_ID).Dr_Name;
                }

            }
            if (PatientID > 0 && ViewName == "Edit")
            {
                return PartialView("OperationSheet", data);
            }
            else if (PatientID > 0 && ViewName == "Detail")
            {
                return PartialView("_ViewOperation", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                return GeneratePdf(data.Operation);
            }
            return PartialView("OperationSheet", data);
        }


        [HttpPost]
        public async Task<IActionResult> OperationSheet([FromForm] OperationModel Oprationmodel)
        {
            try
            {
                var model = Oprationmodel;
                if (model != null)
                {
                    string msg;

                    // Assuming you're using a service/repository named _patient
                    if (model.Id > 0)
                    {
                        // Handle the image file
                        if (model.PerOPImageFile != null && model.PerOPImageFile.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" +
                                model.PerOPImageFile.FileName;
                            var filePath = Path.Combine("wwwroot/Images_Data", uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await model.PerOPImageFile.CopyToAsync(fileStream);
                            }
                            model.PerOPImage = uniqueFileName;
                        }
                        await _patient.UpdateOperationSheet(model);
                        msg = "Data updated successfully";
                    }
                    else if (_PatientId > 0)
                    {
                        // Handle the image file
                        if (model.PerOPImageFile != null && model.PerOPImageFile.Length > 0)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PerOPImageFile.FileName;
                            var filePath = Path.Combine("wwwroot/Images_Data", uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await model.PerOPImageFile.CopyToAsync(fileStream);
                            }
                            model.PerOPImage = uniqueFileName;
                        }
                        _patient.AddOperationSheet(model, _PatientId);
                        msg = "Data added successfully";
                    }
                    else if (model.PatientID > 0 && model.Id == 0)
                    {
                        _patient.AddOperationSheet(model, model.PatientID);
                        msg = "Data added successfully";
                    }
                    else
                    {
                        return Json("Something went wrong");
                    }



                    return Json(msg);
                }

                return Json("Something went wrong");
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return Json("Error occurred while processing the request");
            }
        }

        [HttpPost]
        public async Task<IActionResult> OperationSheetold(string model)
        {
            if (model != null)
            {
                string msg;
                var _model = JsonConvert.DeserializeObject<List<OperationModel>>(model);
                if (_model[0].Id > 0)
                {
                    await _patient.UpdateOperationSheet(_model[0]);
                    msg = "Data updated successfully";
                    return Json(msg);
                }
                else if (_PatientId > 0)
                {
                    _patient.AddOperationSheet(_model[0], _PatientId);
                    msg = "Data added successfully";
                    return Json(msg);
                }
                else if (_model[0].PatientID > 0 && _model[0].Id == 0)
                {
                    _patient.AddOperationSheet(_model[0], _model[0].PatientID);
                    msg = "Data added successfully";
                    return Json(msg);
                }

                return Json("something went wrong");
            }
            return Json("something went wrong");
        }

        [HttpPost]
        public IActionResult DischargePost(string model)
        {
            if (ModelState.IsValid)
            {
                string msg;
                PatientViewModel modl = new PatientViewModel();
                var _model = JsonConvert.DeserializeObject<List<DischargeModel>>(model).FirstOrDefault();
                if (_model.Id > 0)
                {
                    _patient.UpdateDischarge(_model);
                    msg = "Data updated successfully";
                    return Json(msg);
                }
                if (_model.PatientID > 0)
                {
                    _patient.AddDischarge(_model, _model.PatientID);
                    msg = "Data added successfully";
                    return Json(msg);
                }
                return Json("something went wrong");
            }
            return Json("something went wrong");
        }

        [HttpGet]
        public IActionResult Discharge(long PatientID, string ViewName)
        {
            if (PatientID == 0)
            {
                if (_PatientId != 0)
                {
                    PatientID = _PatientId;
                };
            };

            //var JRlist = await _docterRepo.getDropDownlist("jr");
            //ViewBag.JRlist = new SelectList(JRlist, "Name", "Name");

            //var SRlist = await _docterRepo.getDropDownlist("sr");
            //ViewBag.SRlist = new SelectList(SRlist, "Name", "Name");

            PatientViewModel data = new PatientViewModel();
            if (PatientID > 0 && ViewName == "Edit")
            {
                data = _patient.DischargeDetail(PatientID);
                return PartialView("Discharge", data);
            }
            if (PatientID > 0 && ViewName == "Create")
            {
                data = _patient.DischargeDetail(PatientID);
                return PartialView("Discharge", data);
            }
            //if (_PatientId > 0)
            //{

            //    data = _patient.DischargeDetail(_PatientId);
            //    return PartialView("Discharge", data);
            //}

            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = _patient.DischargeDetail(PatientID);
                return PartialView("_ViewDischarge", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                var Printdata = _patient.DischargePrintDetail(PatientID);

                return GeneratePdf(Printdata);
            }
            return PartialView("Discharge", data);
        }

        // GET: Patient/Delete/5
        public async Task<IActionResult> Delete(long? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return NotFound();
            }
            var patient = await _patient.PatientDetail(id, cancellationToken);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id, CancellationToken cancellationToken)
        {
            var result = await _patient.DeletePatient(id, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public JsonResult SaveModels(string data)
        {
            try
            {
                List<InvestigationModel> Tabledata = JsonConvert.DeserializeObject<List<InvestigationModel>>(data);
                var _tabledata = _patient.addTempData(Tabledata);
                string jsonData = JsonConvert.SerializeObject(_tabledata);
                return Json(jsonData);
            }
            catch (System.Exception e)
            {
                return Json(false, e);
            }
        }

        [HttpGet]
        public IActionResult GetSubCategoryList(string query = null, int CategoryId = 0)
        {
            if (CategoryId > 0)
            {
                _catId = CategoryId;
            }
            if (!string.IsNullOrEmpty(query))
            {
                var FilterdCategory = _context.SubCategory
                .Where(d => d.SubCategoryTitle.Contains(query) && d.CategoryId == _catId)
                .Select(d => new { id = d.Id, text = d.SubCategoryTitle })
                .ToList();
                return Json(FilterdCategory);
            }
            else
            {
                var AllSubCategory = _context.SubCategory
                      .Where(a => a.CategoryId == _catId)
                      .Select(d => new { id = d.Id, text = d.SubCategoryTitle })
                      .ToList();
                return Json(AllSubCategory);
            }


        }

        public IActionResult DaysEdit(int investigationId = 0)
        {

            if (investigationId > 0)
            {
                var FilterdCategory = _context.Investigation.Where(a => a.Id == investigationId);
                return Json(FilterdCategory);
            }
            return Json("");

        }

        public IActionResult DeleteInvestigation(int InvestigationId = 0, long PatientID = 0)
        {
            if (InvestigationId > 0)
            {
                var result = _patient.RemoveInvestigation(InvestigationId);
                if (result)
                {
                    var investigationList = _context.Investigation.Where(a => a.PatientID == PatientID).ToList();
                    string jsonData = JsonConvert.SerializeObject(investigationList);
                    return Json(jsonData);
                }
            }
            return Json("");
        }

        [HttpGet("/Patient/GeneratePdf")]
        public IActionResult GeneratePdf<TModel>(TModel model)
        {
            // Create a new document
            Document doc = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);


            List<string> propertiesToIgnore = new List<string>
            { "PatientID","Patient","Address_ID","Dr_ID","OtherO","OtherT","OtherTh","Id",
                "CreatedBy","UpdateBy","CreatedOn","UpdatedOn","DateOfBirth",
                "Address","Email","Status","IsActive","Title","SubCategoryTitle",
                "subCategory","InvestigationModel","InvestigationImagesModel","Value1","Value2","Value3"
            };

            // Add data to the PDF document
            doc.Open();
            // Iterate through the properties of the model and add them to the PDF
            PropertyInfo[] properties = typeof(TModel).GetProperties();
            doc.AddTitle("heading");
            doc.AddHeader("H1", "this is header");
            foreach (PropertyInfo property in properties)
            {
                if (propertiesToIgnore.Contains(property.Name))
                {
                    continue; // Skip this property
                }
                string propertyName = property.Name;
                object propertyValue = property.GetValue(model);

                // Add the property name and value to the PDF as paragraphs
                doc.Add(new Paragraph($"{propertyName}: {propertyValue}"));



            }
            doc.Close(); // Close the document

            // Create a copy of the MemoryStream
            MemoryStream copyStream = new MemoryStream(memoryStream.ToArray());

            // Set the content type and file name for the response
            HttpContext.Response.ContentType = "application/pdf";
            HttpContext.Response.Headers.Add("content-disposition", "inline;filename=mydocument.pdf");

            // Write the PDF content to the response
            copyStream.CopyTo(HttpContext.Response.Body);
            return new EmptyResult();
        }

        [HttpGet("/Patient/GeneratePdfdata")]
        public IActionResult GenerateListofPdf(List<Investigation> model)
        {
            // Create a new document
            Document doc = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);


            List<string> propertiesToIgnore = new List<string>
            { "PatientID","Patient","Address_ID","Dr_ID","OtherO","OtherT","OtherTh","Id",
                "CreatedBy","UpdateBy","CreatedOn","UpdatedOn","DateOfBirth",
                "Address","Email","Status","IsActive","Title","SubCategoryTitle",
                "subCategory","InvestigationModel","InvestigationImagesModel","Value1","Value2","Value3"
            };

            // Add data to the PDF document
            doc.Open();
            foreach (var item in model)
            {
                var properties = typeof(Investigation).GetProperties();
                doc.AddTitle("heading");
                doc.AddHeader("H1", "this is header");
                foreach (PropertyInfo property in properties)
                {
                    if (propertiesToIgnore.Contains(property.Name))
                    {
                        continue; // Skip this property
                    }
                    string propertyName = property.Name;
                    object propertyValue = property.GetValue(item);

                    // Add the property name and value to the PDF as paragraphs
                    doc.Add(new Paragraph($"{propertyName}: {propertyValue}"));

                }
                doc.Add(new Paragraph($"-----------------------------------------------------------------------------"));
            }

            doc.Close(); // Close the document

            // Create a copy of the MemoryStream
            MemoryStream copyStream = new MemoryStream(memoryStream.ToArray());

            // Set the content type and file name for the response
            HttpContext.Response.ContentType = "application/pdf";
            HttpContext.Response.Headers.Add("content-disposition", "inline;filename=mydocument.pdf");

            // Write the PDF content to the response
            copyStream.CopyTo(HttpContext.Response.Body);

            return new EmptyResult();
        }


        [HttpGet]
        public IActionResult Outcome(long PatientID, string ViewName)
        {
            PatientViewModel data = new PatientViewModel();

            ViewBag.PatientID = PatientID;


            if (PatientID > 0 && ViewName == "Edit")
            {
                data = _patient.OutComeDetail(PatientID);
                return PartialView("EditOutcome", data);
            }
            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = _patient.OutComeDetail(PatientID);
                return PartialView("_ViewOutcome", data);
            }
            else if (PatientID > 0 && ViewName == "Print")
            {
                data = _patient.OutComeDetail(PatientID);

                return GeneratePdf(data.Outcome);
            }

            return PartialView("_Outcome", data);

        }



        [HttpPost]
        public IActionResult Outcome(string model)
        {
            try
            {

                if (model != null)
                {
                    string msg;

                    var _model = JsonConvert.DeserializeObject<OutcomeModel>(model);

                    if (_model.Id > 0)
                    {

                        _patient.UpdateOutCome(_model);
                        msg = "Data updated successfully";
                        return Json(msg);
                    }
                    else if (_PatientId > 0 && _model.Id == 0)
                    {

                        _patient.AddOutCome(_model, _model.PatientID);
                        msg = "Data added successfully";
                        return Json(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json("something went wrong");

        }

        [HttpPost]
        public IActionResult UpdateOutcome(string model)
        {
            try
            {

                if (model != null)
                {
                    string msg;

                    var _model = JsonConvert.DeserializeObject<OutcomeModel>(model);

                    if (_model.Id > 0)
                    {

                        _patient.UpdateOutCome(_model);
                        msg = "Data updated successfully";
                        return Json(msg);
                    }
                    else if (_PatientId > 0 && _model.Id == 0)
                    {

                        _patient.AddOutCome(_model, _PatientId);
                        msg = "Data added successfully";
                        return Json(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
            return Json("something went wrong");

        }


        private async Task<string> Uploadimg(IFormFile imgfile)
        {
            if (imgfile != null)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imgfile.FileName;
                var filePath = Path.Combine("wwwroot/Images_Data", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imgfile.CopyToAsync(fileStream);
                }
                return uniqueFileName;
            }
            return "";
        }
    }

}



