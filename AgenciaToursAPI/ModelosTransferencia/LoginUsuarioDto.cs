using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.ModelosTransferencia;

public class LoginUsuarioDto
{
    [Required]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Clave { get; set; } = string.Empty;
}