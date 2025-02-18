namespace DuckBank.Core.Dtos
{
    public class IdDto
    {
        public int Id { get; set; }

        public string EncodedKey { get; set; }

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;
    }
}
