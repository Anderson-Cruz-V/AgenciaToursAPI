using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgenciaToursAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPaises()
        {
            var paises = _context.Paises.ToList();

            return Ok(paises);
        }

        [HttpGet("{id}")]
        public IActionResult GetPais(int id)
        {
            var pais = _context.Paises.Find(id);

            if (pais == null)
            {
                return NotFound();
            }

            return Ok(pais);
        }

        [HttpPost]
        public IActionResult CrearPais(Pais pais)
        {
            _context.Paises.Add(pais);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPais), new { id = pais.Id }, pais);
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarPais(int id, Pais pais)
        {
            if (id != pais.Id)
            {
                return BadRequest();
            }

            _context.Paises.Update(pais);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarPais(int id)
        {
            var pais = _context.Paises.Find(id);

            if (pais == null)
            {
                return NotFound();
            }

            _context.Paises.Remove(pais);
            _context.SaveChanges();

            return NoContent();
        }

    }
}

