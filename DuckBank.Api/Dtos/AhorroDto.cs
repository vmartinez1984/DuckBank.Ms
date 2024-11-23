using System.ComponentModel.DataAnnotations;

namespace DuckBank.Api.Dtos
{

    public class AhorroConDetalleDto : AhorroDtoIn
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public List<MovimientoDto> Depositos { get; set; }
        public List<MovimientoDto> Retiros { get; set; }
        public string Estado { get;  set; }
        public DateTime FechaDeRegistro { get; internal set; }
    }

    public class MovimientoDto
    {
        public decimal Cantidad { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public string Concepto { get; set; }

        public string Referencia { get; set; }

        public string Id { get; set; }

        public decimal SaldoInicial { get; set; }

        public decimal SaldoFinal { get; set; }
    }

    public class MovimientoDtoIn
    {
        public decimal Cantidad { get; set; }

        public string Concepto { get; set; }

        public string Referencia { get; set; }        
    }

    public class AhorroDto : AhorroDtoIn
    {
        public int Id { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; internal set; }
        public DateTime FechaDeRegistro { get; internal set; }
    }

    public class AhorroDtoIn
    {
        [Required]
        [MaxLength(36)]
        public string Guid { get; set; }

        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "EL numero de cliente es un dato obligatorio")]
        [MaxLength(50)]
        public string ClienteId { get; set; }

        public decimal Interes { get; set; } = 0;

        public Dictionary<string, string> Otros { get; set; } = new Dictionary<string, string>();
    }
}