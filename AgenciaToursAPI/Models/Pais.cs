using System.ComponentModel.DataAnnotations;
namespace AgenciaToursAPI.Models;

public class Pais
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
}