using App.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models.DbContext
{
    public class Discharge
    {
        [Key]
        public long Id { get; set; }
        public long PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; }
        [DataType(DataType.Date)]
        public string DOA { get; set; }
        [DataType(DataType.Date)]
        public string DOD { get; set; }
        public string Diagnosis { get; set; }
        [Display(Name = "Case Summary")]
        public string CaseSummary { get; set; }
        public string Investigations { get; set; }
        [Display(Name = "Treatment Given")]
        public string TreatmentGiven { get; set; }
        [Display(Name = "Advice On discharge")]
        public string AdviceOndischarge { get; set; }
        public string SeniorResident { get; set; }
        public string JuniorResident { get; set; }
    }
}
