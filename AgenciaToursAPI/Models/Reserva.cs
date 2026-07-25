using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models;

public class Reserva
{
    public int Id { get; set; }

    public DateTime FechaReserva { get; set; } = DateTime.Now;

    [Range(1, 100)]
    public int CantidadPersonas { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Total { get; set; }

    [Required]
    [StringLength(30)]
    public string Estado { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ClienteId { get; set; }

    public Cliente? Cliente { get; set; }

    [Range(1, int.MaxValue)]
    public int TourId { get; set; }

    public Tour? Tour { get; set; }
}



