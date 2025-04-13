using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EvidenceVault.Models
{
    public class Evidence
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvidenceID { get; set; }

        [ForeignKey("Case")]
        public int CaseID { get; set; }

        [ForeignKey("ApplicationUser")]
       public string? UploadedBy { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        [StringLength(500)]
        public string FilePath { get; set; }

        [Required]
        [StringLength(50)]
        public string FileType { get; set; } // Ensure only ('image', 'video', 'document') are allowed

        public DateTime UploadedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual Case Case { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
