using EvidenceVault.Models;
using Microsoft.EntityFrameworkCore;

namespace EvidenceVault.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Evidence> Evidences { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<LegalReport> LegalReports { get; set; }


    }
}
