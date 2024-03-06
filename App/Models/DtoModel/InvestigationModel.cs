using App.Models.EntityModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.DtoModel
{
    public class InvestigationModel
    {

        public long Id { get; set; }
        public long PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; }

        [DataType(DataType.Date)]
        public string Day { get; set; }

        //CBC
        public string HB { get; set; }
        public string TLC { get; set; }
        public string PLT { get; set; }

        //RFT
        public string SGeat { get; set; }
        public string BUN { get; set; }

        //BLOOD SUGAR
        public string Fasting { get; set; }
        public string PP { get; set; }
        public string Random { get; set; }

        //  LFT
        public string TotalBil { get; set; }
        public string DirectBil { get; set; }
        public string AlkPhosphate { get; set; }
        public string SGDT { get; set; } //SGOT
        public string SGPT { get; set; }

        // TFT
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string TSH { get; set; }
        public string FT3 { get; set; }
        public string FT4 { get; set; }

        //SERUM ELECTROLYTES
        public string Sodium { get; set; }
        public string Potassium { get; set; }
        public string Calcium { get; set; }

        //PT-INR IMG

        public string PT { get; set; }
        public string INR { get; set; }

        //LIPID PROFILE
        public string Cholesterol { get; set; }
        public string Triglyceride { get; set; }
        public string HDL { get; set; }
        public string LDL { get; set; }

        //URINE RM IMG
        public string Blood { get; set; }
        public string PusCell { get; set; }
        public string EpithelialCell { get; set; }
        public string Crystals { get; set; }
        public string Sugar { get; set; }
        public string Color { get; set; }
        public string Appearance { get; set; }
        public string Albumin { get; set; }

        public string ABG { get; set; }

        public string USG { get; set; }
        public string SONOMMOGRAPHY { get; set; }
        public string CECT { get; set; }
        public string MRI { get; set; }
        public string FNAC { get; set; }
        public string TrucutBiopsy { get; set; }
        public string ReceptorStatus { get; set; }
        public string MRCP { get; set; }
        public string ERCP { get; set; }
        public string EndoscopyUpperGI { get; set; }
        public string EndoscopyLowerGI { get; set; }
        public string PETCT { get; set; }
        public string TumorMarkers { get; set; }
        public string IVP { get; set; }
        public string MCU { get; set; }
        public string RGU { get; set; } //Cystoscopy

        public string OtherO { get; set; }
        public string OtherT { get; set; }
        public string OtherTh { get; set; }
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }

    }
}
