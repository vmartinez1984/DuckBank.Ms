using DuckBank.Persistence.Entities;

namespace DuckBank.Persistence.Interfaces
{
    public interface IContactoRepositorio
    {
        Task<int> AgregarAsync(string clienteId, Contacto contacto);

        Task ActualizarAsync(string clienteId, Contacto contacto);

        Task BorrarAsync(string clienteId, string contactoId);

        Task<List<Contacto>> ObtenerAsync(string clienteId);
    }
}
