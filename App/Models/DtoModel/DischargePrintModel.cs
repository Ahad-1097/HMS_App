using App.Models.EntityModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.DtoModel
{
    public class DischargePrintModel
    {
        public long Id { get; set; }
        public long PatientID { get; set; }
        [Display(Name = "Consultants")]
        public string Dr_Name { get; set; }
        //patient
        public string Name { get; set; }
        
        public string Gender { get; set; }
        public string Age { get; set; }

        [Display(Name = "CADS NO")]
        public string CADSNumber { get; set; }
        [Display(Name = "OPD Number")]
        public string OPDNumber { get; set; }

        public string Address { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string DOA { get; set; }
        public string DOD { get; set; }

        [Display(Name = "Diagnosis")]
        public string Diagnosis { get; set; }
        public string Side { get; set; }
        [Display(Name = "Comorbidity")]
        public string CoMorbity { get; set; }

        [Display(Name = "Case Summary")]
        public string CaseSummary { get; set; }
        public string Investigations { get; set; }
        [Display(Name = "Treatment Given")]
        public string TreatmentGiven { get; set; }
        [Display(Name = "Discharge Advice")]
        public string AdviceOndischarge { get; set; }

       
        //[Display(Name = "Alternate Number")]
        //public string AlternateNumber { get; set; }
        
        [Display(Name = "Senior Resident")]
        public string SeniorResident { get; set; }

        [Display(Name = "Junior Resident")]
        public string JuniorResident { get; set; }
       
       
       
      
        public string Title { get; set; }
       
        public bool IsActive { get; set; }
    }
}
