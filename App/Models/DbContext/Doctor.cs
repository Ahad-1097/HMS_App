using System.ComponentModel.DataAnnotations;

namespace App.Models.EntityModels
{
    public class Doctor
    {
        [Key]
        public long Dr_ID { get; set; }
        public string DrId { get; set; }
        public string Dr_Name { get; set; }
        public string Specialty { get; set; }
        public string Education { get; set; }
        public string Certifications { get; set; }
        public int Experience { get; set; }
        public string Schedule { get; set; }
        public string Status { get; set; }
        public string ContactInformation { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }

    }
}
