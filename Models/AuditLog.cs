using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EvidenceVault.Enums;

namespace EvidenceVault.Models
{
    public class AuditLog
    {
        [Key]
        public int LogID { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? UserID { get; set; }

        [ForeignKey("Evidence")]
        public int EvidenceID { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public string Action { get; set; } 

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Navigation properties 
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Evidence Evidence { get; set; }
    }
}
