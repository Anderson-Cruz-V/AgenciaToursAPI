using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string ClaveHash { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Rol { get; set; } = "Usuario";

    public bool Activo { get; set; } = true;
}