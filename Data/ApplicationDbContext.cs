using EvidenceVault.DTO;
using EvidenceVault.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EvidenceVault.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<LegalReport> LegalReports { get; set; }
    }
}
