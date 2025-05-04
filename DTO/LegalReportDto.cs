namespace EvidenceVault.DTO
{
    public class LegalReportDto
    {
        public int ReportID { get; set; }
        public int CaseID { get; set; }
        public string CaseTitle { get; set; }
        public string GeneratedByName { get; set; }
        public string ReportFilePath { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
