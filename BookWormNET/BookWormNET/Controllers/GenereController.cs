using BookWormNET.Services.Implementation;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/generes")]
    public class GenereController : ControllerBase
    {
        private readonly IGenereService _service;

        public GenereController(IGenereService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var genere = _service.GetById(id);
            if (genere == null)
                return NotFound("Genere not found");

            return Ok(genere);
        }
    }
}