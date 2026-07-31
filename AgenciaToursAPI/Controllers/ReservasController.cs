using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
    [Authorize]
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
            try
            {
                var reservas = _context.Reservas
                    .Include(r => r.Cliente)
                    .Include(r => r.Tour)
                        .ThenInclude(t => t!.Transporte)
                    .Include(r => r.MetodoPago)
                    .ToList();

                return Ok(reservas);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al consultar las reservas."
                );
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetReserva(int id)
        {
            try
            {
                var reserva = _context.Reservas
                    .Include(r => r.Cliente)
                    .Include(r => r.Tour)
                        .ThenInclude(t => t!.Transporte)
                    .Include(r => r.MetodoPago)
                    .FirstOrDefault(r => r.Id == id);

                if (reserva == null)
                {
                    return NotFound("La reserva no existe.");
                }

                return Ok(reserva);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al consultar la reserva."
                );
            }
        }

        [HttpPost]
        public IActionResult PostReserva(Reserva reserva)
        {
            try
            {
                if (!_context.Clientes.Any(c => c.Id == reserva.ClienteId))
                {
                    return BadRequest("El cliente indicado no existe.");
                }

                var tour = _context.Tours
                    .Include(t => t.Transporte)
                    .FirstOrDefault(t => t.Id == reserva.TourId);

                if (tour == null)
                {
                    return BadRequest("El tour indicado no existe.");
                }

                var metodoPago = _context.MetodosPago
                    .FirstOrDefault(m => m.Id == reserva.MetodoPagoId);

                if (metodoPago == null)
                {
                    return BadRequest("El método de pago indicado no existe.");
                }

                if (!metodoPago.Activo)
                {
                    return BadRequest("El método de pago indicado no está activo.");
                }

                ActualizarEstadoTour(tour);

                if (tour.Estado == "Finalizado")
                {
                    return BadRequest(
                        "No se puede realizar una reserva para un tour finalizado."
                    );
                }

                if (tour.Estado == "En proceso")
                {
                    return BadRequest(
                        "No se puede realizar una reserva para un tour que ya está en proceso."
                    );
                }

                if (tour.Transporte == null)
                {
                    return BadRequest(
                        "El tour no tiene un transporte asignado."
                    );
                }

                var personasReservadas = _context.Reservas
                    .Where(r =>
                        r.TourId == reserva.TourId &&
                        r.Estado != "Cancelada"
                    )
                    .Sum(r => (int?)r.CantidadPersonas) ?? 0;

                var capacidadDisponible =
                    tour.Transporte.Capacidad - personasReservadas;

                if (reserva.CantidadPersonas > capacidadDisponible)
                {
                    return BadRequest(
                        $"La cantidad solicitada supera la capacidad disponible. " +
                        $"Solo quedan {capacidadDisponible} espacios."
                    );
                }

                reserva.FechaReserva = DateTime.Now;

                reserva.Total = Math.Round(
                    (tour.Precio + tour.Itbis) *
                    reserva.CantidadPersonas,
                    2
                );

                reserva.Estado = "Confirmada";

                _context.Reservas.Add(reserva);
                _context.SaveChanges();

                return CreatedAtAction(
                    nameof(GetReserva),
                    new { id = reserva.Id },
                    reserva
                );
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error al guardar la reserva en la base de datos."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al crear la reserva."
                );
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutReserva(int id, Reserva reserva)
        {
            try
            {
                if (id != reserva.Id)
                {
                    return BadRequest(
                        "El Id de la URL no coincide con el Id de la reserva."
                    );
                }

                var reservaActual = _context.Reservas
                    .AsNoTracking()
                    .FirstOrDefault(r => r.Id == id);

                if (reservaActual == null)
                {
                    return NotFound("La reserva no existe.");
                }

                if (!_context.Clientes.Any(c => c.Id == reserva.ClienteId))
                {
                    return BadRequest("El cliente indicado no existe.");
                }

                var tour = _context.Tours
                    .Include(t => t.Transporte)
                    .FirstOrDefault(t => t.Id == reserva.TourId);

                if (tour == null)
                {
                    return BadRequest("El tour indicado no existe.");
                }

                var metodoPago = _context.MetodosPago
                    .FirstOrDefault(m => m.Id == reserva.MetodoPagoId);

                if (metodoPago == null)
                {
                    return BadRequest("El método de pago indicado no existe.");
                }

                if (!metodoPago.Activo)
                {
                    return BadRequest("El método de pago indicado no está activo.");
                }

                ActualizarEstadoTour(tour);

                if (tour.Estado == "Finalizado")
                {
                    return BadRequest(
                        "No se puede actualizar una reserva para un tour finalizado."
                    );
                }

                if (tour.Estado == "En proceso")
                {
                    return BadRequest(
                        "No se puede actualizar una reserva para un tour que ya está en proceso."
                    );
                }

                if (tour.Transporte == null)
                {
                    return BadRequest(
                        "El tour no tiene un transporte asignado."
                    );
                }

                var personasReservadas = _context.Reservas
                    .Where(r =>
                        r.TourId == reserva.TourId &&
                        r.Id != id &&
                        r.Estado != "Cancelada"
                    )
                    .Sum(r => (int?)r.CantidadPersonas) ?? 0;

                var capacidadDisponible =
                    tour.Transporte.Capacidad - personasReservadas;

                if (reserva.CantidadPersonas > capacidadDisponible)
                {
                    return BadRequest(
                        $"La cantidad solicitada supera la capacidad disponible. " +
                        $"Solo quedan {capacidadDisponible} espacios."
                    );
                }

                reserva.FechaReserva = reservaActual.FechaReserva;

                reserva.Total = Math.Round(
                    (tour.Precio + tour.Itbis) *
                    reserva.CantidadPersonas,
                    2
                );

                if (string.IsNullOrWhiteSpace(reserva.Estado))
                {
                    reserva.Estado = reservaActual.Estado;
                }

                _context.Reservas.Update(reserva);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error al actualizar la reserva en la base de datos."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al actualizar la reserva."
                );
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarReserva(int id)
        {
            try
            {
                var reserva = _context.Reservas.Find(id);

                if (reserva == null)
                {
                    return NotFound("La reserva no existe.");
                }

                _context.Reservas.Remove(reserva);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error al eliminar la reserva de la base de datos."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al eliminar la reserva."
                );
            }
        }

        private void ActualizarEstadoTour(Tour tour)
        {
            DateTime fechaHoraInicio =
                tour.Fecha.Date + tour.Hora;

            DateTime fechaHoraActual = DateTime.Now;

            if (fechaHoraActual < fechaHoraInicio)
            {
                tour.Estado = "Vigente";
            }
            else if (fechaHoraActual <= tour.FechaHoraFin)
            {
                tour.Estado = "En proceso";
            }
            else
            {
                tour.Estado = "Finalizado";
            }
        }
    }
}