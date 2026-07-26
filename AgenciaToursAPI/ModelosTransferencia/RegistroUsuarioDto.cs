using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.ModelosTransferencia;

public class RegistroUsuarioDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Correo { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Clave { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Rol { get; set; } = "Usuario";
}