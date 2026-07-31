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
    public class ToursController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToursController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTours()
        {
            try
            {
                var tours = _context.Tours
                    .Include(t => t.Pais)
                    .Include(t => t.Destino)
                    .Include(t => t.CategoriaTour)
                    .Include(t => t.GuiaTuristico)
                    .Include(t => t.Transporte)
                    .ToList();

                return Ok(tours);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al consultar los tours."
                );
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTour(int id)
        {
            try
            {
                var tour = _context.Tours
                    .Include(t => t.Pais)
                    .Include(t => t.Destino)
                    .Include(t => t.CategoriaTour)
                    .Include(t => t.GuiaTuristico)
                    .Include(t => t.Transporte)
                    .FirstOrDefault(t => t.Id == id);

                if (tour == null)
                {
                    return NotFound("El tour no existe.");
                }

                return Ok(tour);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al consultar el tour."
                );
            }
        }

        [HttpPost]
        public IActionResult PostTour(Tour tour)
        {
            try
            {
                if (!_context.Paises.Any(p => p.Id == tour.PaisId))
                {
                    return BadRequest("El país indicado no existe.");
                }

                if (!_context.Destinos.Any(d => d.Id == tour.DestinoId))
                {
                    return BadRequest("El destino indicado no existe.");
                }

                if (!_context.CategoriaTours.Any(c => c.Id == tour.CategoriaTourId))
                {
                    return BadRequest("La categoría indicada no existe.");
                }

                if (!_context.GuiasTuristicos.Any(g => g.Id == tour.GuiaTuristicoId))
                {
                    return BadRequest("El guía turístico indicado no existe.");
                }

                if (!_context.Transportes.Any(t => t.Id == tour.TransporteId))
                {
                    return BadRequest("El transporte indicado no existe.");
                }

                var destinoPertenecePais = _context.Destinos.Any(
                    d => d.Id == tour.DestinoId &&
                         d.PaisId == tour.PaisId
                );

                if (!destinoPertenecePais)
                {
                    return BadRequest(
                        "El destino indicado no pertenece al país seleccionado."
                    );
                }

                CalcularDatosTour(tour);

                _context.Tours.Add(tour);
                _context.SaveChanges();

                return CreatedAtAction(
                    nameof(GetTour),
                    new { id = tour.Id },
                    tour
                );
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error al guardar el tour en la base de datos."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al crear el tour."
                );
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutTour(int id, Tour tour)
        {
            try
            {
                if (id != tour.Id)
                {
                    return BadRequest(
                        "El Id de la URL no coincide con el Id del tour."
                    );
                }

                var tourExiste = _context.Tours.Any(t => t.Id == id);

                if (!tourExiste)
                {
                    return NotFound("El tour no existe.");
                }

                if (!_context.Paises.Any(p => p.Id == tour.PaisId))
                {
                    return BadRequest("El país indicado no existe.");
                }

                if (!_context.Destinos.Any(d => d.Id == tour.DestinoId))
                {
                    return BadRequest("El destino indicado no existe.");
                }

                if (!_context.CategoriaTours.Any(c => c.Id == tour.CategoriaTourId))
                {
                    return BadRequest("La categoría indicada no existe.");
                }

                if (!_context.GuiasTuristicos.Any(g => g.Id == tour.GuiaTuristicoId))
                {
                    return BadRequest("El guía turístico indicado no existe.");
                }

                if (!_context.Transportes.Any(t => t.Id == tour.TransporteId))
                {
                    return BadRequest("El transporte indicado no existe.");
                }

                var destinoPertenecePais = _context.Destinos.Any(
                    d => d.Id == tour.DestinoId &&
                         d.PaisId == tour.PaisId
                );

                if (!destinoPertenecePais)
                {
                    return BadRequest(
                        "El destino indicado no pertenece al país seleccionado."
                    );
                }

                CalcularDatosTour(tour);

                _context.Tours.Update(tour);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error al actualizar el tour en la base de datos."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al actualizar el tour."
                );
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarTour(int id)
        {
            try
            {
                var tour = _context.Tours.Find(id);

                if (tour == null)
                {
                    return NotFound("El tour no existe.");
                }

                _context.Tours.Remove(tour);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "No se puede eliminar el tour porque tiene reservas relacionadas."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al eliminar el tour."
                );
            }
        }

        private void CalcularDatosTour(Tour tour)
        {
            tour.Itbis = Math.Round(
                tour.Precio * 0.18m,
                2
            );

            DateTime fechaHoraInicio =
                tour.Fecha.Date + tour.Hora;

            tour.FechaHoraFin = fechaHoraInicio
                .AddDays(tour.DuracionDias)
                .AddHours(tour.DuracionHoras);

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