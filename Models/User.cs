using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EvidenceVault.Enums;

namespace EvidenceVault.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }  

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        [EnumDataType(typeof(UserRole))] 
        public string Role { get; set; }

        public bool CreatedByAdmin { get; set; } = false;

        public int? CreatedByAdminID { get; set; }

        public bool IsSuperAdmin { get; set; } = false;

        [ForeignKey("CreatedByAdminID")]
        public User CreatedByAdminUser { get; set; } 
    }
}
