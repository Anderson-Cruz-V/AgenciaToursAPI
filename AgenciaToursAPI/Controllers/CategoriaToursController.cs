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
    public class CategoriaToursController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriaToursController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetCategoriaTours()
        {
            var categorias = _context.CategoriaTours.ToList();

            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public IActionResult GetCategoriaTour(int id)
        {
            var categoria = _context.CategoriaTours.Find(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPost]
        public IActionResult CrearCategoriaTour(CategoriaTour categoriaTour)
        {
            _context.CategoriaTours.Add(categoriaTour);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetCategoriaTour),
                new { id = categoriaTour.Id },
                categoriaTour
            );
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarCategoriaTour(int id, CategoriaTour categoriaTour)
        {
            if (id != categoriaTour.Id)
            {
                return BadRequest();
            }

            _context.CategoriaTours.Update(categoriaTour);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarCategoriaTour(int id)
        {
            var categoria = _context.CategoriaTours.Find(id);

            if (categoria == null)
            {
                return NotFound();
            }

            _context.CategoriaTours.Remove(categoria);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
