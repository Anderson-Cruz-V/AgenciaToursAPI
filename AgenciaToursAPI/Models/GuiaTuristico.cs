using System.ComponentModel.DataAnnotations;

namespace AgenciaToursAPI.Models
{
    public class GuiaTuristico
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Especialidad { get; set; } = string.Empty;
    }
}