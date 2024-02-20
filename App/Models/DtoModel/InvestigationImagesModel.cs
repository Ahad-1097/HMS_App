using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace App.Models.DtoModel
{
    public class InvestigationImagesModel
    {
        public long Id { get; set; }
        public long InvestigationID { get; set; }
        public int PatientId { get; set; }
        public List<IFormFile> BloodSugar_img { get; set; }
        public List<IFormFile> TFT_img { get; set; }
        public List<IFormFile> USG_img { get; set; }
        public List<IFormFile> SONOMMOGRAPHY_img { get; set; }
        public List<IFormFile> CECT_img { get; set; }
        public List<IFormFile> MRI_img { get; set; }
        public List<IFormFile> FNAC_img { get; set; }
        public List<IFormFile> TrucutBiopsy_img { get; set; }
        public List<IFormFile> ReceptorStatus_img { get; set; }
        public List<IFormFile> MRCP_img { get; set; }
        public List<IFormFile> ERCP_img { get; set; }
        public List<IFormFile> EndoscopyUpperGI_img { get; set; }
        public List<IFormFile> EndoscopyLowerGI_img { get; set; }
        public List<IFormFile> PETCT_img { get; set; }
        public List<IFormFile> TumorMarkers_img { get; set; }
        public List<IFormFile> IVP_img { get; set; }
        public List<IFormFile> MCU_img { get; set; }
        public List<IFormFile> RGU_img { get; set; }
        public List<IFormFile> ABG_img { get; set; }
        public List<IFormFile> CBC_img { get; set; }
        public List<IFormFile> RFT_img { get; set; }
        public List<IFormFile> PTINR_img { get; set; }
        public List<IFormFile> LFT_img { get; set; }
        public List<IFormFile> LIPIDPROFILE_img { get; set; }
        [Display(Name = "Urine R/M img")]
        public List<IFormFile> UrineRM_img { get; set; }
        [Display(Name = "PT/DNR img")]
        public List<IFormFile> PTDNR_img { get; set; }
        public List<IFormFile> OtherO { get; set; }
        public List<IFormFile> OtherT { get; set; }
        public List<IFormFile> OtherTh { get; set; }
        public string Name { get; set; }
        public string ExtTtype { get; set; }
        public string Msg { get; set; }
    }
}
