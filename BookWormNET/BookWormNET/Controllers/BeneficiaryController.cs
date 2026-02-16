


using BookWormNET.Models;
using BookWormNET.Services;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/beneficiaries")]
    public class BeneficiariesController : ControllerBase
    {
        private readonly IBeneficiaryService _beneficiaryService;

        // Constructor Injection (instead of @Autowired)
        public BeneficiariesController(IBeneficiaryService beneficiaryService)
        {
            _beneficiaryService = beneficiaryService;
        }

        // POST: api/beneficiaries
        [HttpPost]
        public ActionResult<Beneficiary> CreateBeneficiary([FromBody] Beneficiary beneficiary)
        {
            var result = _beneficiaryService.CreateBeneficiary(beneficiary);
            return Ok(result);
        }

        // PUT: api/beneficiaries/{id}
        [HttpPut("{id}")]
        public ActionResult<Beneficiary> UpdateBeneficiary(int id, [FromBody] Beneficiary beneficiary)
        {
            var result = _beneficiaryService.UpdateBeneficiary(id, beneficiary);
            return Ok(result);
        }

        // GET: api/beneficiaries/{id}
        [HttpGet("{id}")]
        public ActionResult<Beneficiary> GetBeneficiaryById(int id)
        {
            var beneficiary = _beneficiaryService.GetBeneficiaryById(id);
            if (beneficiary == null)
                return NotFound();

            return Ok(beneficiary);
        }

        // GET: api/beneficiaries
        [HttpGet]
        public ActionResult<IEnumerable<Beneficiary>> GetAllBeneficiaries()
        {
            return Ok(_beneficiaryService.GetAllBeneficiaries());
        }

        // DELETE: api/beneficiaries/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteBeneficiary(int id)
        {
            _beneficiaryService.DeleteBeneficiary(id);
            return Ok($"Beneficiary deleted successfully with id: {id}");
        }
    }
}
