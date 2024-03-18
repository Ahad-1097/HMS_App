using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models.EntityModels
{
    public class Investigation
    {
        [Key]
        public long Id { get; set; }
        public long PatientID { get; set; }
        [ForeignKey(nameof(PatientID))]
        public Patient Patient { get; set; }

        public string Day { get; set; }

        //CBC
        public string HB { get; set; }
        public string TLC { get; set; }
        public string PLT { get; set; }

        //RFT
        [Display(Name = "S.Creat")]
        public string SGeat { get; set; }
        public string BUN { get; set; }

        //BLOOD SUGAR
        public string Fasting { get; set; }
        public string PP { get; set; }
        public string Random { get; set; }

        //  LFT
        [Display(Name = "Total Bil")]
        public string TotalBil { get; set; }
        [Display(Name = "Direct Bil")]
        public string DirectBil { get; set; }
        [Display(Name = "ALK Phosphate")]
        public string AlkPhosphate { get; set; }
        [Display(Name = "SGOT")]
        public string SGDT { get; set; }
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
        [Display(Name = "Pus Cell")]
        public string PusCell { get; set; }
        [Display(Name = "Epithelial Cell")]
        public string EpithelialCell { get; set; }
        public string Crystals { get; set; }
        public string Sugar { get; set; }
        public string Color { get; set; }
        public string Appearance { get; set; }
        public string Albumin { get; set; }

        public string ABG { get; set; }

        public string USG { get; set; }
        [Display(Name = "Sonography")]
        public string SONOMMOGRAPHY { get; set; }
        public string CECT { get; set; }
        public string MRI { get; set; }
        public string FNAC { get; set; }
        [Display(Name = "Trucut Biopsy")]
        public string TrucutBiopsy { get; set; }
        [Display(Name = "Receptor Status")]
        public string ReceptorStatus { get; set; }
        public string MRCP { get; set; }
        public string ERCP { get; set; }
        [Display(Name = "Endoscopy-Upper GI")]
        public string EndoscopyUpperGI { get; set; }
        [Display(Name = "Endoscopy-Lower GI")]
        public string EndoscopyLowerGI { get; set; }
        [Display(Name = "PET-CT")]
        public string PETCT { get; set; }
        [Display(Name = "Tumor Markers")]
        public string TumorMarkers { get; set; }
        public string IVP { get; set; }
        [Display(Name = "MCU + RGU")]
        public string MCU { get; set; }
        [Display(Name = "Cystoscopy")]
        public string RGU { get; set; } //Cystoscopy

        [Display(Name = "Others")]
        public string OtherO { get; set; }

        [Display(Name = "Serum Amylase")]
        public string OtherT { get; set; } // Serum Amylase

        [Display(Name = "Serum Lipase")]
        public string OtherTh { get; set; }  // Serum lipase
        public string CreatedBy { get; set; }
        public string UpdateBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsActive { get; set; }

    }
}
