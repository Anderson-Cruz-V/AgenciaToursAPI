using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Correo { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    [Range(18, 100)]
    public int Edad { get; set; }
}