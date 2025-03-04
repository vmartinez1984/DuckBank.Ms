namespace DuckBank.Persistence.Entities
{
   public class Paginado<T>
    {
        public int NumeroDePagina { get; set; }

        public int RegistrosPorPagina { get; set; }

        public string TextABuscar { get; set; }

        public int TotalDeRegistros { get; set; }

        public int TotalDeRegistrosFiltrados { get; set; }

        public List<T> Lista { get; set; }
    }
}
