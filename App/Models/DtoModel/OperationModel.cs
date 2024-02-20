using App.Models.EntityModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace App.Models.DtoModel
{
    public class OperationModel
    {
        public long Id { get; set; }
        public long PatientID { get; set; }
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
        public IFormFile PerOPImageFile { get; set; }

    }
}
