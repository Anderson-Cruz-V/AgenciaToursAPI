using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models
{
    public class Transporte
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Placa { get; set; } = string.Empty;

        [Range(1, 100)]
        public int Capacidad { get; set; }
    }
}