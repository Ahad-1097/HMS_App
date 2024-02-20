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
        public string OpertingSurgeon { get; set; }
        public string Position { get; set; }
        public string Anaesthesia { get; set; }
        public string PreoperativeDiagnosis { get; set; }
        public string OperationTitle { get; set; }
        public string Findings { get; set; }
        public string Duration { get; set; }
        public string StepsOfOperation { get; set; }
        public string Antibiotics { get; set; }
        public string SpecimensSentFor { get; set; }
        public string PostOperativeInstructions { get; set; }
        public string PerOPImage { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }

    }
}
