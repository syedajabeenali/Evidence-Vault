namespace EvidenceVault.DTO
{
    public class SearchEvidenceDto
    {
        public int? CaseId { get; set; }  // optional
        public string? CaseTitle { get; set; } // NEW - optional
        public string? UploadedByUserId { get; set; } // optional
        public string? FileType { get; set; } // optional ("image", "video", "document")
        public DateTime? UploadedAfter { get; set; } // optional
        public DateTime? UploadedBefore { get; set; } // optional
    }
}
