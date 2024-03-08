using App.Models.DtoModel;
using App.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.DtoModel
{
    public class PatientModel
    {
        public long PatientID { get; set; }
        [Display(Name = "Patient Name")]
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DOA { get; set; }
        public long Address_ID { get; set; }
        [ForeignKey(nameof(Address_ID))]
        public Address Address { get; set; }
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
        [Display(Name = "Age")]
        public string OtherO { get; set; }
        public string OtherT { get; set; }
        public string OtherTh { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string SerialNumber { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Title { get; set; }
        public string SubCategoryTitle { get; set; }
        public string Dr_Name { get; set; }
        public SubCategoryModel subCategory { get; set; }
        public List<InvestigationModel> InvestigationModel { get; set; }
        public virtual InvestigationImagesModel InvestigationImagesModel { get; set; }

    }
}
