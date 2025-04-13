using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EvidenceVault.Enums;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenceVault.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public bool IsSuperAdmin { get; set; } = false;
        public bool ? CreatedByAdmin { get; set; } = false;
        public string ? CreatedByAdminID { get; set; }

        [ForeignKey("CreatedByAdminID")]
        public ApplicationUser CreatedByAdminUser { get; set; }
    }
}
