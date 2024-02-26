using App.Models.EntityModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models.DtoModel
{
    public class CaseSheetModel
    {
        public long Id { get; set; }
        public long PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; }
        public string PRESENTING_COMPLAINTS { get; set; }
        public string HistoryOfPresentingIllness { get; set; }
        public string PastHistory { get; set; }
        public string PersonalHistory { get; set; }
        public string Diet { get; set; }
        public string Appetite { get; set; }
        public string Sleep { get; set; }
        public string Bowel { get; set; }
        public string Bladder { get; set; }
        public string Addiction { get; set; }
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
        public string RespiratorySystem { get; set; }
        public string CNS { get; set; }
        public string CVS { get; set; }
        public string PerAbdomen { get; set; }
        public string LocoregionalExam { get; set; }


        public string Remark { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string AddImage { get; set; }
        public IFormFile AddImageFile { get; set; }
    }
}
