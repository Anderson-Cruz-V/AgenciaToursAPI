using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuiasTuristicosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GuiasTuristicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetGuiasTuristicos()
        {
            var guias = _context.GuiasTuristicos.ToList();

            return Ok(guias);
        }

        [HttpGet("{id}")]
        public IActionResult GetGuiaTuristico(int id)
        {
            var guia = _context.GuiasTuristicos.Find(id);

            if (guia == null)
            {
                return NotFound("El guía turístico no existe.");
            }

            return Ok(guia);
        }

        [HttpPost]
        public IActionResult PostGuiaTuristico(GuiaTuristico guiaTuristico)
        {
            try
            {
                _context.GuiasTuristicos.Add(guiaTuristico);
                _context.SaveChanges();

                return CreatedAtAction(
                    nameof(GetGuiaTuristico),
                    new { id = guiaTuristico.Id },
                    guiaTuristico
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al crear el guía turístico."
                );
            }
        }

        [HttpPut("{id}")]
        public IActionResult PutGuiaTuristico(int id, GuiaTuristico guiaTuristico)
        {
            try
            {
                if (id != guiaTuristico.Id)
                {
                    return BadRequest(
                        "El Id de la URL no coincide con el Id del guía turístico."
                    );
                }

                var guiaExiste = _context.GuiasTuristicos.Any(
                    g => g.Id == id
                );

                if (!guiaExiste)
                {
                    return NotFound("El guía turístico no existe.");
                }

                _context.GuiasTuristicos.Update(guiaTuristico);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al actualizar el guía turístico."
                );
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGuiaTuristico(int id)
        {
            try
            {
                var guia = _context.GuiasTuristicos.Find(id);

                if (guia == null)
                {
                    return NotFound("El guía turístico no existe.");
                }

                _context.GuiasTuristicos.Remove(guia);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "No se puede eliminar el guía porque está relacionado con un tour."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al eliminar el guía turístico."
                );
            }
        }
    }
}