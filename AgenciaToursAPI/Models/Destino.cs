using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models;

public class Destino
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public int PaisId { get; set; }
    public Pais Pais { get; set; } = null!;
}
