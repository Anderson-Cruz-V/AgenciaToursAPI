

namespace AgenciaToursAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Usuario> _passwordHasher;

        public AuthController(
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Usuario>();
        }

        [HttpPost("registro")]
        public IActionResult Registrar(RegistroUsuarioDto datosRegistro)
        {
            try
            {
                string correoNormalizado = datosRegistro.Correo
                    .Trim()
                    .ToLower();

                bool correoExiste = _context.Usuarios.Any(
                    u => u.Correo.ToLower() == correoNormalizado
                );

                if (correoExiste)
                {
                    return BadRequest(
                        "Ya existe un usuario registrado con ese correo."
                    );
                }

                string rol = datosRegistro.Rol.Trim();

                if (rol != "Administrador" && rol != "Usuario")
                {
                    return BadRequest(
                        "El rol solamente puede ser Administrador o Usuario."
                    );
                }

                var usuario = new Usuario
                {
                    Nombre = datosRegistro.Nombre.Trim(),
                    Correo = correoNormalizado,
                    Rol = rol,
                    Activo = true
                };

                usuario.ClaveHash = _passwordHasher.HashPassword(
                    usuario,
                    datosRegistro.Clave
                );

                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                return Created(
                    string.Empty,
                    new
                    {
                        mensaje = "Usuario registrado correctamente.",
                        usuario.Id,
                        usuario.Nombre,
                        usuario.Correo,
                        usuario.Rol,
                        usuario.Activo
                    }
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al registrar el usuario."
                );
            }
        }

        [HttpPost("login")]
        public IActionResult IniciarSesion(LoginUsuarioDto datosLogin)
        {
            try
            {
                string correoNormalizado = datosLogin.Correo
                    .Trim()
                    .ToLower();

                var usuario = _context.Usuarios.FirstOrDefault(
                    u => u.Correo.ToLower() == correoNormalizado
                );

                if (usuario == null)
                {
                    return Unauthorized(
                        "El correo o la contraseña son incorrectos."
                    );
                }

                if (!usuario.Activo)
                {
                    return Unauthorized(
                        "El usuario se encuentra inactivo."
                    );
                }

                PasswordVerificationResult resultadoClave =
                    _passwordHasher.VerifyHashedPassword(
                        usuario,
                        usuario.ClaveHash,
                        datosLogin.Clave
                    );

                if (resultadoClave == PasswordVerificationResult.Failed)
                {
                    return Unauthorized(
                        "El correo o la contraseña son incorrectos."
                    );
                }

                string token = GenerarToken(usuario);

                return Ok(
                    new
                    {
                        mensaje = "Inicio de sesión correcto.",
                        token,
                        usuario = new
                        {
                            usuario.Id,
                            usuario.Nombre,
                            usuario.Correo,
                            usuario.Rol
                        }
                    }
                );
            }
            catch (Exception)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno al iniciar sesión."
                );
            }
        }

        private string GenerarToken(Usuario usuario)
        {
            string claveJwt = _configuration["Jwt:Clave"]
                ?? throw new InvalidOperationException(
                    "La clave JWT no está configurada."
                );

            string emisor = _configuration["Jwt:Emisor"]
                ?? throw new InvalidOperationException(
                    "El emisor JWT no está configurado."
                );

            string audiencia = _configuration["Jwt:Audiencia"]
                ?? throw new InvalidOperationException(
                    "La audiencia JWT no está configurada."
                );

            int duracionMinutos = int.Parse(
                _configuration["Jwt:DuracionMinutos"] ?? "60"
            );

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()
                ),

                new Claim(
                    ClaimTypes.Name,
                    usuario.Nombre
                ),

                new Claim(
                    ClaimTypes.Email,
                    usuario.Correo
                ),

                new Claim(
                    ClaimTypes.Role,
                    usuario.Rol
                )
            };

            var clave = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(claveJwt)
            );

            var credenciales = new SigningCredentials(
                clave,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: emisor,
                audience: audiencia,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(duracionMinutos),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}