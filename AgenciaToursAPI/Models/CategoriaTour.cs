using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models;

public class CategoriaTour
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
}