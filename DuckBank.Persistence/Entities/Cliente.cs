using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DuckBank.Persistence.Entities
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int Id { get; set; }

        public string EncodedKey { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Correo { get; set; }

        public string Nombre { get; set; }

        public string Telefono { get; set; }

        public Direccion Direccion { get; set; }

        public bool EstaActivo { get; set; } = true;

        public Dictionary<string, string> Otros { get; set; }

        public DateTime FechaDeNacimiento { get; set; }

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;
        public List<Contacto> Contactos { get; set; } = new List<Contacto>();
    }
}
