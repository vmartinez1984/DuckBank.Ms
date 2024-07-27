using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckBank.Api.Entidades
{
    [BsonIgnoreExtraElements]
    public class Movimiento
    {
        public decimal Cantidad { get; set; }

        //[DataMember]
        // public string Id { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public string Concepto { get; set; }

        public string Referencia { get; set; }
    }
}