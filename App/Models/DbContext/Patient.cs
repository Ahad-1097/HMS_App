using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.EntityModels
{
    public class Patient
    {
        [Key]
        public long PatientID { get; set; }
        public string DrId { get; set; } //this id used for CONSULTANTS
        public string CADSNumber { get; set; }
        [Display(Name = "OPD Number")]
        public string OPDNumber { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOA { get; set; }
        public long Address_ID { get; set; }
        [ForeignKey(nameof(Address_ID))]
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        public string AlternateNumber { get; set; }
        public string SeniorResident { get; set; }
        public string JuniorResident { get; set; }
        public string UserId { get; set; }
       
        public string Daignosis { get; set; }
        public string Side { get; set; }
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
    }
}
