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
        public string DOA { get; set; }
        public string DOD { get; set; }
        public string Diagnosis { get; set; }
        public string CaseSummary { get; set; }
        public string Investigations { get; set; }
        public string TreatmentGiven { get; set; }
        public string AdviceOndischarge { get; set; }


        //patient
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        
       
        public long Address_ID { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Alternate Number")]
        public string AlternateNumber { get; set; }
        [Display(Name = "CADS NO")]
        public string CADSNumber { get; set; }
        [Display(Name = "OPD Number")]
        public string OPDNumber { get; set; }
        public string Dr_ID { get; set; }
        [Display(Name = "Senior Resident")]
        public string SeniorResident { get; set; }

        [Display(Name = "Junior Resident")]
        public string JuniorResident { get; set; }
        public string Daignosis { get; set; }
        public string Side { get; set; }
        [Display(Name = "Co-Morbity")]
        public string CoMorbity { get; set; }
       
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Title { get; set; }
        public string Dr_Name { get; set; }
        public bool IsActive { get; set; }
    }
}
