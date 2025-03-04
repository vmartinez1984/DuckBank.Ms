using DuckBank.Persistence.Entities;

namespace DuckBank.Persistence.Interfaces
{
    public interface IClienteRepositorio
    {
        Task ActualizarAsync(Cliente item);
        Task<int> AgregarAsync(Cliente item);
        Task<Cliente> ObtenerPorCorreoAsync(string correo);
        Task<Cliente> ObtenerPorIdAsync(string id);
        Task<Cliente> ObtenerPorOtrosAsync(string llave, string valor);
        Task<Paginado<Cliente>> ObtenerTodosAsync(int numeroDePagina, int numeroDeRegistros, string textoABuscar);
    }
}
