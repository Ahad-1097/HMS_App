using App.Models.EntityModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.DbContext
{
    public class Operation
    {
        [Key]
        public long Id { get; set; }
        public long PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; }
        public string Dr_ID { get; set; }
        [DataType(DataType.Date)]
        public string Date { get; set; }
        public string Indication { get; set; }
        public string Anaesthetist { get; set; }
        [Display(Name = "Operting Surgeon(s)")]
        public string OpertingSurgeon { get; set; }
        public string Position { get; set; }
        public string Anaesthesia { get; set; }
        [Display(Name = "Pre-Operative Diagnosis")]
        public string PreoperativeDiagnosis { get; set; }
        [Display(Name = "Operation Title")]
        public string OperationTitle { get; set; }
        public string Findings { get; set; }
        public string Duration { get; set; }
        [Display(Name = "Steps Of Operation")]
        public string StepsOfOperation { get; set; }
        public string Antibiotics { get; set; }
        [Display(Name = "Specimens Sent For")]
        public string SpecimensSentFor { get; set; }
        [Display(Name = "Post-Operative Instructions")]
        public string PostOperativeInstructions { get; set; }
        [Display(Name = "PERHAPS IMAGE")]
        public string PerOPImage { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

    }
}
