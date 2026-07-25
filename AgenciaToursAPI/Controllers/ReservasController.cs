using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetReservas()
        {
            var reservas = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Tour)
                .ToList();

            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public IActionResult GetReserva(int id)
        {
            var reserva = _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Tour)
                .FirstOrDefault(r => r.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }

        [HttpPost]
        public IActionResult CrearReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetReserva),
                new { id = reserva.Id },
                reserva
            );
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarReserva(int id, Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return BadRequest();
            }

            _context.Reservas.Update(reserva);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarReserva(int id)
        {
            var reserva = _context.Reservas.Find(id);

            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            _context.SaveChanges();

            return NoContent();
        }
    }
}