using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
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
            var tours = _context.Tours
                .Include(t => t.Pais)
                .Include(t => t.Destino)
                .Include(t => t.CategoriaTour)
                .ToList();

            return Ok(tours);
        }
        [HttpGet("{id}")]
        public IActionResult GetTour(int id)
        {
            var tour = _context.Tours
                .Include(t => t.Pais)
                .Include(t => t.Destino)
                .Include(t => t.CategoriaTour)
                .FirstOrDefault(t => t.Id == id);

            if (tour == null)
            {
                return NotFound();
            }

            return Ok(tour);
        }
        [HttpPost]
        public IActionResult CrearTour(Tour tour)
        {
            _context.Tours.Add(tour);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTour), new { id = tour.Id }, tour);
        }
        [HttpPut("{id}")]
        public IActionResult ActualizarTour(int id, Tour tour)
        {
            if (id != tour.Id)
            {
                return BadRequest();
            }

            _context.Tours.Update(tour);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult EliminarTour(int id)
        {
            var tour = _context.Tours.Find(id);

            if (tour == null)
            {
                return NotFound();
            }

            _context.Tours.Remove(tour);
            _context.SaveChanges();

            return NoContent();
        }
    }
}