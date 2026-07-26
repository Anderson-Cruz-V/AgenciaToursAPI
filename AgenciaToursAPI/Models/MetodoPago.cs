using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models
{
    public class MetodoPago
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; }
    }
}