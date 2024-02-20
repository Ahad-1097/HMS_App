using System.Collections.Generic;
using App.DtoModel;
using App.Models.DbContext;
using App.Models.DtoModel;
using App.Models.EntityModels;

namespace App.Models.ViewModel
{
    public class PatientViewModel
    {

        public virtual AddressModel AddressModel { get; set; }
        public InvestigationModel InvestigationModel { get; set; }
        public InvestigationImagesModel InvestigationImagesModel { get; set; }
        public List<InvestigationModel> InvestigationModelList { get; set; }
        public List<Investigation> InvestigationList { get; set; }
        public PatientModel PatientModel { get; set; }
        public InvestigationImages InvestigationImages { get; set; }
        public List<InvestigationImages> InvestigationImagesList { get; set; }
        public virtual Patient  Patient { get; set; }
        public virtual Progress Progress { get; set; }
        public virtual Operation Operation { get; set; }
        public virtual Discharge Discharge { get; set; }
        public virtual CaseSheet CaseSheet { get; set; }
        public virtual CaseSheetModel CaseSheetModel { get; set; }
        public virtual OperationModel OperationModel { get; set; }
        public virtual ProgressModel ProgressModel { get; set; }
        public virtual DischargeModel DischargeModel { get; set; }

        public virtual Outcome Outcome { get; set; }
        public virtual OutcomeModel OutcomeModel { get; set; }
    }
}
