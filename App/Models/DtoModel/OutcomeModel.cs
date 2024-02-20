using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models.DtoModel
{
    public class OutcomeModel
    {
        public long Id { get; set; }
        public long PatientID { get; set; }
        [DataType(DataType.Date)]
        public string Date { get; set; }
        public string Outcome { get; set; }

    }
}
