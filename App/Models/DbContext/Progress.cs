using App.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models.DbContext
{
    public class Progress
    {
        [Key]
        public long Id { get; set; }
        public long PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; }
        [DataType(DataType.Date)]
        public string Date { get; set; }
        public string Cc { get; set; }
        public string GeneralCondition { get; set; }
        public string Vitals { get; set; }
        public string PR { get; set; }
        public string BP { get; set; }
        public string RR { get; set; }
        public string SpO2 { get; set; }
        public string Temp { get; set; }
        public string GeneralExamination { get; set; }
        public string CNS { get; set; }
        public string CVS { get; set; }
        public string RS { get; set; }
        public string PA { get; set; }
        public string LocalExamination { get; set; }
        public string Drains { get; set; }
        public string Urine { get; set; }
        public string Advice { get; set; }
        public string Remark { get; set; }
    }
}
