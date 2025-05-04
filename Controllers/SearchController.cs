using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvidenceVault.Data;
using EvidenceVault.DTO;
using EvidenceVault.Models;
using Microsoft.AspNetCore.Authorization;

namespace EvidenceVault.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("evidence")]
       /* [Authorize(Roles = "PoliceOfficer,Investigator,Admin")]*/
        public async Task<IActionResult> SearchEvidence([FromBody] SearchEvidenceDto searchParams)
        {
            var query = _context.Evidences.Include(e => e.Case).Include(e => e.User).AsQueryable();

            if (searchParams.CaseId.HasValue)
            {
                query = query.Where(e => e.CaseID == searchParams.CaseId);
            }

            if (!string.IsNullOrEmpty(searchParams.CaseTitle))
            {
                query = query.Where(e => e.Case.CaseTitle.Contains(searchParams.CaseTitle));
            }

            if (!string.IsNullOrEmpty(searchParams.UploadedByUserId))
            {
                query = query.Where(e => e.UploadedBy == searchParams.UploadedByUserId);
            }

            if (!string.IsNullOrEmpty(searchParams.FileType))
            {
                query = query.Where(e => e.FileType == searchParams.FileType.ToLower());
            }

            if (searchParams.UploadedAfter.HasValue)
            {
                query = query.Where(e => e.UploadedAt >= searchParams.UploadedAfter.Value);
            }

            if (searchParams.UploadedBefore.HasValue)
            {
                query = query.Where(e => e.UploadedAt <= searchParams.UploadedBefore.Value);
            }

            var evidences = await query
                .Select(e => new
                {
                    e.EvidenceID,
                    e.FileName,
                    e.FileType,
                    e.FilePath,
                    e.UploadedAt,
                    CaseTitle = e.Case.CaseTitle,
                    UploadedByUser = e.User.Name
                })
                .ToListAsync();

            return Ok(evidences);
        }


    }
}
