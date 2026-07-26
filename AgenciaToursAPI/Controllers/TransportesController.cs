using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTransportes()
        {
            var transportes = _context.Transportes.ToList();

            return Ok(transportes);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransporte(int id)
        {
            var transporte = _context.Transportes.Find(id);

            if (transporte == null)
            {
                return NotFound("El transporte no existe.");
            }

            return Ok(transporte);
        }

        [HttpPost]
        public IActionResult PostTransporte(Transporte transporte)
        {
            try
            {
                var placaExiste = _context.Transportes.Any(
                    t => t.Placa == transporte.Placa
                );

                if (placaExiste)
                {
                    return BadRequest(
                        "Ya existe un transporte registrado con esa placa."
                    );
                }

                _context.Transportes.Add(transporte);
                _context.SaveChanges();

                return CreatedAtAction(
                    nameof(GetTransporte),
                    new { id = transporte.Id },
                    transporte
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al crear el transporte."
                );
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutTransporte(int id, Transporte transporte)
        {
            try
            {
                if (id != transporte.Id)
                {
                    return BadRequest(
                        "El Id de la URL no coincide con el Id del transporte."
                    );
                }

                var transporteExiste = _context.Transportes.Any(
                    t => t.Id == id
                );

                if (!transporteExiste)
                {
                    return NotFound("El transporte no existe.");
                }

                var placaDuplicada = _context.Transportes.Any(
                    t => t.Placa == transporte.Placa && t.Id != id
                );

                if (placaDuplicada)
                {
                    return BadRequest(
                        "Ya existe otro transporte registrado con esa placa."
                    );
                }

                _context.Transportes.Update(transporte);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al actualizar el transporte."
                );
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransporte(int id)
        {
            try
            {
                var transporte = _context.Transportes.Find(id);

                if (transporte == null)
                {
                    return NotFound("El transporte no existe.");
                }

                _context.Transportes.Remove(transporte);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "No se puede eliminar el transporte porque está relacionado con un tour."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al eliminar el transporte."
                );
            }
        }
    }
}