//using Microsoft.AspNetCore.Mvc;

//namespace BookWormNET.Controllers
//{
//    public class ProductBeneficiaryController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using BookWormNET.Models;
using BookWormNET.Services;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookWormNET.Controllers
{
    [Route("api/product-beneficiaries")]
    [ApiController]
    public class ProductBeneficiaryController : ControllerBase
    {
        private readonly IProductBeneficiaryService _service;

        public ProductBeneficiaryController(IProductBeneficiaryService service)
        {
            _service = service;
        }

        // GET: api/ProductBeneficiary
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/ProductBeneficiary/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetById(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/ProductBeneficiary
        [HttpPost]
        public IActionResult Create([FromBody] ProductBeneficiary productBeneficiary)
        {
            var created = _service.Add(productBeneficiary);
            return CreatedAtAction(nameof(GetById),
                new { id = created.ProdbenId }, created);
        }

        // PUT: api/ProductBeneficiary/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] ProductBeneficiary productBeneficiary)
        {
            var updated = _service.Update(id, productBeneficiary);
            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        // DELETE: api/ProductBeneficiary/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _service.Delete(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
