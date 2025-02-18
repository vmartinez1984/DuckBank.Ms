using System.ComponentModel.DataAnnotations;

namespace DuckBank.Core.Dtos
{
    public class DireccionDto
    {
        [Required]
        [MaxLength(120)]
        public string Alcaldia { get; set; }

        [Required]
        [MaxLength(120)]
        public string CalleYNumero { get; set; }

        [Required]
        [MaxLength(5)]
        [MinLength(5)]
        public string CodigoPostal { get; set; }

        [Required]
        [MaxLength(120)]
        public string Colonia { get; set; }

        [Required]
        [MaxLength(120)]
        public string CoordenadasGps { get; set; }

        [Required]
        [MaxLength(50)]
        public string Estado { get; set; }

        [Required]
        [MaxLength(120)]
        public string Referencia { get; set; }
    }
}
