using DuckBank.Persistence.Entities;

namespace DuckBank.Core.Interfaces
{
    public interface IClienteRepositorio
    {
        Task ActualizarAsync(Cliente item);
        Task<int> AgregarAsync(Cliente item);
        Task<Cliente> ObtenerPorCorreoAsync(string correo);
        Task<Cliente> ObtenerPorIdAsync(string id);
        Task<Cliente> ObtenerPorOtrosAsync(string llave, string valor);
        Task<List<Cliente>> ObtenerTodosAsync();
    }
}
