namespace DuckBank.Persistence.Entities
{
    public class Contacto
    {
        public string Nombre { get; set; }

        public string Cuenta { get; set; }

        public string Alias { get; set; }

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        public string EncodedKey { get; set; }

        public int Id { get; internal set; }

        public bool EstaActivo { get; set; } = true;
    }
}
