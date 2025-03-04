using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DuckBank.Persistence.Entities
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int Id { get; set; }

        public string EncodedKey { get; set; }

        public string Nombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string Contraseña { get; set; }
        public Role Role { get; set; }
        public bool EstaActivo { get; set; } = true;
        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        public string Correo { get; set; }
    }

    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }

        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        public bool EstaActivo { get; set; } = true;
    }
}
