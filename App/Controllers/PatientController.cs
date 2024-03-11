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
using App.Models.EntityModels;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

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
            var doctors = await _docterRepo.GetDoterList();
            ViewBag.DoctorList = new SelectList(doctors, "Dr_ID", "Dr_Name");

            //var JRlist = await _docterRepo.getDropDownlist("jr");
            //ViewBag.JRlist = new SelectList(JRlist, "Name", "Name");

            //var SRlist = await _docterRepo.getDropDownlist("sr");
            //ViewBag.SRlist = new SelectList(SRlist, "Name", "Name");


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

            var doctors = await _docterRepo.GetDoterList();
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

                    var doctors = await _docterRepo.GetDoterList();
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
            var doctors = await _docterRepo.GetDoterList();
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
                var doctors = await _docterRepo.GetDoterList();
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
                if (data.InvestigationList.Any(a => a.Id == 0))
                {
                    //data.InvestigationModel.PatientID= _PatientId;
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

                data = _patient.PictureDetail(PatientID, ImgId, "Edit");
                if (data.InvestigationImages == null)
                {
                    ViewBag.Method = "save";
                    ViewBag.PatientId = PatientID;
                }
                else
                {
                    return PartialView("_EditPicture", data);
                }
            }

            else if (PatientID > 0 && ViewName == "Detail")
            {
                data = _patient.PictureDetail(PatientID, ImgId, "Detail");
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
                }
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
        public async Task<IActionResult> Operation(long PatientID, string ViewName)
        {
            PatientViewModel data = new PatientViewModel();
            var doctors = await _docterRepo.GetDoterList();
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
                "subCategory","InvestigationModel","InvestigationImagesModel","Value1","Value2","Value3","SerialNumber"
            };



            // Add data to the PDF document
            doc.Open();
            // Iterate through the properties of the model and add them to the PDF
            PropertyInfo[] properties = typeof(TModel).GetProperties();
            iTextSharp.text.Font hs = new iTextSharp.text.Font
            {
                Size = 18
            };
            hs.SetColor(0, 122, 100);
            hs.IsBold();


            // Create a table to contain the header content
            PdfPTable headerTable = new PdfPTable(1)
            {
                TotalWidth = doc.PageSize.Width, // Set total width to match document width
                LockedWidth = true, // Ensure the table width is fixed
            };

            // Create a cell with the content
            PdfPCell headerCell = new PdfPCell();

            // Create a Phrase with the image and header text
            Phrase headerPhrase = new Phrase();

            // Add the image
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("wwwroot/images/logo.png"); // Replace "path_to_your_image.jpg" with the path to your image
            image.ScaleToFit(700f, 700f); // Adjust the image size as needed
            headerPhrase.Add(new Chunk(image, 0, 0));

            // Add the header text
            headerPhrase.Add(new Chunk("     Department of Surgery, J.N Medical College AMU", hs));

            // Set the Phrase as the content of the cell
            headerCell.AddElement(headerPhrase);

            // Set cell properties
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.Border = 0; // No border

            // Add the cell to the table
            headerTable.AddCell(headerCell);

            // Write the table to the PDF document
            headerTable.WriteSelectedRows(0, 1, 30, doc.Top, writer.DirectContent);

            // Draw a line 
            PdfContentByte cbh = writer.DirectContent;
            // Set the color of the line
            cbh.SetColorStroke(BaseColor.GREEN);
            // Set the width of the line
            cbh.SetLineWidth(1f);

            // Draw a line from the left edge to the right edge of the page
            cbh.MoveTo(doc.LeftMargin, doc.PageSize.Height - 60);
            cbh.LineTo(doc.PageSize.Width - doc.RightMargin, doc.PageSize.Height - 60);
            cbh.Stroke();

            doc.Add(new Paragraph($"\n\n\n"));

            foreach (PropertyInfo property in properties)
            {
                if (propertiesToIgnore.Contains(property.Name)) { continue; }

                string propertyName;

                var displayNameAttribute = property.GetCustomAttribute<DisplayAttribute>();
                if (displayNameAttribute != null && displayNameAttribute.Name != null)
                {
                    propertyName = displayNameAttribute.Name;
                }
                else
                {
                    propertyName = property.Name;
                }
                if (model != null)
                {
                    var propertyValue = property.GetValue(model) ?? null;
                    
                    // Create a table with two columns to contain the property name and value
                    PdfPTable table = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    float[] columnWidths = { 40f, 60f }; // Define column widths (50% each)
                    table.SetWidths(columnWidths);

                    // Create a cell for the property name
                    PdfPCell propertyNameCell = new PdfPCell(new Phrase(propertyName, FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Border = 0,// No border
                        BorderWidthBottom = 1,
                        Padding = 5f
                    };
                    table.AddCell(propertyNameCell);

                    // Create a cell for the property value
                    PdfPCell propertyValueCell = new PdfPCell(new Phrase($"{propertyValue}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Border = 0, // No border
                        BorderWidthBottom = 1
                    };
                    table.AddCell(propertyValueCell);

                    // Add the table to the document
                    doc.Add(table);
                }
                else
                {
                    doc.Add(new Paragraph($"{propertyName}: "));
                }
            }

            // Add signature line at the bottom of the page
            PdfPTable signatureTable = new PdfPTable(1)
            {
                TotalWidth = doc.PageSize.Width, // Set total width to match document width
                LockedWidth = true, // Ensure the table width is fixed
            };

            // Create a cell with the content
            PdfPCell signatureCell = new PdfPCell(new Phrase("Signature:"))
            {
                
                HorizontalAlignment = Element.ALIGN_LEFT,
                Border = 0
            };

            signatureTable.AddCell(signatureCell);
            signatureTable.WriteSelectedRows(0, 1, 36, 55, writer.DirectContent);

            // Create a new PdfContentByte object
            PdfContentByte cb = writer.DirectContent;
            // Set the color of the line
            cb.SetColorStroke(BaseColor.GREEN);
            // Set the width of the line
            cb.SetLineWidth(1f);
            // Draw a line at the bottom of the page
            cb.MoveTo(doc.LeftMargin, doc.BottomMargin);
            cb.LineTo(doc.PageSize.Width - doc.RightMargin, doc.BottomMargin);
            cb.Stroke();

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
            { "PatientID","Patient","Address_ID","Dr_ID","Id",
                "CreatedBy","UpdateBy","CreatedOn","UpdatedOn","DateOfBirth",
                "Address","Email","Status","IsActive","Title","SubCategoryTitle",
                "subCategory","InvestigationModel","InvestigationImagesModel","Value1","Value2","Value3"
            };

            // Add data to the PDF document
            doc.Open();

            //header and single line --------------------
            iTextSharp.text.Font hs = new iTextSharp.text.Font
            {
                Size = 18
            };
            hs.SetColor(0, 122, 100);
            hs.IsBold();


            // Create a table to contain the header content
            PdfPTable headerTable = new PdfPTable(1)
            {
                TotalWidth = doc.PageSize.Width, // Set total width to match document width
                LockedWidth = true, // Ensure the table width is fixed
            };

            // Create a cell with the content
            PdfPCell headerCell = new PdfPCell();

            // Create a Phrase with the image and header text
            Phrase headerPhrase = new Phrase();

            // Add the image
            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance("wwwroot/images/logo.png"); // Replace "path_to_your_image.jpg" with the path to your image
            image.ScaleToFit(700f, 700f); // Adjust the image size as needed
            headerPhrase.Add(new Chunk(image, 0, 0));

            // Add the header text
            headerPhrase.Add(new Chunk("     Department of Surgery, J.N Medical College AMU", hs));

            // Set the Phrase as the content of the cell
            headerCell.AddElement(headerPhrase);

            // Set cell properties
            headerCell.HorizontalAlignment = Element.ALIGN_CENTER;
            headerCell.Border = 0; // No border

            // Add the cell to the table
            headerTable.AddCell(headerCell);

            // Write the table to the PDF document
            headerTable.WriteSelectedRows(0, 1, 30, doc.Top, writer.DirectContent);

            // Draw a line 
            PdfContentByte cbh = writer.DirectContent;
            // Set the color of the line
            cbh.SetColorStroke(BaseColor.GREEN);
            // Set the width of the line
            cbh.SetLineWidth(1f);

            // Draw a line from the left edge to the right edge of the page
            cbh.MoveTo(doc.LeftMargin, doc.PageSize.Height - 60);
            cbh.LineTo(doc.PageSize.Width - doc.RightMargin, doc.PageSize.Height - 60);
            cbh.Stroke();

            doc.Add(new Paragraph($"\n\n\n"));

            /// data start-----------------------------


            foreach (var item in model)
            {
                var properties = typeof(Investigation).GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    if (propertiesToIgnore.Contains(property.Name)) { continue; }

                    string propertyName;

                    var displayNameAttribute = property.GetCustomAttribute<DisplayAttribute>();
                    if (displayNameAttribute != null && displayNameAttribute.Name != null)
                    {
                        propertyName = displayNameAttribute.Name;
                    }
                    else
                    {
                        propertyName = property.Name;
                    }
                    if (item != null)
                    {
                        var propertyValue = property.GetValue(item) ?? null;

                        // Create a table with two columns to contain the property name and value
                        PdfPTable table = new PdfPTable(2)
                        {
                            WidthPercentage = 100
                        };
                        float[] columnWidths = { 40f, 60f }; // Define column widths (50% each)
                        table.SetWidths(columnWidths);

                        // Create a cell for the property name
                        PdfPCell propertyNameCell = new PdfPCell(new Phrase(propertyName, FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Border = 0,// No border
                            BorderWidthBottom = 1,
                            Padding = 5f
                        };
                        table.AddCell(propertyNameCell);

                        // Create a cell for the property value
                        PdfPCell propertyValueCell = new PdfPCell(new Phrase($"{propertyValue}", FontFactory.GetFont(FontFactory.HELVETICA, 12)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            Border = 0, // No border
                            BorderWidthBottom = 1
                        };
                        table.AddCell(propertyValueCell);

                        // Add the table to the document
                        doc.Add(table);
                    }
                    else
                    {
                        doc.Add(new Paragraph($"{propertyName}: "));
                    }

                }
                //---------------------------new data start---------------------------------------------;
                // Start a new page
                doc.NewPage();
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

            //ViewBag.PatientID = PatientID;


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
            data.Outcome.PatientID = _PatientId;
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
                    else if (_model.PatientID > 0 && _model.Id == 0)
                    {

                        _patient.AddOutCome(_model, _model.PatientID);
                        msg = "Data added successfully";
                        return Json(msg);
                    }
                    else if (_PatientId > 0)
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



