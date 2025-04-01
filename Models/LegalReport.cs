using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EvidenceVault.Models
{
    public class LegalReport
    {
        [Key]
        public int ReportID { get; set; } 

        [ForeignKey("Case")]
        public int CaseID { get; set; } 

        [ForeignKey("User")]
        public int? GeneratedBy { get; set; } 

        [Required]
        [MaxLength(500)]
        public string ReportFilePath { get; set; } 

        public DateTime GeneratedAt { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        [ForeignKey("DeletedBy")]
        public User? DeletedByUser { get; set; }
        public DateTime DeletedAt { get; set; }  = DateTime.Now;

        // Navigation properties 
        public virtual Case Case { get; set; }
        public virtual User User { get; set; }
    }
}
