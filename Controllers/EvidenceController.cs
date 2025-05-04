using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EvidenceVault.Data;
using EvidenceVault.DTO;
using EvidenceVault.Models;
using Microsoft.AspNetCore.Authorization;

namespace EvidenceVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvidenceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EvidenceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        /*[Authorize(Roles = "PoliceOfficer,Admin")]*/
        public async Task<IActionResult> UploadEvidence([FromForm] EvidenceUploadDto model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            // Check if case exists
            var caseExists = await _context.Cases.AnyAsync(c => c.CaseID == model.CaseID);
            if (!caseExists)
            {
                return BadRequest(new { message = "Invalid CaseID." });
            }

            // Save file to server (you can improve this with better path management)
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedEvidence");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }

            // Determine file type
            var fileExtension = Path.GetExtension(model.File.FileName).ToLower();
            string fileType = fileExtension switch
            {
                ".jpg" or ".jpeg" or ".png" => "image",
                ".mp4" or ".mov" => "video",
                ".pdf" or ".docx" or ".txt" => "document",
                _ => "unknown"
            };

            if (fileType == "unknown")
            {
                return BadRequest(new { message = "Unsupported file type." });
            }

            // Save Evidence record
            var evidence = new Evidence
            {
                CaseID = model.CaseID,
                UploadedBy = model.UploadedByUserId,
                FileName = model.File.FileName,
                FilePath = filePath,
                FileType = fileType,
            };

            _context.Evidences.Add(evidence);
            await _context.SaveChangesAsync();

            // Create AuditLog record
            var auditLog = new AuditLog
            {
                UserID = model.UploadedByUserId,
                EvidenceID = evidence.EvidenceID,
                Action = "Upload",
                Timestamp = DateTime.Now
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Evidence uploaded successfully.", evidenceId = evidence.EvidenceID });
        }

        [HttpGet("bycase/{caseId}")]
        /*[Authorize(Roles = "PoliceOfficer,Investigator,Admin")]*/
        public async Task<IActionResult> GetEvidenceByCaseId(int caseId)
        {
            var evidenceList = await _context.Evidences
                .Where(e => e.CaseID == caseId)
                .Select(e => new
                {
                    e.EvidenceID,
                    e.FileName,
                    e.FileType,
                    e.UploadedAt,
                    e.UploadedBy
                })
                .ToListAsync();

            return Ok(evidenceList);
        }

        [HttpGet("{evidenceId}")]
        /*[Authorize(Roles = "PoliceOfficer,Investigator,Admin")]*/
        public async Task<IActionResult> GetEvidenceById(int evidenceId)
        {
            var evidence = await _context.Evidences.FindAsync(evidenceId);

            if (evidence == null)
                return NotFound(new { message = "Evidence not found." });

            return Ok(evidence);
        }

        [HttpGet("auditlogs/{evidenceId}")]
        /*[Authorize(Roles = "Admin")]*/
        public async Task<IActionResult> GetAuditLogsForEvidence(int evidenceId)
        {
            var logs = await _context.AuditLogs
                .Where(a => a.EvidenceID == evidenceId)
                .Select(a => new
                {
                    a.LogID,
                    a.UserID,
                    a.Action,
                    a.Timestamp
                })
                .ToListAsync();

            return Ok(logs);
        }
    }
}
