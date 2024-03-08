using App.Models.EntityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace App.Models.DbContext
{
    public class CaseSheet
    {

        [Key]
        public long Id { get; set; }
        public long PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; }
        [Display(Name = "PRESENTING COMPLAINTS")]
        public string PRESENTING_COMPLAINTS { get; set; }
        [Display(Name = "History Of Presenting Illness")]
        public string HistoryOfPresentingIllness { get; set; }
        [Display(Name = "Past History")]
        public string PastHistory { get; set; }
        [Display(Name = "Personal History")]
        public string PersonalHistory { get; set; }
        public string Diet { get; set; }
        public string Appetite { get; set; }
        public string Sleep { get; set; }
        public string Bowel { get; set; }
        public string Bladder { get; set; }
        public string Addiction { get; set; }
        [Display(Name = "Family History")]
        public string FamilyHistory { get; set; }

        //Vitals
      
        public string BP { get; set; }
        public string PR { get; set; }
        public string RR { get; set; }
        public string Temp { get; set; }
        public string SpO2 { get; set; }

        //GENERAL EXAMINATION
       
        public string Pallor { get; set; }
        public string Icterus { get; set; }
        public string Cyanosis { get; set; }
        public string Clubbing { get; set; }
        public string Edema { get; set; }
        public string Lymphadenopathy { get; set; }

        //SYSTEMIC EXAMINATION
        [Display(Name = "Respiratory System")]
        public string RespiratorySystem { get; set; }
        public string CNS { get; set; }
        public string CVS { get; set; }
        public string PerAbdomen { get; set; }
        [Display(Name = "Locoregional Exam")]
        public string LocoregionalExam { get; set; }
        
       
        public string Remark { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

    }
}
