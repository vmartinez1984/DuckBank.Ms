using System.ComponentModel.DataAnnotations;

namespace DuckBank.Core.Dtos
{
    public class ClienteDtoIn
    {
        [Required]
        [MaxLength(120)]
        public string PrimerApellido { get; set; }
                
        [MaxLength(120)]
        public string SegundoApellido { get; set; }

        [Required]
        [MaxLength(50)]
        public string Correo { get; set; }

        //[Required]
        public string EncodedKey { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(10)]
        [MinLength(10)]
        public string Telefono { get; set; }

        [Required]
        public DireccionDto Direccion { get; set; }

        public Dictionary<string, string> Otros { get; set; }
    }

    public class ClienteDto : ClienteDtoIn
    {
        public int Id { get; set; }
    }
}
