using Microsoft.EntityFrameworkCore;
using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgenciaToursAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DestinosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDestinos()
        {
            var destinos = _context.Destinos
                .Include(d => d.Pais)
                .ToList();

            return Ok(destinos);
        }

        [HttpGet("{id}")]
        public IActionResult GetDestino(int id)
        {
            var destino = _context.Destinos.Find(id);

            if (destino == null)
            {
                return NotFound();
            }

            return Ok(destino);
        }

        [HttpPost]
        public IActionResult CrearDestino(Destino destino)
        {
            _context.Destinos.Add(destino);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDestino), new { id = destino.Id }, destino);
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarDestino(int id, Destino destino)
        {
            if (id != destino.Id)
            {
                return BadRequest();
            }

            _context.Destinos.Update(destino);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarDestino(int id)
        {
            var destino = _context.Destinos.Find(id);

            if (destino == null)
            {
                return NotFound();
            }

            _context.Destinos.Remove(destino);
            _context.SaveChanges();

            return NoContent();
        }
    }
}