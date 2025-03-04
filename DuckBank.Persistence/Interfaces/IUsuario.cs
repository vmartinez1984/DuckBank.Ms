using DuckBank.Persistence.Entities;

namespace DuckBank.Persistence.Interfaces
{
    public interface IUsuario
    {
        Task<Usuario> ObtenerPorCorreoAsync(string correo);
        Task<Usuario> ObtenerPorIdAsync(string idEncodedKey);
    }
}
