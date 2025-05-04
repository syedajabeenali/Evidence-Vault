using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EvidenceVault.DTO
{
    public class EvidenceUploadDto
    {
        [Required]
        public int CaseID { get; set; }

        [Required]
        public string UploadedByUserId { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
