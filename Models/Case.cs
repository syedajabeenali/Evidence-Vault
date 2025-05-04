using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EvidenceVault.Models
{
    public class Case
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CaseID { get; set; }

        [Required]
        [StringLength(255)]
        public string CaseTitle { get; set; }

        public string Description { get; set; }

        [ForeignKey(nameof(User))]
        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Property
        public virtual ApplicationUser User { get; set; }
    }
}
