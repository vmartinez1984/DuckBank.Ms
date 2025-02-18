using DuckBank.Core.Entities;

namespace DuckBank.Core.Interfaces.Repositories
{
    public interface IClienteRepositorio
    {
        Task ActualizarAsync(Cliente item);
        Task<int> AgregarAsync(Cliente item);
        Task<Cliente> ObtenerPorCorreoAsync(string correo);
        Task<Cliente> ObtenerPorIdAsync(string id);
        Task<List<Cliente>> ObtenerTodosAsync();
    }
}
