using BookWormNET.Models;
using BookWormNET.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookWormNET.Controllers
{
    [ApiController]
    [Route("api/library-packages")]
    public class LibraryPackageController : ControllerBase
    {
        private readonly ILibraryPackageService _service;

        public LibraryPackageController(ILibraryPackageService service)
        {
            _service = service;
        }

        
        [HttpGet]
        public IActionResult GetAll()
        {
            var packages = _service.GetAllPackages();
            return Ok(packages);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var pkg = _service.GetPackageById(id);
            return Ok(pkg);
        }
    }
}