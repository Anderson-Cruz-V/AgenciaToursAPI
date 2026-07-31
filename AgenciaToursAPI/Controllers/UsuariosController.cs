using AgenciaToursAPI.Data;
using AgenciaToursAPI.Models;
using AgenciaToursAPI.ModelosTransferencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgenciaToursAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpGet]
        public IActionResult ObtenerUsuarios()
        {
            try
            {
                var usuarios = _context.Usuarios
                    .Select(usuario => new
                    {
                        usuario.Id,
                        usuario.Nombre,
                        usuario.Correo,
                        usuario.Rol,
                        usuario.Activo
                    })
                    .ToList();

                return Ok(usuarios);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al consultar los usuarios."
                );
            }
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerUsuario(int id)
        {
            try
            {
                var usuario = _context.Usuarios
                    .Where(usuario => usuario.Id == id)
                    .Select(usuario => new
                    {
                        usuario.Id,
                        usuario.Nombre,
                        usuario.Correo,
                        usuario.Rol,
                        usuario.Activo
                    })
                    .FirstOrDefault();

                if (usuario == null)
                {
                    return NotFound("El usuario no fue encontrado.");
                }

                return Ok(usuario);
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al consultar el usuario."
                );
            }
        }

        [HttpPost]
        public IActionResult CrearUsuario(
            RegistroUsuarioDto datosUsuario
        )
        {
            try
            {
                string correoNormalizado = datosUsuario.Correo
                    .Trim()
                    .ToLower();

                bool correoExiste = _context.Usuarios.Any(
                    usuario =>
                        usuario.Correo.ToLower() == correoNormalizado
                );

                if (correoExiste)
                {
                    return BadRequest(
                        "Ya existe un usuario registrado con ese correo."
                    );
                }

                string rol = datosUsuario.Rol.Trim();

                if (rol != "Administrador" && rol != "Usuario")
                {
                    return BadRequest(
                        "El rol solamente puede ser Administrador o Usuario."
                    );
                }

                var usuario = new Usuario
                {
                    Nombre = datosUsuario.Nombre.Trim(),
                    Correo = correoNormalizado,
                    Rol = rol,
                    Activo = true
                };

                usuario.ClaveHash = _passwordHasher.HashPassword(
                    usuario,
                    datosUsuario.Clave
                );

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return CreatedAtAction(
                    nameof(ObtenerUsuario),
                    new { id = usuario.Id },
                    new
                    {
                        mensaje = "Usuario creado correctamente.",
                        usuario.Id,
                        usuario.Nombre,
                        usuario.Correo,
                        usuario.Rol,
                        usuario.Activo
                    }
                );
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error al guardar el usuario."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al crear el usuario."
                );
            }
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarUsuario(
            int id,
            RegistroUsuarioDto datosUsuario
        )
        {
            try
            {
                var usuarioExistente = _context.Usuarios.Find(id);

                if (usuarioExistente == null)
                {
                    return NotFound("El usuario no fue encontrado.");
                }

                string correoNormalizado = datosUsuario.Correo
                    .Trim()
                    .ToLower();

                bool correoDuplicado = _context.Usuarios.Any(
                    usuario =>
                        usuario.Correo.ToLower() == correoNormalizado &&
                        usuario.Id != id
                );

                if (correoDuplicado)
                {
                    return BadRequest(
                        "Ya existe otro usuario registrado con ese correo."
                    );
                }

                string rol = datosUsuario.Rol.Trim();

                if (rol != "Administrador" && rol != "Usuario")
                {
                    return BadRequest(
                        "El rol solamente puede ser Administrador o Usuario."
                    );
                }

                usuarioExistente.Nombre = datosUsuario.Nombre.Trim();
                usuarioExistente.Correo = correoNormalizado;
                usuarioExistente.Rol = rol;

                usuarioExistente.ClaveHash =
                    _passwordHasher.HashPassword(
                        usuarioExistente,
                        datosUsuario.Clave
                    );

                _context.SaveChanges();

                return Ok(
                    new
                    {
                        mensaje = "Usuario actualizado correctamente.",
                        usuarioExistente.Id,
                        usuarioExistente.Nombre,
                        usuarioExistente.Correo,
                        usuarioExistente.Rol,
                        usuarioExistente.Activo
                    }
                );
            }
            catch (DbUpdateException)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error al actualizar el usuario."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al actualizar el usuario."
                );
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                var usuario = _context.Usuarios.Find(id);

                if (usuario == null)
                {
                    return NotFound("El usuario no fue encontrado.");
                }

                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(
                    "No se puede eliminar el usuario porque tiene información relacionada."
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al eliminar el usuario."
                );
            }
        }
    }
}