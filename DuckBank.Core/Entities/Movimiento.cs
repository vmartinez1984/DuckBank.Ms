using MongoDB.Bson.Serialization.Attributes;

namespace DuckBank.Core.Entities
{
    [BsonIgnoreExtraElements]
    public class Movimiento
    {
        public decimal Cantidad { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public string Concepto { get; set; }

        public string EncodedKey { get; set; }

        public decimal SaldoInicial { get; set; }

        public decimal SaldoFinal { get; set; }
        public int Id { get; set; }
    }
}
