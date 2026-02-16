using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/languages")]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_languageService.GetAll());
        }

        // GET: api/languages/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var language = _languageService.GetById(id);
            if (language == null)
                return NotFound("Language not found");

            return Ok(language);
        }
    }

}