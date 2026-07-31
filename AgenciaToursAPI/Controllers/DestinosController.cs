using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
    [Authorize]
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
        public IActionResult PostDestino(Destino destino)
        {
            var paisExiste = _context.Paises.Any(p => p.Id == destino.PaisId);

            if (!paisExiste)
            {
                return BadRequest("El país indicado no existe.");
            }

            _context.Destinos.Add(destino);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDestino), new { id = destino.Id }, destino);
        }

        [HttpPut("{id}")]
        public IActionResult PutDestino(int id, Destino destino)
        {
            if (id != destino.Id)
            {
                return BadRequest("El Id de la URL no coincide con el Id del destino.");
            }

            if (!_context.Paises.Any(p => p.Id == destino.PaisId))
            {
                return BadRequest("El país indicado no existe.");
            }

            var destinoExiste = _context.Destinos.Any(d => d.Id == id);

            if (!destinoExiste)
            {
                return NotFound("El destino no existe.");
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