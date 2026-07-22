using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models;

public class Tour
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Precio { get; set; }

    public decimal Itbis { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan Hora { get; set; }

    [Range(1, int.MaxValue)]
    public int DuracionDias { get; set; }

    [Range(0, 23)]
    public int DuracionHoras { get; set; }

    public DateTime FechaHoraFin { get; set; }

    public string Estado { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int PaisId { get; set; }

    public Pais Pais { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int DestinoId { get; set; }

    public Destino Destino { get; set; } = null!;

    [Range(1, int.MaxValue)]
    public int CategoriaTourId { get; set; }

    public CategoriaTour CategoriaTour { get; set; } = null!;
}