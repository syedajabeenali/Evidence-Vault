using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EvidenceVault.Enums;

namespace EvidenceVault.Models
{
    public class AuditLog
    {
        [Key]
        public int LogID { get; set; } 

        [ForeignKey("User")]
        public int? UserID { get; set; }

        [ForeignKey("Evidence")]
        public int EvidenceID { get; set; }

        [Required]
        [EnumDataType(typeof(UserRole))]
        public string Action { get; set; } 

        public DateTime Timestamp { get; set; } = DateTime.Now;

        // Navigation properties 
        public virtual User User { get; set; }
        public virtual Evidence Evidence { get; set; }
    }
}
