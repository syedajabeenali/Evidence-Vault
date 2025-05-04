using Microsoft.AspNetCore.Mvc;
using EvidenceVault.Models;
using EvidenceVault.DTO;
using Microsoft.AspNetCore.Authorization;
using EvidenceVault.Data;

namespace EvidenceVault.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaseController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
       /* [Authorize(Roles = "PoliceOfficer,Admin")]*/
        public async Task<IActionResult> CreateCase([FromBody] CreateCaseDto model)
        {
            var caseEntity = new Case
            {
                CaseTitle = model.CaseTitle,
                Description = model.Description,
                CreatedBy = model.CreatedByUserId
            };

            _context.Cases.Add(caseEntity);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Case created successfully.", caseId = caseEntity.CaseID });
        }

        [HttpGet("all")]
        /*[Authorize(Roles = "PoliceOfficer,Investigator,Admin")]*/
        public IActionResult GetAllCases()
        {
            var cases = _context.Cases.ToList();
            return Ok(cases);
        }

        [HttpGet("{id}")]
       /* [Authorize(Roles = "PoliceOfficer,Investigator,Admin")]*/
        public async Task<IActionResult> GetCaseById(int id)
        {
            var caseEntity = await _context.Cases.FindAsync(id);
            if (caseEntity == null) return NotFound();
            return Ok(caseEntity);
        }
    }
}
