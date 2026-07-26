using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetodosPagoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MetodosPagoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMetodosPago()
        {
            var metodosPago = _context.MetodosPago.ToList();

            return Ok(metodosPago);
        }

        [HttpGet("{id}")]
        public IActionResult GetMetodoPago(int id)
        {
            var metodoPago = _context.MetodosPago.Find(id);

            if (metodoPago == null)
            {
                return NotFound("El método de pago no existe.");
            }

            return Ok(metodoPago);
        }

        [HttpPost]
        public IActionResult PostMetodoPago(MetodoPago metodoPago)
        {
            try
            {
                var nombreExiste = _context.MetodosPago.Any(
                    m => m.Nombre == metodoPago.Nombre
                );

                if (nombreExiste)
                {
                    return BadRequest(
                        "Ya existe un método de pago con ese nombre."
                    );
                }

                _context.MetodosPago.Add(metodoPago);
                _context.SaveChanges();

                return CreatedAtAction(
                    nameof(GetMetodoPago),
                    new { id = metodoPago.Id },
                    metodoPago
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al crear el método de pago."
                );
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutMetodoPago(int id, MetodoPago metodoPago)
        {
            try
            {
                if (id != metodoPago.Id)
                {
                    return BadRequest(
                        "El Id de la URL no coincide con el Id del método de pago."
                    );
                }

                var metodoExiste = _context.MetodosPago.Any(
                    m => m.Id == id
                );

                if (!metodoExiste)
                {
                    return NotFound("El método de pago no existe.");
                }

                var nombreDuplicado = _context.MetodosPago.Any(
                    m => m.Nombre == metodoPago.Nombre && m.Id != id
                );

                if (nombreDuplicado)
                {
                    return BadRequest(
                        "Ya existe otro método de pago con ese nombre."
                    );
                }

                _context.MetodosPago.Update(metodoPago);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al actualizar el método de pago."
                );
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMetodoPago(int id)
        {
            try
            {
                var metodoPago = _context.MetodosPago.Find(id);

                if (metodoPago == null)
                {
                    return NotFound("El método de pago no existe.");
                }

                _context.MetodosPago.Remove(metodoPago);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "No se puede eliminar el método de pago porque está relacionado con una reserva."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al eliminar el método de pago."
                );
            }
        }
    }
}